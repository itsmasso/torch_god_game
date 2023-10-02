using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using Unity.VisualScripting;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = LevelManager.Instance.player.transform;
    }

}
