using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Zoom")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float defaultFov = 60f;
    [SerializeField] private float zoomedFov = 40f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Screen Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private float targetFov;

    private void Awake()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CameraManager: Virtual Camera reference is not set", this);
        }
        targetFov = defaultFov;
    }

    private void Update()
    {
        if (virtualCamera == null) return;
        
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * zoomSpeed);
    }

    public void HandleChargeUpdate(float chargeRatio)
    {
        targetFov = Mathf.Lerp(defaultFov, zoomedFov, chargeRatio);
    }

    public void TriggerLandShake()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
} 