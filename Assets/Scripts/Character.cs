using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buffs;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

public class Character : AliveEntity
{
    //public object Class;

    [SerializeField] private float speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject CharacterBody;
    
    private Camera _followCamera;
    private List<Buff> _buffs;
    private List<ParticleSystem> _particles;
    private bool _attackHold;
    private float _attackDelay;

    public GameObject deathSpawnPoint;
    public GameObject dungeonDeathSpawnPoint;
    public Weapon currentWeapon;
    public GameObject partForRangeAttack;
    public Player owner;
    public new Camera camera;
    public float damageModifier = 1;
    public float attackRate;
    
    private static readonly int AttackId = Animator.StringToHash("Attack");

    public event Action<float> SpeedChanged;
    
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            SpeedChanged?.Invoke(value);
        }
    }

    public void AttackAnimationEnded(string _)
    {
        currentWeapon.Disable();
    }
    
    private void Awake()
    {
        IsDead = false;
        //currentWeapon = GetComponentInChildren<MeleeWeapon>();
        currentWeapon.owner = this;
        
        _followCamera = GetComponentInChildren<Camera>();
        LookAtCamera.CameraTransform = camera.transform;
        _buffs = new();
        _particles = new();

        var clip = _animator.runtimeAnimatorController.animationClips.First(x => x.name == "MeleeAttack_OneHanded");
        
        var animEvent = new AnimationEvent
        {
            time = clip.length,
            functionName = nameof(AttackAnimationEnded),
            stringParameter = clip.name
        };

        clip.AddEvent(animEvent);
        
        // test
        //ApplyBuff(new RegenerationBuff(.25f, 5));
    }

    private IEnumerator LateInitialization()
    {
        yield return new WaitForEndOfFrame();
        MaxHealth = 100;
        Health = MaxHealth;
        StartCoroutine(TickBuffs());
        HealthChanged += newHealth =>
        {
            if (newHealth <= 0)
                Die();
        };
        isDamagable = true;
    }
    
    private void Start()
    {
        StartCoroutine(LateInitialization());
    }

    public void Spawn(Vector3 spawnPos, SpawnStats stats)
    {
        IsDead = false;
        MaxHealth = stats.MaxHealth;
        Health = stats.Health;

        //_transform.position = _followCamera.transform.position;
    }

    public override void Die(AliveEntity killer = null)
    {
        IsDead = true;
        if (owner.currentDungeon)
        {
            Transform.position = dungeonDeathSpawnPoint.transform.position;
        }
        else
        {
            Transform.position = deathSpawnPoint.transform.position;
        }
        base.Die(killer);
        
        foreach (var buff in _buffs)
            buff.OnDeactivate();
        _buffs.Clear();
        
        Health = MaxHealth;
        owner.OnDie();

        IsDead = false;
    }

    public void ApplyBuff(Buff buff)
    {
        var containedBuff = _buffs.FirstOrDefault(x => x.Type == buff.Type);
        if (containedBuff != null)
        {
            containedBuff.RemainingTime = buff.MaxTime;
            containedBuff.Stacks++;
        }
        else
        {
            buff.Owner = this;
            //buff.RemainingTime = buff.MaxTime;
            buff.Stacks = 1;
            _buffs.Add(buff);
            buff.OnActivate();
        }
    }

    public ParticleSystem AddParticles(ParticleSystem particleSystem)
    {
        var copyParticle = Instantiate(particleSystem, CharacterBody.transform);
        _particles.Add(copyParticle);
        return copyParticle;
    }
    
    public void RemoveParticles(ParticleSystem particleSystem)
    {
        var particle = _particles.FirstOrDefault(x => x == particleSystem);
        if (particle == null) return;
        _particles.Remove(particle);
        Destroy(particle);
    }
    
    private IEnumerator TickBuffs()
    {
        const float tickDelay = 0.1f;
        
        while (!IsDead)
        {
            yield return new WaitForSeconds(tickDelay);
            
            foreach (var buff in _buffs.ToArray())
            {
                buff.RemainingTime -= tickDelay;
                buff.Tick();
            }

            int removeOffset = 0;
            for (var i = 0; i < _buffs.Count; i++)
            {
                var buffToRemove = _buffs[i];
                if (buffToRemove.RemainingTime <= 0.1f)
                {
                    buffToRemove.OnDeactivate();
                    _buffs.RemoveAt(i - removeOffset++);
                }
            }
        }
    }

    public override bool ApplyDamage(DamageInfo damageInfo)
    {
        if (Health > 0 && !base.ApplyDamage(damageInfo)) return false;
        
        return true;
    }
    
    private void Update()
    {
        _attackDelay -= Time.deltaTime;
        var lmbPressed = Input.GetMouseButton(0);
        if (_attackDelay <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack_OneHanded"))
        {
            if (lmbPressed)
            {
                currentWeapon.Attack();
                _animator.SetTrigger(AttackId);
                _attackDelay = attackRate;
                //_attackDelay = currentWeapon.attackRate;
            }
        }

        if (!lmbPressed)
        {
            _animator.ResetTrigger(AttackId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HealableObject heal;
        if (!(heal = other.GetComponent<HealableObject>())) return;
        
        Health += heal.health * (1 + owner.level * 0.25f);
        
        Destroy(other.gameObject);
    }
}