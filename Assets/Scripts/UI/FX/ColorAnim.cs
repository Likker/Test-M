using System;
using System.Collections;
using UnityEngine;

public class ColorAnim : FXAnim
{
    public UnityEngine.UI.Graphic graphic;

    public float          duration;
    public Color          startColor;
    public Color          endColor;
    public AnimationCurve curve;

    public override void Sample(float normalizedTime)
    {
        graphic.color = Color.Lerp(startColor, endColor, curve.Evaluate(normalizedTime));
    }

    protected override IEnumerator AnimateCoroutine(Action callback)
    {
        float time = 0.0f;

        while (time < duration)
        {
            graphic.color =  Color.Lerp(startColor, endColor, curve.Evaluate(time / duration));
            time          += Time.deltaTime;
            yield return null;
        }

        graphic.color = endColor;
        callback?.Invoke();
    }
}
