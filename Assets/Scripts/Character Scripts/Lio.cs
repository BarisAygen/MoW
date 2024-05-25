public class Lio : CharacterBase {
    private float damage = 280f;

    public void Awake()
    {
        setName("Lion");
        setHealth(300);
        setSize(1);
        setClass('C');
    }

    public override void PerformSkill()
    {
        if (targetEnemy != null && UnityEngine.Random.value < 3.0f / 10.0f)
        {
            targetEnemy.TakeDamage(damage, this);
            MakeUntargetableForTurns(2);
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