using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeHandler : MonoBehaviour
{
    protected Defense defense;
    protected int currentUpgradeLevel = 0;

    public abstract bool Upgrade();

    protected virtual void Awake()
    {
        defense = GetComponent<Defense>();
        if (defense == null)
        {
            Debug.LogError("UpgradeHandler requires a Defense component.");
        }
    }
}
