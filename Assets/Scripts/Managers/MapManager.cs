
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public List<GameObject> m_Enemies;
	public Rect             m_Rect;

	public int m_EnemiesToPop;
	
	private void Awake()
	{
		for (int i = 0; i < m_EnemiesToPop; i++)
		{
			PopEnemy();
		}
	}

	public void PopEnemy()
	{
		GameObject enemyObj = Instantiate(m_Enemies[Random.Range(0, m_Enemies.Count)]);
		enemyObj.transform.position = GetRandomPosition();
	}

	private Vector3 GetRandomPosition()
	{
		return new Vector3(m_Rect.x + Random.Range(-0.5f, 0.5f) * m_Rect.size.x, 0.0f, m_Rect.y + Random.Range(-0.5f, 0.5f) * m_Rect.size.y);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(new Vector3(m_Rect.x, 0.25f, m_Rect.y), new Vector3(m_Rect.size.x, 0.25f, m_Rect.size.y));
	}
#endif
}
