using UnityEngine;


public class ButtonControllerScript : MonoBehaviour
{
    [SerializeField] MarioController mario;
    public void LastCheckpoint()
    {
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        mario.restartCheckpoint();
        GameControllerScript.GetGameController().RestartGame();
        transform.parent.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
