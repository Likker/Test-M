using System;
using System.Collections;
using UnityEngine;

public abstract class FXAnim : MonoBehaviour
{
	private Coroutine _coroutine;

	public abstract void Sample(float normalizedTime);

	protected abstract IEnumerator AnimateCoroutine(Action callback);

	public void Play(Action callback = null)
	{
		Stop();
		_coroutine = StartCoroutine(AnimateCoroutine(callback));
	}

	public void Stop()
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);
	}
}