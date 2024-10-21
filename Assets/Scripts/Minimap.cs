using System;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject character;
    private Transform _followTransform;
    private Transform _transform;
    
    private void Start()
    {
        _transform = transform;
        _followTransform = character.transform;
    }

    private void LateUpdate()
    {
        var newPos = _followTransform.position;
        newPos.y = _transform.position.y;
        _transform.position = newPos;
    }
}