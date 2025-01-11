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
    
    var targeting = gameObject.AddComponent<EnemyTargeting>();
    targeting.SetDetectionRange(12f);
    targeting.SetStopDistance(2.5f);
    targeting.SetSafetyMargin(1f);
    targeting.SetSmoothTime(0.8f);
    targeting.SetRotationSpeed(3f);
    targeting.SetAttackInterval(1.5f); // Ataque más lento pero más fuerte
    
    base.Start();
}

}
