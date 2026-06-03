using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public Image cooldownMask;
    public float cooldownTime = 5f;

    private bool isCoolingDown = false;
    private float timer = 0f;

    void Start()
    {
        if (cooldownMask != null)
        {
            cooldownMask.fillAmount = 0f;
        }
    }

    void Update()
    {
        if (isCoolingDown)
        {
            timer -= Time.deltaTime;
            if (cooldownMask != null)
            {
                cooldownMask.fillAmount = timer / cooldownTime;
            }

            if (timer <= 0f)
            {
                isCoolingDown = false;
                timer = 0f;
                if (cooldownMask != null)
                {
                    cooldownMask.fillAmount = 0f;
                }
            }
        }
    }

    public void UseSkill() 
    {
        if (isCoolingDown) return;

        Debug.Log("Skill is used!");
        isCoolingDown = true;
        timer = cooldownTime;

        if (cooldownMask != null)
        {
            cooldownMask.fillAmount = 1f;
        }
    }
}