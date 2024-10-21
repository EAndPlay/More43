using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float Damage;
    public int CriticalChance;
    public float CriticalMultiplier;
    // private void OnTriggerEnter(Collider other)
    // {
    //     //EnemyEntity mob;
    //     var damageInfo = new DamageInfo { Damage = Damage, CriticalChance = CriticalChance, CriticalMultiplier = CriticalMultiplier};
    // }

    // TODO: leave or make AI/Character.Attack()
    public virtual void Attack() { } // Sword, Bow
}