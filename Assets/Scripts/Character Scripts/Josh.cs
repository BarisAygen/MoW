using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Josh : CharacterBase {
    private float damage = 100f;

    public void Awake()
    {
        setName("Josh");
        setHealth(500);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 3.0f / 10.0f)
        {
            targetEnemy.TakeDamage(damage, this);
        }

        else
        {
            PerformAttack(targetEnemy);
            targetEnemy.ShowMissedAndMove();
        }

        targetEnemy = null;
    }

    public void PerformAttack(CharacterBase target)
    {
        StartCoroutine(AttemptAttackAgain(target));
    }


    private IEnumerator AttemptAttackAgain(CharacterBase targetEnemy)
    {
        yield return new WaitForSeconds(1f);

        if (UnityEngine.Random.value < 3.0f / 10.0f)
        {
            targetEnemy.TakeDamage(damage * 2, this);
        }

        else
        {
            targetEnemy.ShowMissedAndMove();
        }

        targetEnemy = null;
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}
