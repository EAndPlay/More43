using System;
using System.Linq;
using UnityEngine;
using Weapons;

namespace NPC.MobsBehaviours
{
    public sealed class SkeletonRange : Mob
    {
        private static readonly int SpeedAnimId = Animator.StringToHash("Speed");

        private void Start()
        {
            var clip = animator.runtimeAnimatorController.animationClips.First(x =>
                x.name == "Standing Draw Arrow");

            var animEvent = new AnimationEvent
            {
                time = clip.length,
                functionName = "RangeAttackAnimationEnded",
                stringParameter = clip.name
            };

            clip.AddEvent(animEvent);
        }

        protected override Action RangeAttack => () =>
        {
            animator.SetTrigger(RangeAttackAnimId);
        };

        // protected override void StartMove()
        // {
        //     base.StartMove();
        //     
        //     _animator.SetFloat(SpeedAnimId, 1);
        // }
        //
        // protected override void StopMove()
        // {
        //     base.StopMove();
        //     
        //     _animator.SetFloat(SpeedAnimId, 0);
        // }
    }
}