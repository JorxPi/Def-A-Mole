using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefense", menuName = "Tower Defense/Defense")]
public class DefenseData : ScriptableObject
{
    public string defenseName;
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;
    public float maxHealth;
    public int cost;
    public int[] upgradeCosts;
    public Sprite defenseIcon; 
    public GameObject prefab;

    [TextArea(3, 5)]
    public string description;
}
