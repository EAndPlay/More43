using System;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class RangeWeapon : Weapon
    {
        public GameObject projectile;
        public bool projectileLookAtTarget;
        [SerializeField] private Transform projectileSpawnPoint;
        
        // private void Awake()
        // {
        //     projectile = Resources.Load<GameObject>("Weapons/FireBall");
        // }

        public override void Attack(object[] args)
        {
            var targetTransform = (Transform)args[0];

            var startPos = projectileSpawnPoint.position;
            var targetPos = targetTransform.position;
            var direction = targetPos - startPos;
            
            var obj = Instantiate(projectile, startPos, Quaternion.identity, GameGlobals.IndependentObjects).GetComponent<Projectile>();
            var objTransform = obj.transform;
            
            if (projectileLookAtTarget)
            {
                objTransform.rotation = Quaternion.LookRotation(direction.normalized) * new Quaternion(0, 90, 90, 0);
                //objTransform.Rotate(0, 90, 90);
            }

            obj.owner = this;
            obj.GetComponent<Rigidbody>().velocity = direction.normalized * 15; //((targetTransform.position + (targetTransform.GetComponentInParent<Rigidbody>().velocity / (float)Math.Sqrt(2)) - Vector3.up) - objTransform.position).normalized * 15;
        }
    }
}