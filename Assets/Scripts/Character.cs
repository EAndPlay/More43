using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buffs;
using UnityEngine;

public class Character : MonoBehaviour
{
    //public object Class;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    
    private bool _isDead;
    private Camera _followCamera;

    private Transform _transform;
    
    public List<Buff> Buffs;
    
    public event Action<float> HealthChanged;
    public event Action<float> MaxHealthChanged;
    public event Action<float> SpeedChanged;

    private void Awake()
    {
        _transform = transform;
        
        _isDead = true;
        _followCamera = GetComponentInChildren<Camera>();
        Buffs = new();
        
        // test
        ApplyBuff(new RegenerationBuff(.25f, 5));
    }

    public void Spawn(Vector3 spawnPos)
    {
        _isDead = false;

        //_transform.position = _followCamera.transform.position;
    }

    public void Die()
    {
        _isDead = true;
        Buffs.Clear();
    }
    
    private void Start()
    {
        MaxHealth = 100;
        Health = MaxHealth / 4;
        
        // test
        //StartCoroutine(SmoothHealthRegeneration());
        Spawn(Vector3.zero);
        
        StartCoroutine(TickBuffs());
        //Task.Run(TickBuffs);
    }

    public void ApplyBuff(Buff buff)
    {
        buff.Owner = this;
        buff.RemainingTime = buff.MaxTime;
        Buffs.Add(buff);
    }
    
    // mb use coroutine?
    private IEnumerator TickBuffs()
    {
        const float tickDelay = 0.1f;
        
        while (!_isDead)
        {
            yield return new WaitForSeconds(tickDelay);
            
            foreach (var buff in Buffs)
            {
                buff.RemainingTime -= tickDelay;
                buff.Tick();
            }

            int removeOffset = 0;
            for (var i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].RemainingTime <= 0)
                {
                    Buffs.RemoveAt(i - removeOffset);
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

    public float Health
    {
        get => health;
        set
        {
            health = value;
            HealthChanged?.Invoke(value);
        }
    }
    
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            MaxHealthChanged?.Invoke(value);
        }
    }
    
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            SpeedChanged?.Invoke(value);
        }
    }
}