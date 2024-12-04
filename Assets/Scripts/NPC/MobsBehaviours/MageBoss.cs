using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace NPC.MobsBehaviours
{
    public class MageBoss : Mob
    {
        private static readonly int IsMovingAnimId = Animator.StringToHash("IsMoving");

        [SerializeField] private GameObject shield;
        
        private void Start()
        {
            var clip = animator.runtimeAnimatorController.animationClips.First(x => x.name == "Attack01");
            
            var animEvent = new AnimationEvent
            {
                time = clip.length / 2,
                functionName = nameof(RangeAttackAnimationEnded),
                stringParameter = clip.name
            };
            
            clip.AddEvent(animEvent);
            
            shield.SetActive(false);

            HealthChanged += OnHealthChange;
        }

        private void OnHealthChange(float newHealth)
        {
            if (newHealth / maxHealth >= 0.5f) return;

            Transform.localScale *= 1.2f;
            StartCoroutine(StateChanging());
            HealthChanged -= OnHealthChange;
        }

        private IEnumerator StateChanging()
        {
            while (!isDead)
            {
                shield.SetActive(true);
                isDamagable = false;
                yield return new WaitForSeconds(10);
                shield.SetActive(false);
                isDamagable = true;
                yield return new WaitForSeconds(10);
            }
        }
        
        private void RangeAttackAnimationEnded(string _)
        {
            isAttacking = false;
            weapon.Attack(new object[] { TargetTransform });
        }
        
        protected override Action RangeAttack => () =>
        {
            animator.SetTrigger(RangeAttackAnimId);
        };

        protected override void StartMove()
        {
            base.StartMove();
            
            animator.SetBool(IsMovingAnimId, true);
        }

        protected override void StopMove()
        {
            base.StopMove();
            
            animator.SetBool(IsMovingAnimId, false);
        }

        public override bool ApplyDamage(DamageInfo damageInfo)
        {
            if (!base.ApplyDamage(damageInfo)) return false;
            
            if (!isAttacking)
                animator.SetTrigger(GetDamageAnimId);
            
            return true;
        }
    }
}