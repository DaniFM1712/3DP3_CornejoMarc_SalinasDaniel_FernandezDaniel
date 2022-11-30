using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour, IRestartGameElement
{
    public void RestartGame()
    {
        gameObject.SetActive(false);
    }


    public void Pick()
    {
        gameObject.SetActive(false);
        GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().addPoints(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
