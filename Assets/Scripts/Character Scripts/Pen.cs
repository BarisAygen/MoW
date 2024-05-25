public class Pen : CharacterBase {
    private float damage = 50f;

    public void Awake()
    {
        setName("Pen");
        setHealth(830);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 3.0f / 10.0f)
        {
            targetEnemy.TakeDamage(damage, this);
            this.Heal(damage);
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