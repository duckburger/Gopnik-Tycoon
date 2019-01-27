using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] CinemachineVirtualCamera mainVirtCamera;
    [SerializeField] CinemachineVirtualCamera secondaryVirtCamera;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetFollowObject(Transform transToLookAt)
    {
        if (this.mainVirtCamera == null || this.secondaryVirtCamera == null)
        {
            Debug.LogError("Couldn't find the main or the secondary virtual camera!");
            return;
        }
        this.mainVirtCamera.gameObject.SetActive(false);
        this.secondaryVirtCamera.gameObject.SetActive(true);
        this.secondaryVirtCamera.Follow = transToLookAt;
    }

    public void ReturnCameraToPlayer()
    {
        if (this.mainVirtCamera == null || this.secondaryVirtCamera == null)
        {
            Debug.LogError("Couldn't find the main or the secondary virtual camera!");
            return;
        }
        this.secondaryVirtCamera.Follow = null;
        this.secondaryVirtCamera.gameObject.SetActive(false);
        this.mainVirtCamera.gameObject.SetActive(true);
    }

}
