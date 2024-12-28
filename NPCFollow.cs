using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player; 
    public float interactionRange = 5f;
    public float rotationSpeed = 5f; 
    private Vector3 direction;
    public bool isInInteractRange;
    private void Start()
    {
        isInInteractRange =false;
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= interactionRange)
        {
            isInInteractRange=true;
            direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            isInInteractRange=false;
        }
    }
}
