using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntUH : BasicUH
{
    protected override void ApplyUpgrade(int level)
    {
        Damageable damageable = defense.GetComponent<Damageable>();
        switch (level)
        {
            case 0:
                damageable.IncreaseMaxHealth(100); 
                break;
            case 1:
                damageable.IncreaseMaxHealth(150);
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
                    return $"Upgrade cost: {upgradeCost}\nMax Health increased by 100";
                case 1:
                    return $"Upgrade cost: {upgradeCost}\nMax Health increased by 150";
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
