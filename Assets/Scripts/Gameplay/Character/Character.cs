using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public abstract class Character : MappedObject, IHittable
{
	public  int m_Life;
	private int m_MaxLife;
	
	public  Action<float> OnHit { get; set; }
	private BumpEffect    m_BumpEffect;

	protected override void Awake()
	{
		base.Awake();
		m_BumpEffect = GetComponentInChildren<BumpEffect>();
		m_MaxLife    = m_Life;
	}
	
	public void TakeDamage(int _Damage)
	{
		if (m_BumpEffect != null)
			m_BumpEffect.Play();
      
		m_Life -= _Damage;
		OnHit?.Invoke(Mathf.Clamp01((float)m_Life / (float)m_MaxLife));
      
		if (m_Life <= 0)
			Die();
	}

	protected abstract void Die();
}
