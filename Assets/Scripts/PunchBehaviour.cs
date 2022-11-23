using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehaviour : StateMachineBehaviour
{

    public TPController.TPunchType m_PunchType;
    [SerializeField] float m_StartPcTime;
    [SerializeField] float m_EndPcTime;
    TPController m_MarioController;

    bool m_PunchStarted;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PunchStarted = false;
        m_MarioController = animator.GetComponent<TPController>();
        m_MarioController.HitPunch(m_PunchType, false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_PunchStarted && stateInfo.normalizedTime >= m_StartPcTime && stateInfo.normalizedTime <= m_EndPcTime)
        {
            m_PunchStarted = true;
            m_MarioController.HitPunch(m_PunchType, true);
        }
        else if (m_PunchStarted && stateInfo.normalizedTime > m_EndPcTime)
        {
            m_PunchStarted = false;
            m_MarioController.HitPunch(m_PunchType, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MarioController.HitPunch(m_PunchType, false);
        m_MarioController.SetPunchActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
