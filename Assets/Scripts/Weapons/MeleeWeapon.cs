using System;
using System.Collections;
using NPC;
using UnityEngine;

namespace Weapons
{
    public class MeleeWeapon : Weapon
    {
        private DamageInfo _damageInfo;

        private Collider _triggerCollider;
        
        private void Awake()
        {
            _triggerCollider = GetComponent<BoxCollider>();
        }
        
        public override void Attack(object[] _)
        {
            _damageInfo = new DamageInfo
            {
                Id = DamageInfo.StaticId++,
                Damage = damage,
                CriticalChance = criticalChance,
                CriticalMultiplier = criticalMultiplier,
                Owner = owner
            };
            Enable();
        }

        private void OnTriggerEnter(Collider other)
        {
            AliveEntity getter;
            if (!(getter = other.GetComponent<AliveEntity>())) return;

            var getterIsMob = getter is Mob;
            var senderIsMob = owner is Mob;
            //Disable();

            if (getterIsMob != senderIsMob)
            {
                if (senderIsMob)
                    _damageInfo.Damage *= ((Mob)owner).damageModifier;
                else
                    _damageInfo.Damage *= ((Character)owner).damageModifier;
                
                getter.ApplyDamage(_damageInfo);
            }
        }
    
        public override void Enable()
        {
            _triggerCollider.enabled = true;
        }

        public override void Disable()
        {
            _triggerCollider.enabled = false;
        }
    }
}