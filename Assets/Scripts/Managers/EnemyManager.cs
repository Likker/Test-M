using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMB<EnemyManager>
{
	private List<Enemy>    m_Enemies;

	public void Register(Enemy _Enemy)
	{
		m_Enemies        ??= new List<Enemy>();
		m_Enemies.Add(_Enemy);
	}

	public void Unregister(Enemy _Enemy)
	{
		m_Enemies ??= new List<Enemy>();
		m_Enemies.Remove(_Enemy);
	}
	
	public bool IsEnemyAvailable()
	{
		return m_Enemies.Count > 0;
	}

	public Enemy GetClosestEnemy(Vector3 _Position)
	{
		float maxDistance  = Mathf.Infinity;
		Enemy closestEnemy = null;
		
		for (int i = 0; i < m_Enemies.Count; i++)
		{
			float distance = Vector3.Distance(m_Enemies[i].transform.position, _Position);
			if (distance < maxDistance)
			{
				closestEnemy = m_Enemies[i];
				maxDistance  = distance;
			}
		}
		
		return closestEnemy;
	}
	
}
