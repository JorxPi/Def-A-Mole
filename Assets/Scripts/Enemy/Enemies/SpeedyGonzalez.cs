using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedyGonzalez : Enemy
{
    protected override void Start()
    {
        maxHealth = 50f;
        speed = 4f;
        damageAmount = 5f;
        goldReward = 75;
        
        base.Start();
    }
}
