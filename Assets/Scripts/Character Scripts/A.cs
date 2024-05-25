using System.Collections;
using UnityEngine;

public class A : CharacterBase {
    private float damage = 30f;

    public void Awake()
    {
        setName("A");
        setHealth(470);
        setSize(1);
        setClass('B');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null)
        {
            StartCoroutine(AttemptSkillWithDelay());
        }
    }

    private IEnumerator AttemptSkillWithDelay()
    {
        if (UnityEngine.Random.value < 3.0f / 5.0f)
        {
            targetEnemy.TakeDamage(damage, this);
            yield return new WaitForSeconds(0.2f);
            PerformSkill();
        }

        else 
        {
            targetEnemy.ShowMissedAndMove();
            targetEnemy = null;
        }
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}