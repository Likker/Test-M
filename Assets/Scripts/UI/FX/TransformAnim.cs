using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnim : FXAnim
{
	public float          m_Duration;
	public Vector3        m_LocalOffset;
	public AnimationCurve m_Curve;

	public override void Sample(float normalizedTime)
	{
		Vector3 baseLocalPos = transform.localPosition;
		Vector3 endLocalPos  = baseLocalPos + m_LocalOffset;

		transform.localPosition = Vector3.LerpUnclamped(baseLocalPos, endLocalPos, m_Curve.Evaluate(normalizedTime));
	}

	protected override IEnumerator AnimateCoroutine(Action callback)
	{
		Vector3 baseLocalPos = transform.localPosition;
		Vector3 endLocalPos  = baseLocalPos + m_LocalOffset;
		float   time         = 0.0f;

		while (time < m_Duration)
		{
			transform.localPosition = Vector3.LerpUnclamped(baseLocalPos, endLocalPos, m_Curve.Evaluate(time / m_Duration));
			time += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = endLocalPos;
		callback?.Invoke();
	}
}