using System;
using System.Linq;
using UnityEngine;

namespace NPC.MobsBehaviours
{
    public class MageBoss : Mob
    {
        private static readonly int IsMovingAnimId = Animator.StringToHash("IsMoving");

        private void Start()
        {
            var clip = _animator.runtimeAnimatorController.animationClips.First(x => x.name == "Attack01");
            
            var animEvent = new AnimationEvent
            {
                time = clip.length / 2,
                functionName = nameof(RangeAttackAnimationEnded),
                stringParameter = clip.name
            };
            
            clip.AddEvent(animEvent);
        }

        private void RangeAttackAnimationEnded(string _)
        {
            isAttacking = false;
            weapon.Attack(new object[] { TargetTransform });
        }
        
        protected override Action RangeAttack => () =>
        {
            _animator.SetTrigger(RangeAttackAnimId);
        };

        protected override void StartMove()
        {
            base.StartMove();
            
            _animator.SetBool(IsMovingAnimId, true);
        }

        protected override void StopMove()
        {
            base.StopMove();
            
            _animator.SetBool(IsMovingAnimId, false);
        }

        public override bool ApplyDamage(DamageInfo damageInfo)
        {
            if (!base.ApplyDamage(damageInfo)) return false;
            
            if (!isAttacking)
                _animator.SetTrigger(GetDamageAnimId);
            
            return true;
        }
    }
}