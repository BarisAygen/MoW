using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class White_Angel : CharacterBase {
    private float healAmountSingle = 300f;
    private float healAmountAll = 100f;
    private float healAmountSelf = 200f;

    private void Awake()
    {
        setName("White_Angel");
        setHealth(230);
        setSize(1);
        setClass('B');
    }

    public override void PerformSkill()
    {
        float actionChance = UnityEngine.Random.value;

        if (actionChance < 1.0f / 3.0f)
        {
            HealSingleTeammate();
        }

        else if (actionChance > 1.0f / 3.0f && actionChance < 2.0f / 3.0f)
        {
            HealAllTeammates();
        }

        else
        {
            SelfHeal();
        }
    }

    private void HealSingleTeammate()
    {
        List<CharacterBase> selectableTeammates = FindObjectsOfType<CharacterBase>()
            .Where(c => c.team == this.team && c != this && c.isSelectable)
            .ToList();

        if (selectableTeammates.Count > 0)
        {
            int randomIndex = Random.Range(0, selectableTeammates.Count);
            CharacterBase randomTeammate = selectableTeammates[randomIndex];
            randomTeammate.Heal(healAmountSingle);
        }
    }

    private void HealAllTeammates()
    {
        foreach (var teammate in FindObjectsOfType<CharacterBase>().Where(c => c.team == this.team))
        {
            if(teammate.isSelectable == true)
            {
                teammate.Heal(healAmountAll);
            }
        }
    }

    private void SelfHeal()
    {
        Heal(healAmountSelf);
    }

    public override string GetSkillDescription()
    {
        return "";
    }
}
