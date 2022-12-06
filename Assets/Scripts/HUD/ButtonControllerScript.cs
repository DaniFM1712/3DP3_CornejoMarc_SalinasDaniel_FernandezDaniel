using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonControllerScript : MonoBehaviour
{
    [SerializeField] MarioController mario;
    [SerializeField] private UnityEvent startSounds;


    public void LastCheckpoint()
    {
        FindObjectOfType<HealthController>().substractLive();
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
        startSounds.Invoke();
    }

    public void RestartLevel()
    {
        FindObjectOfType<HealthController>().restartLives();
        mario.restartCheckpoint();
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
        startSounds.Invoke();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
