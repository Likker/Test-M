using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IJoystickController
{
	public  float m_Speed;
	 
	// CACHE
	private Rigidbody m_RigidBody;
	
	public Action OnStartMove;
	public Action OnStopMove;
	
	// BUFFER
	private bool    m_IsMoving;
	private Vector3 m_Direction;

	private void Awake()
	{
		m_RigidBody = GetComponent<Rigidbody>();
	}
	
	private void Start()
	{
		m_IsMoving = false;
		Joystick.Instance.Register(this);
	}

	public void OnTouchDown()
	{
		m_Direction = Vector3.zero;
		StartMove();
	}

	public void OnTouch(Vector2 direction)
	{
		m_Direction = new Vector3(direction.x, 0.0f, direction.y);
	}

	public void OnTouchUp()
	{
		StopMove();
		m_Direction = Vector3.zero;
	}

	private void StartMove()
	{
		m_IsMoving = true;
		OnStartMove?.Invoke();
	}
	
	private void StopMove()
	{
		m_IsMoving = false;
		OnStopMove?.Invoke();
	}

	public void Move(Vector3 _Direction)
	{
		transform.forward = Vector3.Lerp(transform.forward, _Direction, Time.fixedDeltaTime * 12.0f);
		m_RigidBody.MovePosition(m_RigidBody.position + _Direction.normalized * (m_Speed * Time.fixedDeltaTime));
	}
	
	private void FixedUpdate()
	{
		if (!m_IsMoving)
			return;

		Move(m_Direction.normalized);
	}

	public Vector3 GetPosition()
	{
		return m_RigidBody.position;
	}
}
