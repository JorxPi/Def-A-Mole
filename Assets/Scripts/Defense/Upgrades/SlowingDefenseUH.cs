using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingDefenseUH : BasicUH
{
    [Header("Projectile Reference")]
    public SlowingProjectile slowingProjectile;

    protected override void ApplyUpgrade(int level)
    {
        switch (level)
        {
            case 0:
                slowingProjectile.explosionRadius += 0.5f;
                break;
            case 1:
                slowingProjectile.slowAmount += 10f; 
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
                    return $"Upgrade cost: {upgradeCost}\nExplosion Radius increased by 0.5";
                case 1:
                    return $"Upgrade cost: {upgradeCost}\nSlow Amount increased by 0.2";
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
