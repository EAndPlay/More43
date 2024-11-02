using System;
using System.Collections;
using NPC;
using UnityEngine;

namespace Weapons
{
    public class SwordWeapon : Weapon
    {
        private IEnumerator SpinSword()
        {
            for (var i = 0; i < 18; i++)
            {
                transform.Rotate(i * 10, 0, 0);
                yield return new WaitForSeconds(1 / 18);
            }
        }
        
        public override void Attack()
        {
            StartCoroutine(SpinSword());
        }

        private void OnTriggerEnter(Collider other)
        {
            AliveEntity ent;
            if (!(ent = other.GetComponent<Mob>())) return;
            
            var damageInfo = new DamageInfo
            {
                Damage = Damage,
                CriticalChance = CriticalChance,
                CriticalMultiplier = CriticalMultiplier
            };
            ent.ApplyDamage(damageInfo);
        }
    }
}