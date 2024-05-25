public class Elektro : CharacterBase {
    private float damage = 60f;

    public void Awake()
    {
        setName("Elektro");
        setHealth(400);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 3.0f / 10.0f)
        {
            targetEnemy.TakeDamage(damage, this);
            this.damage += 50;
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