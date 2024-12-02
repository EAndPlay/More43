using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Player variables")]
    public Slider healthSlider;
    public Slider experienceSlider;
    public TMP_Text currentHealth;
    public TMP_Text currentExperience;
    public TMP_Text maxHealth;
    public TMP_Text neededExperience;
    public TMP_Text currentLevel;
    
    [Header("Dungeon")]
    public Canvas dungeonStats;
    public Image mobsStatusMark;
    public Image bossStatusMark;
    public TMP_Text currentMobsCount;
    public TMP_Text maxMobsCount;
}