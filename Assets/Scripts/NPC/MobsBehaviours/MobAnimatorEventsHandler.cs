using UnityEngine;

namespace NPC.MobsBehaviours
{
    public class MobAnimatorEventsHandler : MonoBehaviour
    {
        protected static int MeleeAttackAnimId = Animator.StringToHash("MeleeAttack");
        
        [SerializeField] private Mob _mob;
        [SerializeField] private Animator _animator;
        
        private void MeleeAttackAnimationEnded(string _)
        {
            _mob.isAttacking = false;
            _mob.weapon.Disable();
            //_animator.ResetTrigger(MeleeAttackAnimId);
        }

        private void RangeAttackAnimationEnded(string _)
        {
            if (!_mob.isAttacking) return;
            _mob.weapon.Attack(new object[] { _mob.target.partForRangeAttack.transform });
            _mob.isAttacking = false;
        }
    }
}