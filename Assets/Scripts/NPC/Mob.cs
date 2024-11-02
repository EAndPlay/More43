using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    public enum AttackType
    {
        Melee,
        Range
    }
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Mob : AliveEntity
    {
        [SerializeField] protected float searchRadius;
        [SerializeField] protected float closeRadius;
        [SerializeField] public GameObject spawnPoint;
        [SerializeField] protected float moveSpeed;
        [SerializeField] public float damage;
        [SerializeField] protected float attackRate;
        protected Character Target;
        protected Transform TargetTransform;

        protected AttackType AttackType;

        private Rigidbody _rigidbody;
        
        //private Coroutine _followCoroutine;
        
        private void Awake()
        {
            Transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            MaxHealth = maxHealth;
            Health = MaxHealth;
            IsDead = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Character character;
            if (!(character = other.GetComponent<Character>()) || Target != null) return;

            Target = character;
            TargetTransform = Target.transform;
            /*_followCoroutine = */StartCoroutine(FollowTarget());
        }

        private IEnumerator FollowTarget()
        {
            const float delayMaxTime = 0.5f;
        
            float scanDelta = 0;
            float attackDelay = 0;
        
            while (!isDead)
            {
                if (Target.IsDead)
                    yield break;
            
                scanDelta += Time.fixedDeltaTime;
                attackDelay -= Time.fixedDeltaTime;
                var targetPosition = TargetTransform.position;
                var moveVector = targetPosition - Transform.position;
                if (scanDelta >= delayMaxTime)
                {
                    var dest = TargetTransform.position - Transform.position;
                    var distance = moveVector.magnitude;
                    if (distance >= searchRadius)
                    {
                        Transform.position = spawnPoint.transform.position;
                        Health = maxHealth;
                        Target = null;
                        yield break;
                    }

                    AttackType = distance <= closeRadius ? AttackType.Melee : AttackType.Range;

                    scanDelta = 0;
                }

                Transform.LookAt(moveVector);
                //var rotY = new Quaternion(0, Transform.rotation.y, 0, 0);
                //Transform.rotation = rotY;

                moveVector.y = 0;
                moveVector = moveVector.normalized;
                if (AttackType == AttackType.Range)
                {
                    if (attackDelay <= 0)
                    {
                        RangeAttack();
                        attackDelay = attackRate;
                    }

                    _rigidbody.velocity = moveVector * moveSpeed;
                }
                else
                {
                    _rigidbody.velocity = Vector3.zero;
                }
                
                yield return new WaitForFixedUpdate();
            }

            Target = null;
        }

        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
        }

        public virtual void RangeAttack() {}
        public virtual void MeleeAttack() {}
    }
}