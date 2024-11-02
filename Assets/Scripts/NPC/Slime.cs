using System;
using UnityEngine;
using Weapons;

namespace NPC
{
    public sealed class Slime : Mob
    {
        public override void RangeAttack()
        {
            var position = TargetTransform.position;
            //var ray = new Ray(Transform.position, position);
            var obj = (GameObject)Instantiate(Resources.Load("Weapons/Spit"), Transform.position, Quaternion.identity);
            var objTransform = obj.transform;
            objTransform.LookAt(position);
            obj.GetComponent<SpitProjectile>().owner = this;
            obj.GetComponent<Rigidbody>().velocity = ((position + (Target.GetComponent<Rigidbody>().velocity / (float)Math.Sqrt(2)) - Vector3.up) - objTransform.position).normalized * 15;
        }
    }
}