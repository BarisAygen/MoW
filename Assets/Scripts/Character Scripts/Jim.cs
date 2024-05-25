public class Jim : CharacterBase {
    private float damage = 80f;

    public void Awake()
    {
        setName("Jim");
        setHealth(160);
        setSize(1);
        setClass('A');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 2.0f / 3.0f)
        {
            targetEnemy.TakeDamage(damage, this);
            damage += damage;
        }

        else
        {
            damage = 80;
            targetEnemy.ShowMissedAndMove();
        }

        targetEnemy = null;
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}