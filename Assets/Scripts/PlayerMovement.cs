using System;
using System.Collections;
using System.Collections.Generic;
using Buffs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private Camera _camera;
    [SerializeField] private float currentSpeed;
    private Transform _cameraTransform;
    private Transform _transform;
    private Character _character;
    private Rigidbody _rigidBody;

    private float _dashDelay;

    private void Awake()
    {
        _cameraTransform = _camera.transform;
        _transform = transform;
        _character = GetComponent<Character>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _dashDelay -= Time.fixedDeltaTime;
        var lookVector = _cameraTransform.forward;
        lookVector = new Vector3(lookVector.x, 0, lookVector.z);
        //
        var lookRotation = _cameraTransform.rotation;
        //
        // var forwardVector = new Vector3(lookVectorForward.x, 0, lookVectorForward.z).normalized;
        // var rightVector = _cameraTransform.right;
        //
        var moveVector = new Vector3();
        var rotateVector = _rigidBody.rotation;
        //
        // if (Input.GetKey(KeyCode.W))
        // {
        //     moveVector += forwardVector;
        //     var position = _characterTransform.position;
        //     //rotateVector = Quaternion.FromToRotation(position, position + moveVector); //new Quaternion(lookRotation.x, lookRotation.y, lookRotation.z, 0);
        //     character.transform.rotation = Quaternion.Lerp(_cameraTransform.rotation, lookRotation, 10 * Time.deltaTime);
        //     rotateVector = lookRotation * Quaternion.Euler(new Vector3(0, 180, 180));
        //     //rotateVector = forwardRotation * Quaternion.Euler(new Vector3(0, 180, 180));
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     moveVector += rightVector;
        //     var position = _characterTransform.position;
        //     //rotateVector = Quaternion.FromToRotation(position, position + moveVector); //new Quaternion(lookRotation.x, lookRotation.y, lookRotation.z, 0);
        //     rotateVector = lookRotation * Quaternion.Euler(new Vector3(0, 90, 180));
        // }
        //
        // //_character.transform.rotation = rotateVector;
        // //_character.transform.LookAt(_character.transform.position + moveVector);
        // moveVector *= speed;
        //
        // characterRb.velocity = moveVector;
        
        var cameraRotation = _camera.transform.rotation;
        var forwardRotation = new Quaternion(cameraRotation.x, 0, cameraRotation.z, 0);
        
        var forwardVector = new Vector3(lookVector.x, 0, lookVector.z).normalized;
        var rightVector = _camera.transform.right;
        
        moveVector = new Vector3();
        rotateVector = _rigidBody.rotation;
        
        if (Input.GetKey(KeyCode.W))
        {
            moveVector += forwardVector;
            //rotateVector = forwardRotation * Quaternion.Euler(new Vector3(0, 180, 180));
            rotateVector = forwardRotation * Quaternion.Euler(new Vector3(0, 180, 180));
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            moveVector -= rightVector;
            rotateVector = forwardRotation * Quaternion.Euler(new Vector3(180, 90, 0));
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            moveVector -= forwardVector;
            rotateVector = forwardRotation * Quaternion.Euler(new Vector3(0, 0, 180));
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += rightVector;
            rotateVector = forwardRotation * Quaternion.Euler(new Vector3(0, 90, 180));
        }
        
        _transform.rotation = rotateVector;
        //_character.transform.LookAt(_character.transform.position + moveVector);
        moveVector *= speed;

        //transform.Translate(moveVector * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && _dashDelay >= 1)
        {
            _dashDelay = 0;
            _rigidBody.position += moveVector * 50;
        }
        //_rigidBody.velocity += Vector3.up * jumpForce;
        //_rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        
        var velocityCopy = _rigidBody.velocity;
        _rigidBody.velocity = new Vector3(moveVector.x, velocityCopy.y, moveVector.z);

        
        // Vector3 getGlobaleFacingVector3(float resultAngle)
        // {
        //     float num = -resultAngle + 90f;
        //     float x = Mathf.Cos(num * 0.01745329f);
        //     return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
        // }
        // float getGlobalFacingDirection(float horizontal, float vertical)
        // {
        //     if ((vertical == 0f) && (horizontal == 0f))
        //     {
        //         return transform.rotation.eulerAngles.y;
        //     }
        //     float y = _camera.transform.rotation.eulerAngles.y;
        //     float num2 = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        //     num2 = -num2 + 90f;
        //     return (y + num2);
        // }
        // float x = 0, z = 0;
        // if (Input.GetKey(KeyCode.W))
        //     z = 1;
        // if (Input.GetKey(KeyCode.A))
        //     x = -1;
        // if (Input.GetKey(KeyCode.S))
        //     z = -1;
        // if (Input.GetKey(KeyCode.D))
        //     x = 1;
        //
        // var vector = new Vector3(x, 0, z);
        // float resultAngle = getGlobalFacingDirection(x, z);
        // var zero = getGlobaleFacingVector3(resultAngle);
        // float num6 = (vector.magnitude <= 0.95f) ? ((vector.magnitude >= 0.25f) ? vector.magnitude : 0f) : 1f;
        // zero *= num6;
        // zero *= _speed;
        // var force = zero;
        // var maxVelocityChange = 10;
        // force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
        // force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
        // force.y = 0;
        // if (Input.GetKeyDown(KeyCode.Space))
        //     force.y += 8;
        //
        // _characterRB.AddForce(force, ForceMode.VelocityChange);

        // var moveVector = new Vector3();
        //
        // if (Input.GetKey(KeyCode.W))
        //     moveVector += Vector3.forward;
        // if (Input.GetKey(KeyCode.A))
        //     moveVector += Vector3.left;
        // if (Input.GetKey(KeyCode.S))
        //     moveVector += Vector3.back;
        // if (Input.GetKey(KeyCode.D))
        //     moveVector += Vector3.right;
        // moveVector *= _speed;
        // _velocity = _characterRB.velocity;
        // transform.Translate(moveVector * Time.deltaTime);
    }

    private void Update()
    {
        if (_rigidBody.velocity.y < 0)
        {
            _rigidBody.velocity += Vector3.up * (Physics.gravity.y * Time.deltaTime);
        }
    }
}
