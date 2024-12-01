using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperDefense : Defense
{
    protected override void Start()
    {
        base.Start();
        attackRange = 15f;    
        attackDamage = 40f;  
        attackSpeed = 0.5f;  
    }
}

