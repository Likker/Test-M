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

	public Action        OnAttack { get; set; }
	public Action<float> OnHit    { get; set; }
	public Action        OnDie    { get; set; }
	
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

		if (m_Life <= 0 && m_Life + _Damage > 0)
		{
			OnDie?.Invoke();
			Die();
		}

	}
	
	private void SpawnTooltipDamage(int _Damage)
	{
		var tooltipDamage =
				PoolManager.Instance
						   .SpawnObject(m_TooltipDamage, transform.position  + Vector3.up * 4.5f, Quaternion.identity)
						   .GetComponent<TooltipDamageWorld>();
		tooltipDamage.Play(_Damage, () => PoolManager.Instance.ReleaseObject(tooltipDamage));
	}

	protected virtual void Die()
	{
		if (this is Enemy)
			StartCoroutine(DieCoroutine());
	}

	private IEnumerator DieCoroutine()
	{
		Vector3 baseScale = transform.localScale;
		float   time      = 0.0f;
		
		yield return new WaitForSeconds(0.75f);

		while (time < 0.35f)
		{
			transform.localScale =  Vector3.Lerp(baseScale, Vector3.zero, time / 0.35f);
			time                 += Time.deltaTime;
			yield return null;
		}
		
		Destroy(gameObject);
	}
}
