using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLog : MonoBehaviour
{
    private static PlayerNotification _playerNotification;
    
    private void Awake()
    {
        _playerNotification = Resources.Load<PlayerNotification>("PlayerNotification");
    }
    
    public static void Add(string text, Color32 color, Character character)
    {
        var logText = Instantiate(_playerNotification, GameGlobals.IndependentObjects);
        logText.Notify(text, character.Transform.position + Vector3.up * 2, color);
    }
}