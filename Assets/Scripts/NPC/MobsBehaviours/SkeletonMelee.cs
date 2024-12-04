using System;
using System.Linq;
using UnityEngine;
using Weapons;

namespace NPC.MobsBehaviours
{
    public sealed class SkeletonMelee : Mob
    {
        private static readonly int SpeedAnimId = Animator.StringToHash("Speed");

        private void Start()
        {
            var clip = animator.runtimeAnimatorController.animationClips.First(x =>
                x.name == "Standing Melee Attack Backhand");

            var animEvent = new AnimationEvent
            {
                time = clip.length,
                functionName = "MeleeAttackAnimationEnded",
                stringParameter = clip.name
            };

            clip.AddEvent(animEvent);
        }

        protected override Action MeleeAttack => () =>
        {
            animator.SetTrigger(MeleeAttackAnimId);
        };

        protected override void StartMove()
        {
            base.StartMove();
            
            animator.SetFloat(SpeedAnimId, 1);
        }

        protected override void StopMove()
        {
            base.StopMove();
            
            animator.SetFloat(SpeedAnimId, 0);
        }
    }
}