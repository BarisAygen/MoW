public class Markov : CharacterBase {
    private float damage = 330f;

    public void Awake()
    {
        setName("Markov");
        setHealth(440);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 1.0f / 4.0f)
        {
            targetEnemy.TakeDamage(damage, this);
        }

        else
        {
            targetEnemy.ShowMissedAndMove();
        }

        targetEnemy = null;
    }

    protected override void Die(CharacterBase killer)
    {
        base.Die(killer);

        if (killer != null)
        {
            killer.Heal(100);
        }
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}