using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LookAtCamera))]
public class PlayerNotification : MonoObject
{
    [SerializeField] private TMP_Text notifyText;

    private static readonly Vector3 AddVector = Vector3.up * 0.05f;

    public void Notify(string text, Vector3 targetPosition, Color32 color)
    {
        Transform.position = targetPosition + Vector3.up;
        notifyText.text = text;
        notifyText.faceColor = color;
        StartCoroutine(MoveAnimation());
    }

    private IEnumerator MoveAnimation()
    {
        for (var i = 0; i < 35; i++)
        {
            Transform.position += AddVector;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}