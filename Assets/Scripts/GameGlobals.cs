﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class GameGlobals : MonoBehaviour
{
    public static Transform IndependentObjects;
    public static GameObject MobHealthBar;
    public static Image NoneItemImage;
    public static ParticleSystem BurningParticles;
    public static Sprite DoneMark;
    public static Sprite NotDoneMark;
    public static HealableObject HealHeart;

    private void Awake()
    {
        IndependentObjects = transform;
        NoneItemImage = Resources.Load<Image>("Items/NoneItemImage");
        MobHealthBar = Resources.Load<GameObject>(nameof(MobHealthBar));
        BurningParticles = Resources.Load<ParticleSystem>("Particle_Fire");
        DoneMark = Resources.Load<Sprite>(nameof(DoneMark));
        NotDoneMark = Resources.Load<Sprite>(nameof(NotDoneMark));
        HealHeart = Resources.Load<HealableObject>(nameof(HealHeart));
    }
}