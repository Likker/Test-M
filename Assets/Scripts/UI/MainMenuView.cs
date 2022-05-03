using UnityEngine.UI;

public class MainMenuView : View<MainMenuView>
{
	public Text     m_LevelText;

	public void OnPlayButton()
	{
        FSMManager.Instance.ChangePhase(GamePhase.GAME);
    }

	public void Show(int _Level)
	{
		base.Show();

		m_LevelText.text = "LEVEL " + _Level.ToString();
	}
}

