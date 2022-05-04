using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public abstract class Character : MappedObject, IHittable
{
	[SerializeField] private int        m_Life;
	[SerializeField] private GameObject m_TooltipDamage;
	
	private int        m_MaxLife;
	private BumpEffect m_BumpEffect;

	public Action<float> OnHit { get; set; }
	
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
		SpawnTooltipDamage(_Damage);
		
		if (m_Life <= 0)
			Die();
	}
	
	private void SpawnTooltipDamage(int _Damage)
	{
		var tooltipDamage =
				PoolManager.Instance
						   .SpawnObject(m_TooltipDamage, transform.position  + Vector3.up * 4.5f, Quaternion.identity)
						   .GetComponent<TooltipDamageWorld>();
		tooltipDamage.Play(_Damage, () => PoolManager.Instance.ReleaseObject(tooltipDamage));
	}
	
	protected abstract void Die();
}
