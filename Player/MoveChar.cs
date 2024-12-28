using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MoveChar : MonoBehaviour
{
    public CharacterController controller;
    public Animator PlayerAnimator;
    public PlayerData Data;
    public float turnSmoothTime = 0.1f;
    public PlayerInput InputAction;
    public float jumpForce = 7.0f;
    CharacterStats characterStats;
    private float turnSmoothVelocity;
    private Vector3 direction;
    public GameObject playerCamera;

    private float velocityY,velocityZ,velocityX;
    private bool canJump = true; // Flag to track whether the character can jump
    Vector3 moveVector, jumpVector;

    private float gravity = -15f;
    private bool walkToggle, combatToggle;

    public const float maxDashTime = 1.0f;
    public float dashDistance = 10;
    public float dashCooldownSeconds = 3f;
    private int dashCount = 0;
    public bool isDashing = false;
    private float dashFireTime = 0;


    private float highestJumpPosition = 0;
    private float currentJumpPosition = 0;

    [Header("UI")]
    [SerializeField] UIController ui;
    public bool isCanvasActive;
    [Header("Sword")]
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClick = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 2;
    private bool canDrawSword = true;
    private string cheatCode = "23-1"; // Define your cheat code here
    private string inputString = "";
    private bool isSheatingSword;

    [Header("Cheat")]
    public GameObject cheatCanvas;
    public TextMeshProUGUI cheatText;

    private void Awake()
    {
        InputAction = new PlayerInput();
        
    }
    private void Start()
    {
        //Cursor.visible = false;
        isSheatingSword = false;
        ui = FindObjectOfType<UIController>();
        characterStats = CharacterStats.Instance.GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        InputAction.Gameplay.Move.Enable();
        InputAction.Gameplay.Jump.Enable();
        InputAction.Gameplay.WalkToggle.Enable();
        InputAction.Gameplay.Dash.Enable();
        InputAction.Gameplay.CombatToggle.Enable();
        AddInputCallBack();
    }

    private void OnDisable()
    {
        RemoveInputCallback();
    }
    void Update()
    {
        isCanvasActive = ui.isSherylCanvas || ui.isNobolCanvas || ui.isRobertCanvas || isSheatingSword==true;
        if (!isCanvasActive)
        {
            playerCamera.SetActive(true);
            ApplyGravity();
            float horizontalInput = InputAction.Gameplay.Move.ReadValue<Vector2>().x;
            float verticalInput = InputAction.Gameplay.Move.ReadValue<Vector2>().y;
            direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            currentJumpPosition = transform.position.y;

            if(characterStats.getHealth() <= 0)
            {
                //PlayerAnimator.SetTrigger("death");
                PlayerAnimator.Play("Death");
            }

            if (currentJumpPosition > highestJumpPosition)
            {
                highestJumpPosition = currentJumpPosition;
            }
            Move();
            if (combatToggle)
            {
                if (noOfClick==1)
                {
                    PlayerAnimator.SetBool("hit1", false);
                }
                else if (noOfClick==2)
                {
                    PlayerAnimator.SetBool("hit2", false);
                }
                else if (noOfClick>=3)
                {
                    Debug.Log("Stop anim");
                    PlayerAnimator.SetBool("hit3", false);
                    noOfClick = 0;
                }


                if (Time.time - lastClickedTime > maxComboDelay)
                {
                    noOfClick = 0;
                }

                //cooldown time
                if (Time.time > nextFireTime)
                {
                    // Check for mouse input
                    if (Input.GetMouseButtonDown(0))
                    {
                        OnClick();

                    }
                }
            }
            CheckCheatCode();
        }
        else
        {
            direction = Vector3.zero;
            velocityX = 0f;
            velocityY = 0f;
            velocityZ = 0f;
            playerCamera.SetActive(false);
        }

    }
    private void ApplyGravity()
    {
        moveVector = Vector3.zero;

        if (!controller.isGrounded)
        {
            velocityY += gravity * Time.deltaTime;
        }
        else
        {
            velocityY = -1f;
        }

        moveVector += new Vector3(0f, velocityY, 0f);

        controller.Move(moveVector * Time.deltaTime);
    }



    private void Move()
    {
        Vector3 moveDir;
        PlayerAnimator.SetBool("isWalking", false);
        PlayerAnimator.SetBool("isRunning", false);
        PlayerAnimator.SetBool("isJumping", false);
        PlayerAnimator.SetBool("isDashing", false);
        PlayerAnimator.SetBool("isCombatRun", false);
        
        if (controller.isGrounded == true || direction.magnitude == 0.0f)
        {
            canJump = true;
        }
        

        if (InputAction.Gameplay.Jump.triggered && canJump)
        {
         
            PlayerAnimator.SetBool("isJumping", InputAction.Gameplay.Jump.triggered);
            Jump();
            PlayerAnimator.SetBool("isHardLand", false);

        }
        if (dashCount==2 && Time.time >= dashFireTime)
        {
            dashCount = 0; // Reset dash count
        }

        if (InputAction.Gameplay.Dash.triggered && direction.magnitude != 0.0f && canJump==true)
        {
            StartCoroutine(Dash());
        }

        if (combatToggle && canDrawSword == true)
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerAnimator.Play("Draw Sword 1");
                canDrawSword = false;
            }
            
        }
        else if(!combatToggle && canDrawSword == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerAnimator.Play("Sheath Sword");
                canDrawSword = true;
            }
        }
        

        if (direction.magnitude >= 0.1f)
        {
            jumpForce = 5f;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply gravity to the vertical component
            velocityZ = moveDir.z;  
            velocityX = moveDir.x;

            if (walkToggle && !combatToggle)
            {
                PlayerAnimator.SetBool("isWalking", walkToggle);
                PlayerAnimator.SetBool("isRunning", !walkToggle);
                controller.Move(Data.baseSpeed * Data.walkSpeed * Time.deltaTime * moveDir.normalized);
            }
            else if (combatToggle)
            {
                     
                PlayerAnimator.SetBool("isCombatRun", combatToggle);
                controller.Move(Data.baseSpeed * Data.combatRunSpeed * Time.deltaTime * moveDir.normalized);
            }
            else
            {
                PlayerAnimator.SetBool("isWalking", walkToggle);
                PlayerAnimator.SetBool("isRunning", !walkToggle);
                controller.Move(Data.baseSpeed * Data.nonCombatRunSpeed * Time.deltaTime * moveDir.normalized);
            }
        }
    }

    private void Jump()
    {
        if (!isGrounded()) return;

        velocityY += jumpForce * 2;
        jumpVector.y = velocityY;
        controller.Move(jumpVector * Time.deltaTime);

        canJump = false;

       
        if (highestJumpPosition - currentJumpPosition > 3)
        {
            PlayerAnimator.SetBool("isHardLand", true);
        }
        
        highestJumpPosition = transform.position.y;
    }



    private void AddInputCallBack()
    {
        InputAction.Gameplay.WalkToggle.performed += OnWalkTogglePerformed;
        InputAction.Gameplay.CombatToggle.performed += OnCombatTogglePerformed;

    }

    private void RemoveInputCallback()
    {
        InputAction.Gameplay.WalkToggle.performed -= OnWalkTogglePerformed;
        InputAction.Gameplay.CombatToggle.performed -= OnCombatTogglePerformed;

    }
    private void OnWalkTogglePerformed(InputAction.CallbackContext context)
    {
        walkToggle = !walkToggle;
    }

    private void OnCombatTogglePerformed(InputAction.CallbackContext context)
    {
        combatToggle = !combatToggle;
    }

    IEnumerator Dash()
    {
        if (dashCount > 2 || Time.time < dashFireTime) yield break;
        PlayerAnimator.SetBool("isDashing", InputAction.Gameplay.Dash.triggered);
        float startTime = Time.time;
        Vector3 dashVector = Vector3.zero;
        
        while (Time.time < startTime + 0.2f)
        {
            dashVector += new Vector3(velocityX, velocityY, velocityZ); // Accumulate the dash vector
            controller.Move(dashVector.normalized *Data.dashSpeed* Time.deltaTime);
            yield return null;
        }
        dashCount++;
        if (dashCount == 2)
        {
            dashFireTime = Time.time + dashCooldownSeconds; // Update dash fire time for cooldown
        }

    }

    void OnClick()
    {

        // Record the time of the last click
        lastClickedTime = Time.time;

        // Increment the click counter
        noOfClick++;

        // Clamp the number of clicks between 0 and 3
        noOfClick = Mathf.Clamp(noOfClick, 0, 3);

        // Play animations based on the number of clicks
        if (noOfClick == 1)
        {
            Debug.Log("hit 1");
            PlayerAnimator.SetBool("hit1", true);
            CheckHitEnemy();
        }
        else if (noOfClick == 2)
        {
            Debug.Log("hit 2");
            PlayerAnimator.SetBool("hit1", false);
            PlayerAnimator.SetBool("hit2", true);
            CheckHitEnemy();
        }
        else if (noOfClick == 3 )
        {
            Debug.Log("hit 3");
            PlayerAnimator.SetBool("hit2", false);
            PlayerAnimator.SetBool("hit3", true);
            CheckHitEnemy();
        }
    }

    void CheckHitEnemy()
    {
        RaycastHit hit;
        float maxDistance = 20f;
        // Perform raycast from the player towards the forward direction
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // Check if the object hit by the raycast is an enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                // Perform actions when the enemy is hit
                Debug.Log("Enemy hit by the animation!");
                Enemy e = hit.transform.GetComponent<Enemy>();
                if(e!= null)
                {
                    e.TakeDamage((characterStats.getPlayerAttack()));
                    return;
                }
            }
        }
    }

    void CheckCheatCode()
    {
        // Update inputString with the most recent input
        if (Input.inputString.Length > 0)
        {
            char c = Input.inputString[0];
            inputString += c;
        }

        // Check if the inputString contains the cheat code
        if (inputString.Contains(cheatCode))
        {
            StartCoroutine(CheatActivated(cheatCode));
            SceneManager.LoadScene(1); // Load scene if cheat code is found
        }
        if (inputString.ToLower().Contains("depdepdep"))
        {
            StartCoroutine(CheatActivated("DEPDEPDEP"));
            Data.baseSpeed = 25f;
            inputString = "";
        }
        else if (inputString.ToLower().Contains("oobacachat"))
        {
            StartCoroutine(CheatActivated("OOBACACHAT"));
            characterStats.setXp(231231231);
            inputString = "";
        }
        else if (inputString.ToLower().Contains("njus"))
        {
            StartCoroutine(CheatActivated("NJUS"));
            characterStats.addGold(231231231);
            inputString = "";
        }
    }

    IEnumerator CheatActivated(string text)
    {
        cheatText.text = text;
        cheatCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        cheatCanvas.SetActive(false);
    }

    private bool isGrounded() => controller.isGrounded;

}





