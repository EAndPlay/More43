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
    public Character character;
    public Dungeon currentDungeon;
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

    private void Start()
    {
        character.HealthChanged += health =>
        {
            hud.currentHealth.text = ((int) health).ToString();
            hud.healthSlider.value = health / character.MaxHealth;
        };
        character.MaxHealthChanged += maxHealth =>
        {
            hud.maxHealth.text = ((int) maxHealth).ToString();
            hud.healthSlider.value = character.Health / maxHealth;
        };
        hud.dungeonStats.enabled = false;
        level = 1;
        neededExperience = 150;
        hud.currentExperience.text = "0";
        hud.neededExperience.text = "150";
        hud.currentLevel.text = "1";
        
        //Inventory = new(10, null, this);
    }

    public void EnterDungeon(Dungeon dungeon)
    {
        dungeon.MinLevel = level;
        dungeon.MaxLevel = level + Random.Range(0, 3);
        dungeon.Create();

        character.dungeonDeathSpawnPoint = dungeon.exitSpawnPoint;
        currentDungeon = dungeon;
        hud.dungeonStats.enabled = true;
        dungeon.MobDied += RegisterMobDeath;
        hud.currentMobsCount.text = "0";
        hud.maxMobsCount.text = dungeon.MaxMobsCount.ToString();
    }

    public void LeaveDungeon()
    {
        character.Health = character.MaxHealth;
        hud.dungeonStats.enabled = false;
        hud.mobsStatusMark.sprite = GameGlobals.NotDoneMark;
        hud.bossStatusMark.sprite = GameGlobals.NotDoneMark;

        currentDungeon.MobDied -= RegisterMobDeath;
        currentDungeon.Reset();
        currentDungeon = null;
    }

    public void GiveExperience(int exp)
    {
        var sb = new StringBuilder("+EXP: ").Append(exp);
        PlayerLog.Add(sb.ToString(), new Color32(255, 255, 0, 255), character);
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

        character.MaxHealth *= 1.1f + Mathf.Sqrt(character.MaxHealth) / character.MaxHealth;
        character.Health = character.MaxHealth;
        character.damageModifier += 0.2f;
    }
    
    public void OnKill(Mob mob)
    {
        GiveExperience(mob.experienceReward);
        
        foreach (var dropItem in mob.drop)
        {
            if (dropItem.chance < Random.Range(0, 101)) continue;

            var itemCount = Random.Range(dropItem.minCount, dropItem.maxCount + 1);
            
            Inventory.AddItem(dropItem.item, itemCount);

            var sb = new StringBuilder("Gain item: ").Append(dropItem.item.Name);
            if (itemCount > 1)
                sb.Append(" (x").Append(itemCount).Append(")");

            PlayerLog.Add(sb.ToString(), new Color32(0, 0, 255, 255), character);
        }
    }

    public void OnDie()
    {
        Experience = 0;
        if (currentDungeon)
            LeaveDungeon();
    }

    private void RegisterMobDeath(Mob mob)
    {
        hud.currentMobsCount.text = (currentDungeon.MaxMobsCount - currentDungeon.CurrentMobsCount).ToString();
        if (currentDungeon.CurrentMobsCount == 0)
        {
            hud.mobsStatusMark.sprite = GameGlobals.DoneMark;
        }
        if (mob.isBoss)
        {
            hud.bossStatusMark.sprite = GameGlobals.DoneMark;
        }
    }
}