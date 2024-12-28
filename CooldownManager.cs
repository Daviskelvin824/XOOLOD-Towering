using System.Collections;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    private bool isCooldown = false;

    public void StartCooldown(float cooldownDuration)
    {
        if (!isCooldown)
        {
            StartCoroutine(CooldownCoroutine(cooldownDuration));
        }
    }

    private IEnumerator CooldownCoroutine(float cooldownDuration)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    public bool IsOnCooldown()
    {
        return isCooldown;
    }
}
