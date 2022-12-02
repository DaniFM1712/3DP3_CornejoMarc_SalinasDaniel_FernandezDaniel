using UnityEngine;
using UnityEngine.Events;

public class LifeScript : MonoBehaviour, IRestartGameElement
{
    [SerializeField] UnityEvent makeHudVisible;
    [SerializeField] UnityEvent <float> healthUpdate;
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
        gameObject.SetActive(false);
        healthUpdate.Invoke(1.0f / 8.0f);
        //GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().addPoints(1);
        makeHudVisible.Invoke();
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}

