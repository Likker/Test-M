using System;
using UnityEngine;

public class Joystick : SingletonMB<Joystick>
{
    [Header("Options")]
    public bool m_Lock;
    
    [Header("Config")]
    public float            m_Amplitude;
    public float            m_DeadZone;

    [Header("Refs")]
    [SerializeField] private RectTransform m_ContainerTr;
    [SerializeField] private RectTransform m_DotTr;

    // Cache
    private RectTransform       m_Transform;
    private IJoystickController m_JoystickControllerImpl;

    // Buffers
    private bool            m_IsTouching;
    private Vector2         m_StartPosition;
    private Vector2         m_Position;
    private Vector2         m_Direction;
    
    private void Awake()
    {
        // Cache
        m_Transform = GetComponent<RectTransform>();

        // Buffers
        m_IsTouching = false;
        m_StartPosition = m_ContainerTr.anchoredPosition;
        
        m_ContainerTr.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Transform, Input.mousePosition, null, out Vector2 uiPos);
            m_IsTouching                   = true;
            
            m_ContainerTr.anchoredPosition = uiPos;
            m_Position                     = Vector2.zero;
            m_Direction                    = Vector2.zero;
            
            m_ContainerTr.gameObject.SetActive(true);
            m_JoystickControllerImpl.OnTouchDown();
        }

        if (m_IsTouching)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Transform, Input.mousePosition, null, out Vector2 uiPos);

            Vector2 diffFromStart = uiPos - m_ContainerTr.anchoredPosition;
            Vector2 normDiff = diffFromStart.normalized;
            float diffFromStartMag = diffFromStart.magnitude;
            
            if (m_Lock)
                UpdateLock(diffFromStart, normDiff, diffFromStartMag);
            else
                UpdateFree(diffFromStart, normDiff, diffFromStartMag);

            if (diffFromStartMag < m_DeadZone)
                m_Direction = Vector2.zero;
            else
                m_Direction = normDiff;
            
            m_JoystickControllerImpl.OnTouch(m_Direction);
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_ContainerTr.gameObject.SetActive(false);
            
            m_IsTouching = false;
            m_ContainerTr.anchoredPosition = m_StartPosition;
            m_DotTr.localPosition = Vector3.zero;
            m_Position = Vector2.zero;
            m_Direction = Vector2.zero;
            
            m_JoystickControllerImpl.OnTouchUp();
        }
    }

    private void UpdateFree(Vector2 _DiffFromStart, Vector2 _NormDiff, float _DiffFromStartMag)
    {
        if (_DiffFromStartMag < m_Amplitude)
        {
            m_DotTr.localPosition = _DiffFromStart;
            m_Position            = _NormDiff * (_DiffFromStartMag / m_Amplitude);
        }
        else
        {
            m_ContainerTr.anchoredPosition += _NormDiff * (_DiffFromStartMag - m_Amplitude);
            m_DotTr.localPosition          =  _NormDiff * m_Amplitude;
            m_Position                     =  _NormDiff;
        }
    }

    private void UpdateLock(Vector2 _DiffFromStart, Vector2 _NormDiff, float _DiffFromStartMag)
    {
        if (_DiffFromStartMag < m_Amplitude)
        {
            m_DotTr.localPosition = _DiffFromStart;
            m_Position            = _NormDiff * (_DiffFromStartMag / m_Amplitude);
        }
        else
        {
            m_DotTr.localPosition = _NormDiff * m_Amplitude;
            m_Position            = _NormDiff;
        }
    }

    public void Register(IJoystickController _JoystickController)
    {
        m_JoystickControllerImpl = _JoystickController;
    }
    
    public Vector2 GetJoystickPosition()
    {
        return m_Position;
    }

    public Vector2 GetJoystickDirection()
    {
        return m_Direction;
    }

    public bool IsTouching()
    {
        return m_IsTouching;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_ContainerTr == null)
            return;

        Gizmos.color = Color.blue;
        Vector3 amplitudeOffset = Vector3.right * m_Amplitude;
        amplitudeOffset = transform.TransformVector(amplitudeOffset);
        Gizmos.DrawWireSphere(m_ContainerTr.position, amplitudeOffset.x);

        Gizmos.color = Color.red;
        Vector3 deadZoneOffset = Vector3.right * m_DeadZone;
        deadZoneOffset = transform.TransformVector(deadZoneOffset);
        Gizmos.DrawWireSphere(m_ContainerTr.position, deadZoneOffset.x);
    }
#endif

}

