using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpEffect : MonoBehaviour
{
	public  AnimationCurve m_Curve;
	private Vector3        m_BaseScale;
	private Coroutine      m_BumpCoroutine;
	
	private void Awake()
	{
		m_BaseScale = transform.localScale;
	}

	public void Play()
	{
		if (m_BumpCoroutine != null)
			StopCoroutine(m_BumpCoroutine);
		m_BumpCoroutine = StartCoroutine(BumpCoroutine(0.25f));
	}

	private IEnumerator BumpCoroutine(float _Duration)
	{
		float time = 0.0f;

		while (time < _Duration)
		{
			transform.localScale =  Vector3.LerpUnclamped(Vector3.zero, m_BaseScale, m_Curve.Evaluate(time / _Duration));
			time                 += Time.deltaTime;
			yield return null;
		}
	}
}
