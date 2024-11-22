using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefense", menuName = "Tower Defense/Defense")]
public class DefenseData : ScriptableObject
{
    public string defenseName;
    public int cost;
    public Sprite defenseIcon; 
    public GameObject prefab;
}
