using UnityEngine;
using UnityEngine.UI;

public class ProgressionView : View<ProgressionView>
{
	public HealthBarHUD m_HealthBarHUD;
	
	public void UpdateLife(float _PercentLife)
	{
		m_HealthBarHUD.Refresh(_PercentLife);
	}
}