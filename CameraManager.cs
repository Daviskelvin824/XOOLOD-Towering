using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public Transform camTarget;
    public float pLerp = 0.02f;
    public float rLerp = 0.01f;

    private void Update()
    {
        // Update position using Vector3.Lerp
        transform.position = Vector3.Lerp(transform.position, camTarget.position, pLerp);

        // Update rotation using Quaternion.Lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, camTarget.rotation, rLerp);
    }
}

//[Serializable]
//public struct MouseSensitivity
//{
//    public float horizontal;
//    public float vertical;
//    public bool invertHorizontal;
//    public bool invertVertical;
//}

//public struct CameraRotation
//{
//    public float Pitch;
//    public float Yaw;
//}

//[Serializable]
//public struct CameraAngle
//{
//    public float min;
//    public float max;
//}