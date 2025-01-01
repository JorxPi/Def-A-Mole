using UnityEngine;

public class BasicUH : UpgradeHandler
{
    [Header("Upgrade Costs")]
    public int[] upgradeCosts;

    protected override void Awake()
    {
        base.Awake();
    }

    public override bool Upgrade()
    {
        if (currentUpgradeLevel < upgradeCosts.Length)
        {
            int upgradeCost = upgradeCosts[currentUpgradeLevel];

            if (CurrencyManager.Instance.HasEnoughGold(upgradeCost))
            {
                CurrencyManager.Instance.SpendGold(upgradeCost);

                // Apply upgrade logic based on level
                ApplyUpgrade(currentUpgradeLevel);

                currentUpgradeLevel++;
                Debug.Log($"{defense.gameObject.name} upgraded to level {currentUpgradeLevel}.");
                return true;
            }
            else
            {
                Debug.Log("Not enough gold to upgrade.");
                return false;
            }
        }
        else
        {
            Debug.Log("Max upgrade level reached.");
            return false;
        }
    }

    public string GetUpgradeDescription()
    {
        if (currentUpgradeLevel < upgradeCosts.Length)
        {
            return $"Upgrade {defense.baseStats.defenseName} to Level {currentUpgradeLevel + 1}";
        }
        else
        {
            return $"Max upgraded {defense.baseStats.defenseName}";
        }
    }

    protected virtual void ApplyUpgrade(int level)
    {
        switch (level)
        {
            case 0:
                defense.attackDamage += 5; // +5 attack damage for first upgrade
                break;
            case 1:
                defense.attackSpeed += 2; // +2 attack speed for second upgrade
                break;
        }
    }

    public virtual string GetUpgradeDetails()
    {
        if (currentUpgradeLevel < upgradeCosts.Length)
        {
            int upgradeCost = upgradeCosts[currentUpgradeLevel];
            switch (currentUpgradeLevel)
            {
                case 0:
                    return $"Upgrade cost: {upgradeCost}\nAttack Damage increased by 5";
                case 1:
                    return $"Upgrade cost: {upgradeCost}\nAttack Speed increased by 2";
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

