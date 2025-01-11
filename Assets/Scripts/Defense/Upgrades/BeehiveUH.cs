using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveUH : BasicUH
{
    protected override void ApplyUpgrade(int level)
    {
        switch (level)
        {
            case 0:
                defense.attackRange += 2;
                break;
            case 1:
                defense.attackSpeed += 0.2f;
                break;
        }
    }

    public override string GetUpgradeDetails()
    {
        if (defense.baseStats != null && currentUpgradeLevel < defense.baseStats.upgradeCosts.Length)
        {
            int upgradeCost = defense.baseStats.upgradeCosts[currentUpgradeLevel];
            switch (currentUpgradeLevel)
            {
                case 0:
                    return $"Upgrade cost: {upgradeCost}\nAttack Range increased by 2";
                case 1:
                    return $"Upgrade cost: {upgradeCost}\nAttack Speed increased by 0.2";
                default:
                    return "";
            }
        }
        else
        {
            return "";
        }
    }
}
