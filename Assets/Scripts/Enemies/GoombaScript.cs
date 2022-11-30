using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        Vector3 movement = Vector3.zero;
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;

        GetComponent<CharacterController>().Move(movement);
        onGround = Physics.Raycast(new Ray(transform.position, Vector3.down), 0.1f);

        if (onGround)
        {
            verticalSpeed = 0.0f;
        }
    }

    public void Kill()
    {
        StartCoroutine(KillCoroutine());
        
    }

    IEnumerator KillCoroutine()
    {
        transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        yield return new WaitForSeconds(m_DeadTime);
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
