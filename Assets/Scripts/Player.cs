using System;
using UnityEngine;

[RequireComponent(typeof(HUD))]
public class Player : MonoBehaviour
{
    public Character Character;
    private HUD _hud;
    
    public Location CurrentLocation;
    
    private void Awake()
    {
        _hud = GetComponent<HUD>();
        Character = gameObject.GetComponentInChildren<Character>();
        
        //TODO: remove subs on destroy? make them local (not anon) => add in <Died> event, back on <Spawned>
        Character.MaxHealthChanged += maxHealth =>
        {
            _hud.maxHealth.text = ((int) maxHealth).ToString();
            _hud.healthSlider.value = Character.Health / maxHealth;
        };
        Character.HealthChanged += health =>
        {
            _hud.currentHealth.text = ((int) health).ToString();
            _hud.healthSlider.value = health / Character.MaxHealth;
        };
    }

    private void EnterLocation(Location location)
    {
        
    }
}