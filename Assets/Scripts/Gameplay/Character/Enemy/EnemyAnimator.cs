using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
	private Character m_Character;
	private Animator  m_Animator;
	
	private void Awake()
	{
		m_Character          =  GetComponent<Character>();
		m_Animator           =  GetComponentInChildren<Animator>();
		m_Character.OnHit    += OnHit;
		m_Character.OnAttack += OnAttack;
		m_Character.OnDie    += OnDie;
	}

	private void OnDestroy()
	{
		m_Character.OnHit    -= OnHit;
		m_Character.OnAttack -= OnAttack;
		m_Character.OnDie    -= OnDie;
	}

	private void OnAttack()
	{
		m_Animator.SetTrigger("Attack");
	}
	
	private void OnHit(float _DamagePercentage)
	{
		m_Animator.SetTrigger("Damaged");
	}

	private void OnDie()
	{
		m_Animator.SetTrigger("Die");
	}

}
