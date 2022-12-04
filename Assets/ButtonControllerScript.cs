using UnityEngine;
using UnityEngine.UI;

public class ButtonControllerScript : MonoBehaviour
{
    [SerializeField] MarioController mario;


    public void LastCheckpoint()
    {
        FindObjectOfType<HealthController>().substractLive();
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        FindObjectOfType<HealthController>().restartLives();
        mario.restartCheckpoint();
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
