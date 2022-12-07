using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GoombaAIScript : MonoBehaviour, IRestartGameElement
{

    NavMeshAgent agent;
    [SerializeField] LayerMask obstacleMask;
    Vector3 distanceToPlayer;
    private CharacterController controller;
    private bool onGround;
    private float verticalSpeed;
    private Animator animator;
    enum State { IDLE, PATROL, ALERT, CHASE, HIT, DIE }
    [SerializeField] State currentState;

    [SerializeField] GameObject player;
    [SerializeField] float hearDistance;

    [Header("IDLE")]
    [SerializeField] float idleTime;
    float idleStarted = 0.0f;

    [Header("PATROL")]
    [SerializeField] List<Transform> patrolTargets;
    [SerializeField] float patrolMinDistance;
    [SerializeField] int patrolRoundsToIdle;
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolAcceleration;
    int currentPatrolTarget = 0;

    [Header("ALERT")]
    [SerializeField] float alertSpeedRotation;
    float totalRotated = 0.0f;

    [Header("CHASE")]
    [SerializeField] float CHASE_MAX_RANGE;


    private void Awake()
    {
        GameControllerScript.GetGameController().AddRestartGameElement(this);
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        currentState = State.IDLE;
    }

    void Update()
    {
        distanceToPlayer = player.transform.position - transform.position;
        onGround = Physics.Raycast(new Ray(transform.position, Vector3.down), 0.1f);
        if (!onGround)
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }
        if (onGround)
        {
            verticalSpeed = 0.0f;
        }

        switch (currentState)
        {
            case State.IDLE:
                animator.SetInteger("AnimationIndex", 0);
                UpdateIdle();
                ChangeFromIdle();
                break;
            case State.PATROL:
                animator.SetInteger("AnimationIndex", 1);
                UpdatePatrol();
                ChangeFromPatrol();
                break;
            case State.ALERT:
                animator.SetInteger("AnimationIndex", 3);
                UpdateAlert();
                ChangeFromAlert();
                break;
            case State.CHASE:
                animator.SetInteger("AnimationIndex", 2);
                break;
            case State.HIT:
                animator.SetInteger("AnimationIndex", 1);
                break;
            case State.DIE:
                animator.SetInteger("AnimationIndex", 4);
                break;
        }
    }


    void UpdateIdle()
    {
        //Do nothing
    }

    void ChangeFromIdle()
    {

        if (HearsPlayer())
        {
            currentState = State.ALERT;
            animator.SetInteger("AnimationIndex", 3);
        }
        if (Time.time >= idleStarted + idleTime)
        {
            currentState = State.PATROL;
            currentPatrolTarget = 0;
        }
    }

    private void UpdatePatrol()
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.speed = patrolSpeed;
        agent.acceleration = patrolAcceleration;
        if (agent.hasPath && agent.remainingDistance <= patrolMinDistance)
            currentPatrolTarget++;

        agent.SetDestination(patrolTargets[currentPatrolTarget % patrolTargets.Count].position);

    }

    private void ChangeFromPatrol()
    {
        if (currentPatrolTarget > patrolRoundsToIdle * patrolTargets.Count)
        {
            currentState = State.IDLE;
            idleStarted = Time.time;
        }

        if (HearsPlayer())
        {
            currentState = State.ALERT;
            animator.SetInteger("AnimationIndex", 3);
            totalRotated = 0.0f;
        }
    }

    void UpdateAlert()
    {
        animator.SetInteger("AnimationIndex", 2);
        agent.isStopped = true;
        float frameRotation = alertSpeedRotation * Time.deltaTime;
        transform.Rotate(new Vector3(0.0f, frameRotation, 0.0f));
        totalRotated += frameRotation;
    }

    void ChangeFromAlert()
    {

        if (SeesPlayer())
        {
            currentState = State.CHASE;
            agent.enabled = false;
            StartCoroutine(Chase());
        }
        if (!HearsPlayer() || totalRotated >= 360.0f)
        {
            currentState = State.IDLE;
            totalRotated = 0.0f;
            idleStarted = Time.time;
        }
    }

    bool SeesPlayer()
    {
        float playerDistance = distanceToPlayer.magnitude;

        if (Vector3.Angle(transform.forward, distanceToPlayer.normalized) <= 5)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y+3f, transform.position.z);

            Ray r = new Ray(position, distanceToPlayer.normalized);
            if (Physics.Raycast(r, out RaycastHit hitInfo, playerDistance, obstacleMask))
            {
                return false;
            }
            return true;
        }

        return false;

    }
    bool HearsPlayer()
    {
        return distanceToPlayer.magnitude < hearDistance;
    }


    IEnumerator Chase()
    {
        Vector3 initialPosition = transform.position;
        int moveRepetitions = 0;
        while(moveRepetitions < 60 && currentState == State.CHASE)
        {
            Vector3 movement = transform.forward;
            movement.y += verticalSpeed * Time.deltaTime;
            controller.Move(movement * 0.1f);
            moveRepetitions++;
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        ChangeFromChase();
    }


    void ChangeFromChase()
    {
        agent.enabled = true;
        if (!HearsPlayer())
        {
            currentState = State.PATROL;
            
        }
        else
        {
            currentState = State.ALERT;
            animator.SetInteger("AnimationIndex", 3);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.CompareTag("Player"))
        {

            Vector3 directionVector = (hit.collider.transform.position - transform.position).normalized;
            directionVector.y = 0;
            currentState = State.HIT;
            hit.collider.gameObject.GetComponent<MarioController>().GetDamaged(-1.0f / 8.0f);
            StartCoroutine(EnemyCollision(directionVector, hit.collider));

        }
    }

    IEnumerator EnemyCollision(Vector3 direction, Collider collision)
    {
        while (distanceToPlayer.magnitude < 5f)
        {
            collision.GetComponent<CharacterController>().Move(direction * 0.1f);
            controller.Move(-direction * 0.1f);
            yield return new WaitForEndOfFrame();
        }

    }

    public void ChangeToDie()
    {
        currentState = State.DIE;
        
    }

    public void RestartGame()
    {
        currentState = State.IDLE;
    }
}
