using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventConsumer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    void Step()
    {
        Debug.Log("Step!");
    }

    void PunchSound1(AnimationEvent animation)
    {
        Debug.Log("Punch1!");
    }
    void PunchSound2(AnimationEvent animation)
    {
        Debug.Log("Punch2!");
    }
    void PunchSound3(AnimationEvent animation)
    {
        Debug.Log("Punch3!");
    }
    void FinishPunch(AnimationEvent animation)
    {
        Debug.Log("FinishPunch!");
    }

}
