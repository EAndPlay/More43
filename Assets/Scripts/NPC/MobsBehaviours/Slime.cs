using System;
using UnityEngine;
using Weapons;

namespace NPC.MobsBehaviours
{
    public sealed class Slime : Mob
    {
        protected override Action RangeAttack => () =>
        {
            var position = TargetTransform.position;
            //var ray = new Ray(Transform.position, position);
            var obj = (GameObject)Instantiate(Resources.Load("Weapons/Spit"), Transform.position, Quaternion.identity);
            var objTransform = obj.transform;
            objTransform.LookAt(position);
            obj.GetComponent<SpitProjectile>().owner = this;
            obj.GetComponent<Rigidbody>().velocity =
                ((position + (target.GetComponent<Rigidbody>().velocity / (float)Math.Sqrt(2)) - Vector3.up) -
                 objTransform.position).normalized * 15;
        };
    }
}