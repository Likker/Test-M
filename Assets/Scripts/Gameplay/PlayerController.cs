using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IJoystickController
{
	public  float m_Speed;

	public Action OnMove;
	public Action OnStop;
	
	// Buffer
	private bool    m_IsMoving;
	private Vector3 m_Direction;
	
	private void Start()
	{
		m_IsMoving = false;
		Joystick.Instance.Register(this);
	}

	public void OnTouchDown()
	{
		m_Direction = Vector3.zero;
		Move();
	}

	public void OnTouch(Vector2 direction)
	{
		m_Direction = new Vector3(direction.x, 0.0f, direction.y);
	}

	public void OnTouchUp()
	{
		Stop();
		m_Direction = Vector3.zero;
	}

	private void Move()
	{
		m_IsMoving = true;
		OnMove?.Invoke();
	}
	
	private void Stop()
	{
		m_IsMoving = false;
		OnStop?.Invoke();
	}

	private void Update()
	{
		if (!m_IsMoving)
			return;
			
		transform.position += m_Direction.normalized * (m_Speed * Time.deltaTime);
	}


}
