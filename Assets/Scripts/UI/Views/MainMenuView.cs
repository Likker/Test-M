using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View<MainMenuView>
{
	public Text     m_LevelText;

	public void OnPlayButton()
	{
        FSMManager.Instance.ChangePhase(GamePhase.GAME);
    }

	protected override void Update()
	{
		base.Update();
		if (Input.GetMouseButtonDown(0))
		{
			if (FSMManager.Instance.CurrentPhase == GamePhase.MAIN_MENU)
				FSMManager.Instance.ChangePhase(GamePhase.GAME);
		}
	}

	public void Show(int _Level)
	{
		base.Show();

		m_LevelText.text = "LEVEL " + _Level.ToString();
	}
}

