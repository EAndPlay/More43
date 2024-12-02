using System;
using UnityEngine;

public class MonoObject : MonoBehaviour
{
    //[HideInInspector]
    public Transform Transform;

    private void Awake()
    {
        Transform = transform;
    }
}