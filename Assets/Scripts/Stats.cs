using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject {
    public RenewableStat hp;
    public RenewableStat mp;

    public int agility;
    public int perception;
    public int attackPower;
    public float attackRadius;
}
