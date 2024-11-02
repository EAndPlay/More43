using System;
using System.Collections;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class SpitProjectile : MonoBehaviour
    {
        public Mob owner;
        private void OnTriggerEnter(Collider other)
        {
            AliveEntity ent;
            if (!(ent = other.GetComponent<Character>())) return;

            var damageInfo = new DamageInfo
            {
                Damage = owner.damage
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