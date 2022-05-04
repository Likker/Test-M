using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	// CACHE
	private int     m_Damage;
	private Vector3 m_LastPosition;
	
	public void Launch(int _Damage, Vector3 _Direction, float _Speed, float _DurationMax, LayerMask _LayerMask)
	{
		m_LastPosition = transform.position;
		m_Damage       = _Damage;
		StartCoroutine(ProjectileCoroutine(_Direction, _Speed, _DurationMax, _LayerMask));
	}

	private IEnumerator ProjectileCoroutine(Vector3 _Direction, float _Speed, float _DurationMax, LayerMask _LayerMask)
	{
		float time = 0.0f;

		while (time < _DurationMax)
		{
			
			
			if (Physics.Linecast(m_LastPosition, transform.position, out RaycastHit _Hit, _LayerMask))
			{
				Character character = _Hit.collider.GetComponent<Character>();
				if (character != null)
					character.TakeDamage(m_Damage);
				Destroy(gameObject);
				yield return null;
			}
			m_LastPosition     =  transform.position;
			transform.position += _Direction.normalized * _Speed * Time.deltaTime;
			time               += Time.deltaTime;


			yield return null;
		}
		Destroy(gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(m_LastPosition, transform.position);
	}
}
