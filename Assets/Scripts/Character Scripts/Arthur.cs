public class Arthur : CharacterBase {
    private float damage = 240f;

    public void Awake()
    {
        setName("Arthur");
        setHealth(310);
        setSize(1);
        setClass('B');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 1.0f / 3.0f)
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