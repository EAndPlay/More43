using System;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class RangeWeapon : Weapon
    {
        [SerializeField] private Transform projectileSpawnPoint;
        private GameObject _projectile;
        
        private void Awake()
        {
            _projectile = Resources.Load<GameObject>("Weapons/FireBall");
        }
        
        public override void Attack(object[] args)
        {
            var targetTransform = (Transform)args[0];
            var ray = InGameCamera.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                var obj = Instantiate(_projectile, projectileSpawnPoint.position, Quaternion.identity);
                var objTransform = obj.transform;
                //objTransform.LookAt(hit.point);
                obj.GetComponent<ArrowProjectile>().owner = this;
                obj.GetComponent<Rigidbody>().velocity = ((targetTransform.position + (targetTransform.GetComponentInParent<Rigidbody>().velocity / (float)Math.Sqrt(2)) - Vector3.up) - objTransform.position).normalized * 15;
            }
        }
    }
}