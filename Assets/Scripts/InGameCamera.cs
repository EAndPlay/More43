using UnityEngine;
using UnityEngine.Serialization;

public class InGameCamera : MonoBehaviour
{
    public float CameraDistance = 0.8f;
    public float SensitivityMulti = 5f;
    public static Camera mainCamera;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float distanceMulti;
    [SerializeField] private float distanceOffsetMulti;
    [SerializeField] private Transform targetTransform;
    private Transform _cameraTransform;

    void Awake()
    {
        mainCamera = Camera.main;
        _cameraTransform = mainCamera.transform;
    }

    private void MoveCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var eulerAngles = _cameraTransform.eulerAngles;
        var y = eulerAngles.y;
        //this.distanceOffsetMulti = (cameraDistance * (200f - _camera.fieldOfView)) / 150f;
        var quaternion = Quaternion.Euler(0f, y, 0f);
        var z = eulerAngles.z;
        distanceOffsetMulti = CameraDistance * (200f - mainCamera.fieldOfView) / 150f;
        //_transform.position = _transform.position;
        _cameraTransform.position = targetTransform.position + Vector3.up * (-(0.6f - CameraDistance) * 2f);
        var pos = _cameraTransform.position;// += Vector3.up * (-(0.6f - CameraDistance) * 2);


        var deltaX = Input.GetAxis("Mouse X") * SensitivityMulti;
        var deltaY = -Input.GetAxis("Mouse Y") * SensitivityMulti;
        
        _cameraTransform.RotateAround(pos, Vector3.up, deltaX);
        var num7 = _cameraTransform.rotation.eulerAngles.x % 360;
        var num8 = num7 + deltaY;
        if ((deltaY <= 0 || ((num7 >= 260 || num8 <= 260) && (num7 >= 80 || num8 <= 80))) &&
            (deltaY >= 0 || ((num7 <= 280 || num8 >= 280) && (num7 <= 100 || num8 >= 100))))
            _cameraTransform.RotateAround(_cameraTransform.position, _cameraTransform.right, deltaY);

        _cameraTransform.position -= _cameraTransform.forward * (distance * distanceMulti * distanceOffsetMulti);


        if (CameraDistance < 0.65f)
        {
            _cameraTransform.position += _cameraTransform.right * Mathf.Max((0.6f - CameraDistance) * 2f, 0.65f);
        }
    }

    void Update()
    {
        MoveCamera();
    }
}