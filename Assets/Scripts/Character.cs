using System;
using System.Collections;
using System.Collections.Generic;
using Buffs;
using UnityEngine;
using Weapons;

public class Character : AliveEntity
{
    //public object Class;

    [SerializeField] private float speed;
    
    private Camera _followCamera;
    private List<Buff> _buffs;
    private bool _attackHolded;

    public Weapon CurrentWeapon;
    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;

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

    private void Awake()
    {
        IsDead = false;
        CurrentWeapon = PrimaryWeapon = GetComponentInChildren<SwordWeapon>();
        
        _followCamera = GetComponentInChildren<Camera>();
        _buffs = new();
        
        // test
        ApplyBuff(new RegenerationBuff(.25f, 5));
    }
    
    private void Start()
    {
        Health = MaxHealth = 100;
        // test
        //StartCoroutine(SmoothHealthRegeneration());
        //Spawn(Vector3.zero);
        
        StartCoroutine(TickBuffs());
        //Task.Run(TickBuffs);
    }

    public void Spawn(Vector3 spawnPos)
    {
        IsDead = false;

        //_transform.position = _followCamera.transform.position;
    }

    public override void Die()
    {
        base.Die();
        _buffs.Clear();
    }

    public void ApplyBuff(Buff buff)
    {
        buff.Owner = this;
        buff.RemainingTime = buff.MaxTime;
        _buffs.Add(buff);
    }
    
    // mb use coroutine?
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
                if (_buffs[i].RemainingTime <= 0)
                {
                    _buffs.RemoveAt(i - removeOffset);
                    removeOffset++;
                }
            }
        }
    }
    
    // health bar animation
    private IEnumerator SmoothHealthRegeneration()
    {
        float i = 1;
        while (true)
        {
            if (Health >= MaxHealth)
                i = -0.1f;
            else if (Health <= 0)
                i = 0.1f;
            Health += i;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator DoAttack()
    {
        while (Input.GetMouseButton(0))
        {
            CurrentWeapon.Attack();
            yield return new WaitForSeconds(CurrentWeapon.AttackRate);
        }
    }

    private float _attackDelay;

    private void Update()
    {
        _attackDelay -= Time.deltaTime;
        if (_attackDelay <= 0 && Input.GetMouseButton(0))
        {
            CurrentWeapon.Attack();
            _attackDelay = CurrentWeapon.AttackRate;
        }
        // if (!_attackHolded && Input.GetMouseButtonDown(0))
        // {
        //     _attackHolded = true;
        //     StartCoroutine(DoAttack());
        // }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     _attackHolded = false;
        // }
    }
}