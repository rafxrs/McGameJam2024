using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private float targetZoom;
    [SerializeField] private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (virtualCamera == null)
        {
            // If virtualCamera is not assigned, try to find it in the scene
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
        {
            targetZoom = virtualCamera.m_Lens.OrthographicSize;
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera not found or assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (virtualCamera != null)
        {
            float scrollData = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log(scrollData);

            // Adjust the targetZoom based on scroll input
            targetZoom -= scrollData * zoomFactor;

            // Clamp the targetZoom to avoid extreme values
            targetZoom = Mathf.Clamp(targetZoom, 8f, 50f);

            // Set the virtual camera's OrthographicSize with smooth interpolation
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
        }
    }
}
