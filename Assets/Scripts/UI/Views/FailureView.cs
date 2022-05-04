using UnityEngine.SceneManagement;

public class FailureView : View<FailureView>
{
	public void OnContinueButton()
	{
        SceneManager.LoadScene("Game");
    }
}