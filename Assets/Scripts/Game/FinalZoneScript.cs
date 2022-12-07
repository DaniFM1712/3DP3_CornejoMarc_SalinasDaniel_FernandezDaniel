using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalZoneScript : MonoBehaviour
{
    [SerializeField] private UnityEvent disableFeature;
    [SerializeField] private GameObject finalScreen;

    private void Start()
    {
        finalScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            disableFeature.Invoke();
            finalScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
