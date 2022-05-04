using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeeEnemy : Enemy
{
	[Header("Projectile")]
	public Vector2    m_DelayLaunch;
	public float     m_DurationMax;
	public float     m_SpeedProjectile;
	public int       m_Damage;
	public LayerMask m_LayerMask;

	[Header("REF")]
	public GameObject m_Projectile;

	// CACHE
	private Coroutine m_LaunchProjCo;
	
	protected override void Awake()
	{
		base.Awake();
		FSMManager.onGamePhaseChanged += OnGamePhaseChanged;
	}

	private void OnDestroy()
	{
		FSMManager.onGamePhaseChanged -= OnGamePhaseChanged;
	}

	private void OnGamePhaseChanged(GamePhase _OldPhase, GamePhase _CurrentPhase)
	{
		switch (_CurrentPhase)
		{
			case GamePhase.GAME:
				LaunchProjectile();
				break;
			case GamePhase.FAILURE:
			case GamePhase.SUCCESS:
			{
				if (m_LaunchProjCo != null)
					StopCoroutine(m_LaunchProjCo);
				break;
			}
				
		}
		
			
	}

	private void LaunchProjectile()
	{
		m_LaunchProjCo = StartCoroutine(LaunchProjectileCoroutine(LaunchProjectile));
	}

	private IEnumerator LaunchProjectileCoroutine(Action _Callback)
	{
		yield return new WaitForSeconds(Random.Range(m_DelayLaunch.x, m_DelayLaunch.y));
		Projectile projectile = Instantiate(m_Projectile, transform.position, quaternion.identity).GetComponent<Projectile>();
		projectile.Launch(m_Damage, transform.forward, m_SpeedProjectile, m_DurationMax, m_LayerMask);
		
		_Callback?.Invoke();
	}
}
