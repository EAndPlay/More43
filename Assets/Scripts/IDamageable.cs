using System;
using Random = UnityEngine.Random;

public sealed class DamageInfo
{
    public float Damage;
    public int CriticalChance;
    public float CriticalMultiplier;
}

public interface IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        var rolledCrit = Random.Range(0, 100);
        if (rolledCrit < damageInfo.CriticalChance)
            damageInfo.Damage += damageInfo.CriticalMultiplier;

        Health -= damageInfo.Damage;
    }
}