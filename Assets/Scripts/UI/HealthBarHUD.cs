using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarHUD : HealthBar
{
	protected override void Awake()
	{
		m_LifeImage.fillAmount      = 1.0f;
		m_LifeDelayImage.fillAmount = 1.0f;
	}
}
