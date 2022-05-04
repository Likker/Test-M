using UnityEngine;

public class UIBinding : MonoBehaviour
{
    private void Awake()
    {
        FSMManager.onGamePhaseChanged += OnGamePhaseChanged;
    }

    private void OnDestroy()
    {
        FSMManager.onGamePhaseChanged -= OnGamePhaseChanged;
    }

    private void OnGamePhaseChanged(GamePhase _OldPhase, GamePhase _NewPhase)
    {
        switch (_NewPhase)
        {
            case GamePhase.MAIN_MENU:
                MainMenuView.Instance.Show();
                break;

            case GamePhase.GAME:
                MainMenuView.Instance.Hide();
                ProgressionView.Instance.Show();
                break;

            case GamePhase.SUCCESS:
                ProgressionView.Instance.Hide();
                SuccessView.Instance.Show();
                break;

            case GamePhase.FAILURE:
                ProgressionView.Instance.Hide();
                FailureView.Instance.Show();
                break;
        }
    }
}