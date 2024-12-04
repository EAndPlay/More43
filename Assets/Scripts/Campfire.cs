using System;
using System.Collections;
using System.IO;
using Buffs;
using UnityEngine;

public class Campfire : MonoObject
{
    private bool _isInCampfire;
    
    private void OnTriggerEnter(Collider other)
    {
        Character character;
        if (!(character = other.GetComponent<Character>())) return;

        _isInCampfire = true;
        StartCoroutine(ReAddBuff(character));
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>()) _isInCampfire = false;
    }
    private IEnumerator ReAddBuff(Character target)
    {
        while (_isInCampfire)
        {
            target.ApplyBuff(new BurningBuff());
            yield return new WaitForSeconds(1);
        } 
    }
}