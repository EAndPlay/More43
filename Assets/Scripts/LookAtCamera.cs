using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtCamera : MonoObject
    {
        public static Transform CameraTransform;

        private void FixedUpdate() 
        {
            Transform.LookAt(CameraTransform.position);
        }
    }
}