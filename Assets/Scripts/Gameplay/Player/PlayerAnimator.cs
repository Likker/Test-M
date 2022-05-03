using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	// CACHE
	private Animator         m_Animator;
	private Player           m_Player;
	private PlayerController m_PlayerController;
	
	private void Awake()
	{
		m_Animator                     =  GetComponentInChildren<Animator>();
		m_Player                       =  GetComponent<Player>();
		m_PlayerController             =  GetComponent<PlayerController>();
		
		m_Player.OnStartAttack         += OnStartAttack;
		m_Player.OnStopAttack          += OnStopAttack;
		m_PlayerController.OnStartMove += Refresh;
		m_PlayerController.OnStopMove  += Refresh;
	}
	
	private void OnStartAttack()
	{
		m_Animator.SetTrigger("Strike");
	}
	
	private void OnStopAttack()
	{
		Refresh();
	}

	private void Refresh()
	{
		m_Animator.SetTrigger(m_PlayerController.IsMoving() ? "Walk" : "Idle");
	}
}
