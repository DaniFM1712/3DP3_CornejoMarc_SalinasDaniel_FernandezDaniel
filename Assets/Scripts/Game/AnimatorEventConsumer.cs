using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventConsumer : MonoBehaviour
{
    [SerializeField] AudioSource soundsAudioSource;
    [SerializeField] AudioSource musicAudioSource;

    [Header("Sound")]
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
    [SerializeField] AudioClip LongJump;
    [SerializeField] AudioClip HitSound;
    [SerializeField] AudioClip Death;

    [Header("Particles")]
    [SerializeField] ParticleSystem walkStepsPart;
    [SerializeField] ParticleSystem pickCoinPart;
    [SerializeField] ParticleSystem jump1StartPart;
    [SerializeField] ParticleSystem jumpPart;
    [SerializeField] ParticleSystem lifePart;
    [SerializeField] ParticleSystem punch1Part;
    [SerializeField] ParticleSystem punch2Part;
    [SerializeField] ParticleSystem punch3Part;



    private void Awake()
    {
        soundsAudioSource.PlayOneShot(RestartMusic);
        musicAudioSource.loop = true;
        musicAudioSource.clip = BackgroundMusic;
        Soundtrack();
    }

    public void Step()
    {
        walkStepsPart.Play();
        soundsAudioSource.PlayOneShot(StepGrass);
    }

    public void pickCoin()
    {
        pickCoinPart.Play();
        soundsAudioSource.PlayOneShot(Coin);
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
        lifePart.Play();
        soundsAudioSource.PlayOneShot(Star);
    }
    public void Soundtrack()
    {
        musicAudioSource.Play();
    }

    public void StartMusic()
    {
        soundsAudioSource.PlayOneShot(RestartMusic);
    }

    void Jump1()
    {
        jump1StartPart.Play();
        jumpPart.Play();
        soundsAudioSource.PlayOneShot(Jumps1);
    }
    void Jump2()
    {
        jumpPart.Play();
        soundsAudioSource.PlayOneShot(Jumps2);
    }
    void Jump3()
    {
        jumpPart.Play();
        soundsAudioSource.PlayOneShot(Jumps3);
    }

    void LongJumpSound()
    {
        jump1StartPart.Play();
        soundsAudioSource.PlayOneShot(LongJump);
    }

    void PunchSound1()
    {
        soundsAudioSource.PlayOneShot(Punch1);
    }

    void PunchSound2()
    {
        soundsAudioSource.PlayOneShot(Punch2);
    }

    void PunchSound3()
    {
        soundsAudioSource.PlayOneShot(Punch3);
    }

    public void PunchParticles(int punchIndex)
    {
        Debug.Log("Index: " + punchIndex);
        switch (punchIndex)
        {
            case 1:
                punch1Part.Play();
                break;
            case 2:
                punch2Part.Play();
                break;
            case 3:
                punch3Part.Play();
                break;
        }
    }
    void FinishPunch()
    {
        //Debug.Log("FinishPunch!");
    }

    public void HitSoundEvent()
    {
        soundsAudioSource.PlayOneShot(HitSound);
    }

    public void DeathSoundEvent()
    {
        soundsAudioSource.PlayOneShot(Death);
    }

    public void StopSounds()
    {
        musicAudioSource.Stop();
    }

    public void StartSounds()
    {
        soundsAudioSource.PlayOneShot(RestartMusic);
        musicAudioSource.Play();

    }
}
