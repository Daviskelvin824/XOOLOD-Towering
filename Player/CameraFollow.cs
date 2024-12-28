using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineFreeLook fcam;
    void Start()
    {
        var fcam = GetComponent<CinemachineFreeLook>();
        fcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        fcam.LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
