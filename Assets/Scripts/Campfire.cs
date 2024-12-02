using System;
using System.IO;
using Buffs;
using UnityEngine;

public class Campfire : MonoObject
{
    private void OnTriggerEnter(Collider other)
    {
        Character character;
        if (!(character = other.GetComponent<Character>())) return;
        
        character.ApplyBuff(new BurningBuff());
    }
}