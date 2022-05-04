using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    private Transform _cameraTransform;
    private Transform _transform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _transform = transform;
    }

    private void LateUpdate()
    {
        _transform.forward = _cameraTransform.forward;
    }
}