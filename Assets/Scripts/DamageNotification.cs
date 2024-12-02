using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LookAtCamera))]
public class DamageNotification : MonoObject
{
    [SerializeField] private TMP_Text damageText;

    private static readonly Vector3 AddVector = Vector3.up * 0.05f;

    public void Notify(int damage, Vector3 targetPosition)
    {
        Transform.position = targetPosition + Vector3.up;
        damageText.text = damage.ToString();
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