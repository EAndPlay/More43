using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLog : MonoBehaviour
{
    private static TMP_Text _logTextElement;
    private static Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
        _logTextElement = Resources.Load<TMP_Text>("LogTextElement");
    }
    
    public static void Add(string text, Color32 color)
    {
        var logText = Instantiate(_logTextElement, _transform);
        logText.faceColor = color;
        logText.text = text;
        Destroy(logText, 10);
    }
}