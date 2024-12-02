using System;
using System.Collections;
using System.Linq;
using System.Text;
using Dungeons;
using NPC;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public sealed class Player : MonoBehaviour
{
    [SerializeField] private HUD hud;

    private int _experience; 
    
    public int neededExperience;
    public Character Character;
    public Dungeon CurrentDungeon;
    public Inventory.Inventory Inventory;
    
    public int level;

    public int Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            hud.experienceSlider.value = (float)_experience / neededExperience;
            hud.currentExperience.text = _experience.ToString();
        }
    }
    
    public Location CurrentLocation;
    
    private void Awake()
    {
        //_hud = GetComponent<HUD>();
    }

    private void Start()
    {
        //Character = GetComponent<Character>();
        //TODO: remove subs on destroy? make them local (not anon) => add in <Died> event, back on <Spawned>
        Character.HealthChanged += health =>
        {
            hud.currentHealth.text = ((int) health).ToString();
            hud.healthSlider.value = health / Character.MaxHealth;
        };
        Character.MaxHealthChanged += maxHealth =>
        {
            hud.maxHealth.text = ((int) maxHealth).ToString();
            hud.healthSlider.value = Character.Health / maxHealth;
        };
        hud.dungeonStats.enabled = false;
        level = 1;
        neededExperience = 150;
        hud.currentExperience.text = "0";
        hud.neededExperience.text = "150";
        hud.currentLevel.text = "1";
        
        Inventory = new(10, null);
        //Inventory = new(stats.ItemsMaxSlots, null);
        //Character.Spawn(SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.GetComponent<SpawnPoint>() != null).transform.position, new SpawnStats());
    }

    public void EnterDungeon(Dungeon dungeon)
    {
        dungeon.MinLevel = level;
        dungeon.MaxLevel = level + Random.Range(0, 3);
        dungeon.Create();

        Character.dungeonDeathSpawnPoint = dungeon.exitSpawnPoint;
        CurrentDungeon = dungeon;
        hud.dungeonStats.enabled = true;
        dungeon.MobDied += RegisterMobDeath;
        hud.currentMobsCount.text = "0";
        hud.maxMobsCount.text = dungeon.MaxMobsCount.ToString();
    }

    public void LeaveDungeon()
    {
        hud.dungeonStats.enabled = false;
        hud.mobsStatusMark.sprite = GameGlobals.NotDoneMark;
        hud.bossStatusMark.sprite = GameGlobals.NotDoneMark;

        CurrentDungeon.MobDied -= RegisterMobDeath;
        CurrentDungeon.Reset();
        CurrentDungeon = null;
    }

    public void GiveExperience(int exp)
    {
        var sb = new StringBuilder("+EXP: ").Append(exp);
        PlayerLog.Add(sb.ToString(), new Color32(255, 255, 0, 255));
        if (Experience + exp >= neededExperience)
        {
            var oldExp = neededExperience;
            neededExperience *= (int)(1.5f * Math.Log10(neededExperience));
            Experience += exp - oldExp;
            hud.neededExperience.text = neededExperience.ToString();
            OnLevelUp(false);
        }
        else
        {
            Experience += exp;
        }
    }

    public void OnLevelUp(bool changeExp)
    {
        if (changeExp)
        {
            neededExperience *= (int)(1.5f * Math.Log10(neededExperience));
            hud.neededExperience.text = neededExperience.ToString();
        }
        level++;
        hud.currentLevel.text = level.ToString();

        Character.MaxHealth *= 1.1f + Mathf.Sqrt(Character.MaxHealth) / Character.MaxHealth;
        Character.Health = Character.MaxHealth;
        Character.damageModifier += 0.2f;
    }
    
    public void OnKill(Mob mob)
    {
        GiveExperience(mob.experienceReward);
        
        foreach (var dropItem in mob.drop)
        {
            if (dropItem.Chance < Random.Range(0, 101)) continue;

            var itemCount = Random.Range(dropItem.MinCount, dropItem.MaxCount + 1);
            
            Inventory.AddItem(dropItem.Item, itemCount);

            var sb = new StringBuilder("Gain item: ").Append(dropItem.Item.Name);
            if (itemCount > 1)
                sb.Append(" (x").Append(itemCount).Append(")");
            
            PlayerLog.Add(sb.ToString(), new Color32(0, 0, 255, 255));
        }
    }

    public void OnDie()
    {
        Experience = 0;
        LeaveDungeon();
    }

    private void RegisterMobDeath(Mob mob)
    {
        hud.currentMobsCount.text = (CurrentDungeon.MaxMobsCount - CurrentDungeon.CurrentMobsCount).ToString();
        if (CurrentDungeon.CurrentMobsCount == 0)
        {
            hud.mobsStatusMark.sprite = GameGlobals.DoneMark;
        }
        if (mob.isBoss)
        {
            hud.bossStatusMark.sprite = GameGlobals.DoneMark;
        }
    }
    
    private void EnterLocation(Location location)
    {
        
    }
}