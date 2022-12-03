using System.Collections.Generic;
using UnityEngine;
public class GameControllerScript : MonoBehaviour
{
    // Start is called before the first frame update

    static GameControllerScript m_GameController;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    DependencyInjector m_DependencyInjector;



    static public GameControllerScript GetGameController()
    {
        if (m_GameController == null)
        {
            GameObject l_GameObject = new GameObject("GameController");
            GameControllerScript.DontDestroyOnLoad(l_GameObject);
            m_GameController = l_GameObject.AddComponent<GameControllerScript>();
            l_GameObject.AddComponent<ScoreManager>();
            m_GameController.Init();
        }
        return m_GameController;
    }

    private void Init()
    {
        m_DependencyInjector = gameObject.AddComponent<DependencyInjector>();
        gameObject.AddComponent<ScoreManager>();
    }

    public DependencyInjector GetDependencyInjector()
    {
        return m_DependencyInjector;
    }
    public void AddRestartGameElement(IRestartGameElement restartGameElement)
    {
        m_RestartGameElements.Add(restartGameElement);
    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        foreach (IRestartGameElement restartGameElement in m_RestartGameElements)
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
