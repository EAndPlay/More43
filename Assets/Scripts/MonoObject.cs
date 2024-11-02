using System;
using UnityEngine;

public class MonoObject : MonoBehaviour
{
    public Transform Transform;

    private void Awake()
    {
        Transform = transform;
    }
}