public class Bobdiot : CharacterBase {
    private float damage = 100f;

    public void Awake()
    {
        setName("Bobdiot");
        setHealth(800);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 3.0f / 4.0f)
        {
            targetEnemy.TakeDamage(damage, this);
        }

        else 
        {
            this.TakeDamage(damage, this);
            targetEnemy.ShowMissedAndMove();
        }

        targetEnemy = null;
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}
