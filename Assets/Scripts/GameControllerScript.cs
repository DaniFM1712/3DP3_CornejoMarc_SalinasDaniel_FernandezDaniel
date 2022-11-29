using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameControllerScript : MonoBehaviour
{
    // Start is called before the first frame update

    static GameControllerScript m_GameController;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();

    static public GameControllerScript GetGameController()
    {
        if (m_GameController == null)
        {
            GameObject l_GameObject = new GameObject("GameController");
            GameControllerScript.DontDestroyOnLoad(l_GameObject);
            m_GameController = l_GameObject.AddComponent<GameControllerScript>();
        }
        return m_GameController;
    }
    public void AddRestartGameElement(IRestartGameElement restartGameElement)
    {
        m_RestartGameElements.Add(restartGameElement);
    }

    public void RestartGame()
    {
        foreach(IRestartGameElement restartGameElement in m_RestartGameElements)
        {
            restartGameElement.RestartGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
}
