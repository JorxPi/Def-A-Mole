using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    protected override void Start()
    {
        maxHealth = 200f;
        speed = 1f;
        damageAmount = 15f;
        goldReward = 150;
        
        base.Start();
    }
}
