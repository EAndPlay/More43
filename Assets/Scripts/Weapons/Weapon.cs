using System;
using UnityEngine;

public abstract class Weapon : MonoObject
{
    public float Damage;
    public int CriticalChance;
    public float CriticalMultiplier;
    public float AttackRate;
    // private void OnTriggerEnter(Collider other)
    // {
    //     //EnemyEntity mob;
    //     var damageInfo = new DamageInfo { Damage = Damage, CriticalChance = CriticalChance, CriticalMultiplier = CriticalMultiplier};
    // }

    // TODO: leave or make AI/Character.Attack()
    public virtual void Attack() { } // Sword, Bow
}