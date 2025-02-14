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
    
    var targeting = gameObject.AddComponent<EnemyTargeting>();
    targeting.SetDetectionRange(8f);
    targeting.SetStopDistance(1.9f);
    targeting.SetSafetyMargin(0.3f);
    targeting.SetSmoothTime(0.3f);
    targeting.SetRotationSpeed(8f);
    targeting.SetAttackInterval(0.5f); // Ataque más rápido pero más débil
    
    base.Start();
}
}
