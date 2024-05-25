public class Sharker : CharacterBase {
    private float damage = 160f;

    public void Awake()
    {
        setName("Sharker");
        setHealth(220);
        setSize(1);
        setClass('B');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null)
        {
            if (UnityEngine.Random.value < 1.0f / 4.0f)
            {
                targetEnemy.TakeDamage(damage, this);
            }

            else
            {
                targetEnemy.ShowMissedAndMove();
            }
        }

        targetEnemy = null;
    }

    public override void TakeDamage(float damage, CharacterBase attacker)
    {
        base.TakeDamage(damage, attacker);
        if (getHealth() < 30 && getHealth() > 0)
        {
            setHealth(getMaxHealth());
            UpdateHealthUI();
        }
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}