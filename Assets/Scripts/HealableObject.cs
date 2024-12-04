using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class HealableObject : MonoObject
{
    public int health;
    
    private void FixedUpdate()
    {
        Transform.Rotate(0, 5, 0);
    }
}