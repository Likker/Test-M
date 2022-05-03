using UnityEngine;

public enum GamePhase
{
	MAIN_MENU,
	GAME,
	SUCCESS,
    FAILURE
}

public class FSMManager : SingletonMB<FSMManager>
{
	public delegate void OnGamePhaseChanged(GamePhase _oldPhase, GamePhase _newPhase);
	public static event OnGamePhaseChanged onGamePhaseChanged;

	public GamePhase CurrentPhase { get; private set; }

	private void Start()
	{
		ChangePhase(GamePhase.MAIN_MENU);
	}

	/// <summary>
	/// Changes the phase.
	/// </summary>
	/// <param name="_GamePhase">Game phase.</param>
	public void ChangePhase(GamePhase _GamePhase)
	{
		if (onGamePhaseChanged != null)
			onGamePhaseChanged.Invoke(CurrentPhase, _GamePhase);

		CurrentPhase = _GamePhase;
	}
}