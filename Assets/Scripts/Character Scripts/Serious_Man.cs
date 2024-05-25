public class Serious_Man : CharacterBase {
    private float damage = 100000f;

    public void Awake()
    {
        setName("Serious_Man");
        setHealth(220);
        setSize(1);
        setClass('A');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 1.0f / 8.0f)
        {
            targetEnemy.TakeDamage(damage, this);
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