using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Weapon : MonoObject
{
    public float damage;
    public int criticalChance;
    public float criticalMultiplier;
    public float attackRate;
    public AliveEntity owner;
    
    private Collider _triggerCollider;
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     //EnemyEntity mob;
    //     var damageInfo = new DamageInfo { Damage = Damage, CriticalChance = CriticalChance, CriticalMultiplier = CriticalMultiplier};
    // }

    // TODO: leave or make AI/Character.Attack()
    public virtual void Attack(object[] args = null) { } // Sword, Bow

    private void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider>();
    }
    
    public void Enable()
    {
        _triggerCollider.enabled = true;
    }

    public void Disable()
    {
        _triggerCollider.enabled = false;
    }
}