﻿using System;
using System.Collections;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowProjectile : MonoBehaviour
    {
        public RangeWeapon owner;
        private void OnTriggerEnter(Collider other)
        {
            AliveEntity ent;
            if (!(ent = other.GetComponent<Character>())) return;
            
            var damageInfo = new DamageInfo
            {
                Id = DamageInfo.StaticId++,
                Damage = owner.damage,
                CriticalChance = owner.criticalChance,
                CriticalMultiplier = owner.criticalMultiplier,
                Owner = owner.owner
            };
            ent.ApplyDamage(damageInfo);
            Destroy(gameObject);
        }
        
        private void Awake()
        {
            Destroy(gameObject, 1);
        }
    }
}