using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventConsumer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;


    [SerializeField] AudioClip StepGrass;
    [SerializeField] AudioClip Coin;
    [SerializeField] AudioClip Star;
    [SerializeField] AudioClip BackgroundMusic;
    [SerializeField] AudioClip RestartMusic;
    [SerializeField] AudioClip Punch1;
    [SerializeField] AudioClip Punch2;
    [SerializeField] AudioClip Punch3;
    [SerializeField] AudioClip Jumps1;
    [SerializeField] AudioClip Jumps2;
    [SerializeField] AudioClip Jumps3;


    // Update is called once per frame
    void Update()
    {
        
    }


    private void Awake()
    {
        audioSource.PlayOneShot(RestartMusic);
        musicAudioSource.loop = true;
        musicAudioSource.clip = BackgroundMusic;
        Soundtrack();
    }

    public void Step()
    {
        audioSource.PlayOneShot(StepGrass);
    }

    public void pickCoin()
    {
        audioSource.PlayOneShot(Coin);
    }
    //public void GoombaDies()
    //{
    //    GoombaDie.Play();
    //}

    //public void GoombaStep()
    //{
    //    GoombaSteps.Play();
    //}
    public void StarPick()
    {
        audioSource.PlayOneShot(Star);
    }
    public void Soundtrack()
    {
        musicAudioSource.Play();
    }

    public void StartMusic()
    {
        audioSource.PlayOneShot(RestartMusic);
    }

    void Jump1()
    {
        audioSource.PlayOneShot(Jumps1);
    }
    void Jump2()
    {
        audioSource.PlayOneShot(Jumps2);
    }
    void Jump3()
    {
        audioSource.PlayOneShot(Jumps3);
    }

    void PunchSound1()
    {
        audioSource.PlayOneShot(Punch1);
    }
    void PunchSound2()
    {
        audioSource.PlayOneShot(Punch2);
    }
    void PunchSound3()
    {
        audioSource.PlayOneShot(Punch3);
    }
    void FinishPunch()
    {
        //Debug.Log("FinishPunch!");
    }

}
