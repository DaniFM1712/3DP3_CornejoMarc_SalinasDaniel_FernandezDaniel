using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControllerScript : MonoBehaviour
{
    public void LastCheckpoint()
    {
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
