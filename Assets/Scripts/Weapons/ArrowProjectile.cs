using System;
using System.Collections;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class ArrowProjectile : MonoBehaviour
    {
        public BowWeapon owner;
        private void OnTriggerEnter(Collider other)
        {
            AliveEntity ent;
            if (!(ent = other.GetComponent<Mob>())) return;

            var damageInfo = new DamageInfo
            {
                Damage = owner.Damage,
                CriticalChance = owner.CriticalChance,
                CriticalMultiplier = owner.CriticalMultiplier
            };
            ent.ApplyDamage(damageInfo);
        }

        private IEnumerator DestroyOnEnd()
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
        
        private void Awake()
        {
            StartCoroutine(DestroyOnEnd());
        }
    }
}