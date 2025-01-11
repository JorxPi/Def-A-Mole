using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperUH : BasicUH
{
    protected override void ApplyUpgrade(int level)
    {
        switch (level)
        {
            case 0:
                defense.attackRange += 10; 
                break;
            case 1:
                defense.attackDamage += 15;
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
                    return $"Upgrade cost: {upgradeCost}\nAttack Range increased by 10";
                case 1:
                    return $"Upgrade cost: {upgradeCost}\nAttack Damage increased by 15";
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
