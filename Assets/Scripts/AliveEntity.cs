using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public struct DamageInfo
{
    public float Damage;
    public int CriticalChance;
    public float CriticalMultiplier;
}

public abstract class AliveEntity : MonoObject
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool isDead;
    
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
            health = value;
            HealthChanged?.Invoke(value);
            if (value <= 0)
                Die();
        }
    }
    
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            MaxHealthChanged?.Invoke(value);
        }
    }
    

    public virtual void Die()
    {
        IsDead = true;
        Died?.Invoke();
    }
    
    public void ApplyDamage(DamageInfo damageInfo)
    {
        var rolledCrit = Random.Range(0, 100);
        if (rolledCrit <= damageInfo.CriticalChance)
            damageInfo.Damage *= damageInfo.CriticalMultiplier;

        Health -= damageInfo.Damage;
    }
}