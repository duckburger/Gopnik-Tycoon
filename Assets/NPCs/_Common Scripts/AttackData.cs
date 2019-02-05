using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName ="Gopnik/AttackData")]
public class AttackData : ScriptableObject
{
    public string name;
    public float staminaCost;
    public string animStateName; // Used to pass to the animator
    public float damage;
    public float cooldown = 1.2f;
    public AttackType type;
}
