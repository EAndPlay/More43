using System;
using System.Collections;
using System.Linq;
using System.Text;
using AnyRPG;
using Dungeons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

using Item = Inventory.Item;
using Random = UnityEngine.Random;

namespace NPC
{
    public enum AttackType
    {
        None,
        Melee,
        Range
    }
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Mob : AliveEntity
    {
        public GameObject spawnPoint;
        public Dungeon dungeon;
        public float damageModifier;
        public int level;

        [Header("Manual Set Parameters")]
        public bool isBoss;
        public bool isFollowable;
        public Weapon weapon;
        [SerializeField] protected float baseHealth;
        [SerializeField] protected float baseDamage;
        public int experienceReward;
        public float searchRadius;
        public float closeRadius;
        public float moveSpeed;
        public bool stopOnRangeAttack;
        [SerializeField] protected float attackRate;
        [SerializeField] protected Animator animator;
        [SerializeField] private Transform bodyTransform;
        
        [Serializable]
        public class DropItem
        {
            public Item item;
            public int minCount;
            public int maxCount;
            [Header("0 .. 100%")]
            public int chance;
        }

        public DropItem[] drop;
        
        private Slider _healthSlider;
        private Image _healthSliderFill;
        
        public Character target;
        protected Transform TargetTransform;
        private bool _isMoving;
        [HideInInspector] public bool isAttacking;
        
        private AttackType _attackType;
        private Rigidbody _rigidbody;

        protected static int MeleeAttackAnimId = Animator.StringToHash("MeleeAttack");
        protected static int RangeAttackAnimId = Animator.StringToHash("RangeAttack");
        protected static int GetDamageAnimId = Animator.StringToHash("GetDamage");
        
        protected virtual Action MeleeAttack => null;
        protected virtual Action RangeAttack => null;

        private void OnDestroy()
        {
            HealthChanged -= RegisterHealthChange;
        }

        private void RegisterHealthChange(float newHealth)
        {
            var healthProportion = newHealth / maxHealth;
            _healthSlider.value = healthProportion;
            _healthSliderFill.color = new Color(1 - healthProportion, healthProportion, 0);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Character character;
            if (!(character = other.GetComponent<Character>()) || target != null) return;

            target = character;
            TargetTransform = target.partForRangeAttack.transform;
            StartCoroutine(FollowTarget());
        }
        
        private IEnumerator FollowTarget()
        {
            var deltaTime = Time.fixedDeltaTime;
            const float delayMaxTime = 0.5f;
        
            float scanDelta = 0;
            float attackDelay = 0;
        
            while (!isDead && target)
            {
                if (target.IsDead)
                    yield break;

                while (isAttacking)
                    yield return new WaitForEndOfFrame();
                
                scanDelta += deltaTime;
                attackDelay -= deltaTime;
                var targetPosition = TargetTransform.position;
                var moveVector = targetPosition - Transform.position;
                if (scanDelta >= delayMaxTime)
                {
                    var distance = moveVector.magnitude;
                    if (distance >= searchRadius)
                    {
                        StopMove();
                        Transform.position = spawnPoint.transform.position;
                        Health = maxHealth;
                        target = null;
                        yield break;
                    }

                    if (distance > closeRadius)
                        _attackType = AttackType.Range;
                    else if (distance <= closeRadius)
                        _attackType = AttackType.Melee;

                    scanDelta = 0;
                }
                var lookPos = moveVector;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                bodyTransform.rotation = Quaternion.Slerp(Transform.rotation, rotation, deltaTime * 5);;

                moveVector.y = 0;
                moveVector = moveVector.normalized;

                if (isBoss)
                {
                    if (attackDelay <= 0 && !isAttacking) 
                    {
                        RangeAttack.Invoke();
                        isAttacking = true;
                        attackDelay = attackRate;
                    }
                }
                else
                {
                    if (_attackType == AttackType.Melee)
                    {
                        var hasMelee = MeleeAttack != null;
                        if (hasMelee)
                            _rigidbody.velocity = Vector3.zero;
                        if (attackDelay <= 0 && !isAttacking)
                        {
                            if (hasMelee)
                            {
                                MeleeAttack.Invoke();
                                weapon.Attack();
                            }
                            else if (RangeAttack != null)
                            {
                                RangeAttack.Invoke();
                            }

                            isAttacking = true;
                            attackDelay = attackRate;
                        }

                        if (_isMoving)
                            StopMove();
                    }
                    else if (_attackType == AttackType.Range)
                    {
                        if (RangeAttack != null && attackDelay <= 0 && !isAttacking)
                        {
                            RangeAttack.Invoke();
                            isAttacking = true;
                            attackDelay = attackRate;
                        }

                        if (isFollowable)
                        {
                            _rigidbody.velocity = moveVector * moveSpeed;

                            if (!_isMoving)
                                StartMove();
                        }
                    }
                }

                yield return new WaitForNextFrameUnit();
            }

            target = null;
        }

        public override void Die(AliveEntity killer = null)
        {
            //base.Die();
            dungeon.RegisterMobDeath(this);

            // 50%
            if (Random.Range(0, 100f) < 50)
            {
                var heart = Instantiate(GameGlobals.HealHeart, dungeon.Transform);
                heart.Transform.position = bodyTransform.position + Vector3.up;
            }

            Destroy(gameObject);
        }

        public override bool ApplyDamage(DamageInfo damageInfo)
        {
            if (!base.ApplyDamage(damageInfo)) return false;

            var hitAlert = Instantiate(Resources.Load<DamageNotification>("DamageNotification"), GameGlobals.IndependentObjects);
            hitAlert.Notify((int)damageInfo.Damage, Transform.position + (isBoss ? Vector3.up * 2.5f : Vector3.zero));
            if (health <= 0)
            {
                if (damageInfo.Owner is Character character)
                {
                    character.owner.OnKill(this);
                }
                Die(damageInfo.Owner);
            }

            return true;
        }

        protected virtual void StartMove()
        {
            _isMoving = true;
        }

        protected virtual void StopMove() 
        {
            _isMoving = false;
        }
        
        protected virtual void OnGetDamage() { }

        public virtual void Initialize()
        {
            Transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            if (!(animator = GetComponent<Animator>()))
                animator = GetComponentInChildren<Animator>();
            
            if (!bodyTransform)
                bodyTransform = Transform;
            
            var modifier = Math.Max((level - 1) * 1.2f, 1);
            damageModifier = Math.Max((level - 1) * 1.05f, 1);//modifier;
            MaxHealth = baseHealth * modifier;
            Health = MaxHealth;
            IsDead = false;
            isDamagable = true;
            
            var healthBarCanvas = Instantiate(GameGlobals.MobHealthBar, Transform);
            _healthSlider = healthBarCanvas.GetComponentInChildren<Slider>();
            _healthSliderFill = _healthSlider.transform.FindChildByRecursive("Fill").GetComponent<Image>();
            RegisterHealthChange(health);
            HealthChanged += RegisterHealthChange;
            
            Transform.SetParent(dungeon.Transform);
        }
    }
}