using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaScript : MonoBehaviour, IRestartGameElement
{

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    float verticalSpeed;
    bool onGround;
    [SerializeField] private float m_DeadTime = 3.0f;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        GameControllerScript.GetGameController().AddRestartGameElement(this);
    }

    public void Kill()
    {
        GetComponent<GoombaAIScript>().ChangeToDie();
        StartCoroutine(KillCoroutine());
    }

    IEnumerator KillCoroutine()
    {
        GetComponent<CharacterController>().enabled = false;
        transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<CharacterController>().enabled = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }
}
