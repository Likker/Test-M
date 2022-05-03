using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IJoystickController
{
	public  float m_Speed;
	private bool  m_IsMoving;
	
	// Buffer
	private Vector3 m_Direction;
	
	private void Awake()
	{
		m_IsMoving = false;
		Joystick.Instance.Register(this);
	}

	public void OnTouchDown()
	{
		m_Direction = Vector3.zero;
	}

	public void OnTouch(Vector2 direction)
	{
		m_Direction = new Vector3(direction.x, 0.0f, direction.y);
		if (m_Direction.magnitude > 0.0f)
			Move();
		else
			Stop();
	}

	public void OnTouchUp()
	{
		m_Direction = Vector3.zero;
	}

	private void Move()
	{
		m_IsMoving         =  true;
		transform.position += m_Direction.normalized * (m_Speed * Time.deltaTime);
	}

	private void Stop()
	{
		m_IsMoving = false;
	}
}
