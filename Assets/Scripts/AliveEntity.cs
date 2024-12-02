using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public struct DamageInfo
{
    public static int StaticId = 1;
    
    public int Id;
    public float Damage;
    public int CriticalChance;
    public float CriticalMultiplier;
    public AliveEntity Owner;
}

public abstract class AliveEntity : MonoObject
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool isDead;
    
    private int _lastDamageInfoId;
    
    public event Action<float> HealthChanged;
    public event Action<float> MaxHealthChanged;
    public event Action Died;

    public bool IsDead
    {
        get => isDead;
        protected set => isDead = value;
    }
    public float Health
    {
        get => health;
        set
        {
            HealthChanged?.Invoke(value);
            health = value;
        }
    }
    
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            MaxHealthChanged?.Invoke(value);
            maxHealth = value;
        }
    }
    

    public virtual void Die(AliveEntity killer = null)
    {
        //IsDead = true;
        Died?.Invoke();
    }
    
    public virtual bool ApplyDamage(DamageInfo damageInfo)
    {
        if (damageInfo.Id == _lastDamageInfoId) return false;
        _lastDamageInfoId = damageInfo.Id;
        
        var rolledChance = Random.Range(0, 101);
        if (rolledChance <= damageInfo.CriticalChance)
            damageInfo.Damage *= damageInfo.CriticalMultiplier;
        
        Health -= damageInfo.Damage;
        // if (health <= 0)
        // {
        //     Die(damageInfo.Owner);
        // }
        return true;
    }
}