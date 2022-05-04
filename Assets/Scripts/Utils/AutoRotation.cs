using System.Collections;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
	[SerializeField] private Vector3 _speed;

	private Coroutine m_rotCo;
	
	private void Awake()
	{
		m_rotCo = StartCoroutine(RotationCoroutine());
	}

	public void StopRotation()
	{
		if (m_rotCo != null)
			StopCoroutine(m_rotCo);
	}
	
	private IEnumerator RotationCoroutine()
	{
		while (true)
		{
			transform.Rotate(_speed * Time.deltaTime);
			yield return null;
		}
	}
}
