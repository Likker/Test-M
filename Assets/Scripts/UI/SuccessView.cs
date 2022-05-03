using UnityEngine.SceneManagement;

public class SuccessView : View<SuccessView>
{
    public void OnContinueButton()
	{
        SceneManager.LoadScene(0);
    }
}