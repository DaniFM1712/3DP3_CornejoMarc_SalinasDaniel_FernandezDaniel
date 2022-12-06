using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaEventConsumer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip GoombaDie;

    public void GoombaDies()
    {
        audioSource.PlayOneShot(GoombaDie);
    }

}
