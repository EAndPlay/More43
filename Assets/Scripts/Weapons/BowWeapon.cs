using System;
using UnityEngine;

namespace Weapons
{
    public class BowWeapon : Weapon
    {
        public override void Attack()
        {
            var ray = InGameCamera.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                var obj = (GameObject)Instantiate(Resources.Load("Weapons/Arrow"), Transform.position, Quaternion.identity);
                var objTransform = obj.transform;
                objTransform.LookAt(hit.point);
                obj.GetComponent<ArrowProjectile>().owner = this;
                obj.GetComponent<Rigidbody>().velocity = (hit.point - objTransform.position).normalized * 75;
            }
        }
    }
}