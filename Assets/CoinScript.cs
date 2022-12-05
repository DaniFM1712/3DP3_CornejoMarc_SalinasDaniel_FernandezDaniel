using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CoinScript : MonoBehaviour, IRestartGameElement
{
    [SerializeField] UnityEvent pickCoin;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        GameControllerScript.GetGameController().AddRestartGameElement(this);
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Pick()
    {
        pickCoin.Invoke();
        gameObject.SetActive(false);
        GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().addPoints(1);
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}