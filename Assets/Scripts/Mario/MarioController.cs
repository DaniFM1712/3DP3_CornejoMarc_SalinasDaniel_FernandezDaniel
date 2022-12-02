using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MarioController : MonoBehaviour, IRestartGameElement
{
    public enum TPunchType
    {
        RIGHT_HAND,
        LEFT_HAND, 
        KICK
    }

    [SerializeField] Camera cam;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] KeyCode fwKey;
    [SerializeField] KeyCode backKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode crouchKey;


    [Header("Colliders")]
    [SerializeField] Collider m_RightHandCollider;
    [SerializeField] Collider m_LeftHandCollider;
    [SerializeField] Collider m_KickCollider;

    [Header("Punch")]
    [SerializeField] float m_PunchComboTime = 2.5f;
    TPunchType m_CurrentPunch = TPunchType.RIGHT_HAND;
    float m_CurrentPunchTime;
    bool m_PunchActive = false;

    [Header("Elevator")]
    [SerializeField] private float m_ElevatorMaxAngleAllowed = 10.0f;
    private Collider m_CurrentElevator = null;


    float verticalSpeed = 0.0f;
    bool onGround = false;
    bool touchingCeiling = false;
    int jumpCount = 0;
    float gravityMultiplyer = 1f;

    [SerializeField] Transform cameraDummy;

    float lastTimeMoved;
    Vector3 lastMouseCoords = Vector3.zero;
    [SerializeField] private float m_BridgeForce = 3;


    CheckpointScript m_CurrentCheckpoint = null;
    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    [Header("Enemies & Health")]
    [SerializeField] float m_VerticalKillSpeed = 5.0f;
    [SerializeField] float m_KillGoombaMaxAngle = 45.0f;
    [SerializeField] private UnityEvent<float> healthUpdate;
    [SerializeField] UnityEvent makeHudVisible;


    private bool isInputAccepted = true;

    private void Awake()
    {
        GameControllerScript.GetGameController();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lastTimeMoved = Time.time;
    
        m_CurrentPunchTime = -m_PunchComboTime;
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_KickCollider.gameObject.SetActive(false);

        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        GameControllerScript.GetGameController().AddRestartGameElement(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fw = cam.transform.forward;
        fw.y = 0f;
        fw = fw.normalized;

        Vector3 right = cam.transform.right;
        right.y = 0f;
        right = right.normalized;
        
        Vector3 movement = Vector3.zero;

        //Check Input
        if (isInputAccepted)
        {
            if (Input.GetKey(fwKey))
                movement += fw;
            if (Input.GetKey(backKey))
                movement += -fw;
            if (Input.GetKey(rightKey))
                movement += right;
            if (Input.GetKey(leftKey))
                movement += -right;
            if (Input.GetMouseButtonDown(0) && CanPunch())
            {
                if (MustStartComboPunch())
                    SetComboPunch(TPunchType.RIGHT_HAND);
                else
                    NextComboPunch();
            }
            if (Input.GetKey(crouchKey))
            {
                animator.SetBool("isCrouching", true);
            }
        }

        //Movement: camera direction + input + speed
        //If Movement Forward = Movement
        float currentSpeed = 0f;
        if (movement.magnitude > 0.0f)
        {
            currentSpeed = (Input.GetKey(runKey) ? runSpeed : walkSpeed);
            movement = movement.normalized * currentSpeed * Time.deltaTime;
            transform.forward = movement;   
        }
        animator.SetBool("isRunning", Input.GetKey(runKey));
        animator.SetFloat("speed", currentSpeed);

        if (Input.GetKeyDown(jumpKey) && jumpCount < 3)
        {
            jumpCount++;
            switch (jumpCount)
            {
                case 1:
                    verticalSpeed = jumpSpeed;
                    gravityMultiplyer = 1f;
                    break;
                case 2:
                    animator.SetBool("longJump", false);
                    verticalSpeed = jumpSpeed * 1.5f;
                    gravityMultiplyer = 2f;
                    break;
                case 3:
                    verticalSpeed = jumpSpeed * 2f;
                    gravityMultiplyer = 3f;
                    break;
            }
            
            animator.SetInteger("jumpCount", jumpCount);
        }


        //Apply gravity to vertical speed
        verticalSpeed += Physics.gravity.y * gravityMultiplyer * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;

        //Move
        CollisionFlags flags = controller.Move(movement);

        //onGround =(flags & CollisionFlags.Below) != 0;
        onGround = Physics.Raycast(new Ray(transform.position, Vector3.down), 0.2f);
        
        touchingCeiling = (flags & CollisionFlags.Above) != 0;

        animator.SetFloat("verticalSpeed", verticalSpeed);
        animator.SetBool("onGround", onGround);


        if (onGround)
        {
            verticalSpeed = 0.0f;
            gravityMultiplyer = 1f;
            jumpCount = 0;
            animator.SetInteger("jumpCount", jumpCount);
        }
        if (touchingCeiling && verticalSpeed > 0.0f) verticalSpeed = 0.0f;

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        if(currentSpeed == 0)
        {
            float l_MouseAxisX = Input.GetAxis("Mouse X");
            float l_MouseAxisY = Input.GetAxis("Mouse Y");
            if (lastTimeMoved + 5f <= Time.time)
            {
                if ((l_MouseAxisX == 0.00f && l_MouseAxisY == 0.0f))
                {
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, cameraDummy.rotation, Time.deltaTime * 3f);
                    cam.transform.position = Vector3.Lerp(cam.transform.position, cameraDummy.position, Time.deltaTime * 3f);
                }
                else lastTimeMoved = Time.time;

            }
            if (lastTimeMoved + 10f <= Time.time)
            {
                animator.SetBool("isCrouching", true);
                if (Input.GetKeyDown(jumpKey))
                {
                    animator.SetBool("longJump", true);
                    jumpCount = 1;
                    StartCoroutine(LongJumpMovement());
                }
            }
            else animator.SetBool("isCrouching", false);

        }
        else
        {
            lastTimeMoved = Time.time;
            animator.SetBool("longJump", false);
            animator.SetBool("isCrouching", false);
        }
    }

    IEnumerator LongJumpMovement()
    {
        for (int i = 0; i < 80; i++)
        {
            GetComponent<CharacterController>().Move(cam.transform.forward * 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }

    private void LateUpdate()
    {
        float l_AngleY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0.0f, l_AngleY, 0.0f);
    }

    public void HitPunch(TPunchType punchType, bool active)
    {
        if (punchType == TPunchType.RIGHT_HAND)
            m_RightHandCollider.gameObject.SetActive(active);
        else if (punchType == TPunchType.LEFT_HAND)
            m_LeftHandCollider.gameObject.SetActive(active);
        else if (punchType == TPunchType.KICK)
            m_KickCollider.gameObject.SetActive(active);
    }

    bool CanPunch()
    {
        return !m_PunchActive && animator.GetBool("onGround");
    }

    bool MustStartComboPunch()
    {
        return (Time.time - m_CurrentPunchTime) > m_PunchComboTime;
    }

    public void SetPunchActive(bool isActive)
    {
        m_PunchActive = isActive;
    }

    void NextComboPunch()
    {
        if (m_CurrentPunch == TPunchType.RIGHT_HAND)
           SetComboPunch(TPunchType.LEFT_HAND);
        else if (m_CurrentPunch == TPunchType.LEFT_HAND)
            SetComboPunch(TPunchType.KICK);
        else if (m_CurrentPunch == TPunchType.KICK)
            SetComboPunch(TPunchType.RIGHT_HAND);
    }

    void SetComboPunch(TPunchType punchType)
    {
        m_CurrentPunch = punchType;
        if (punchType == TPunchType.RIGHT_HAND)
            animator.SetTrigger("Punch1");
        else if (punchType == TPunchType.LEFT_HAND)
            animator.SetTrigger("Punch2");
        else if (punchType == TPunchType.KICK)
            animator.SetTrigger("Punch3");

        m_CurrentPunchTime = Time.time;
        m_PunchActive = true;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Bridge"))
        {
            hit.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }

        if (hit.collider.CompareTag("Goomba"))
        {
            if (CanKillWithFeet(hit.normal))
            {
                hit.collider.GetComponent<GoombaScript>().Kill();
                JumpOverEnemy();
            }
            else
            {
                //empujar los dos en direcciones opuestas
                makeHudVisible.Invoke();
                Vector3 directionVector = (hit.collider.transform.position - transform.position).normalized;
                directionVector.y = 0;
                StartCoroutine(EnemyCollision(directionVector, hit.collider));
                healthUpdate.Invoke(-1.0f/8.0f);

            }
        }
    }

    IEnumerator EnemyCollision(Vector3 direction, Collider collision)
    {
        isInputAccepted = false;
        for (int i = 0; i < 30; i++)
        {
            collision.GetComponent<CharacterController>().Move(direction * 0.1f);
            GetComponent<CharacterController>().Move(-direction * 0.1f);
            yield return new WaitForEndOfFrame();
        }
        isInputAccepted = true;
    }

    private bool CanKillWithFeet(Vector3 normal)
    {
        return verticalSpeed < 0.0f && 
            Vector3.Dot(normal, Vector3.up) > Mathf.Cos(m_KillGoombaMaxAngle * Mathf.Deg2Rad);
    }
    private void JumpOverEnemy()
    {
        verticalSpeed = m_VerticalKillSpeed;
    }

    public void RestartGame()
    {
        controller.enabled = false;
        isInputAccepted = true;

        if(m_CurrentCheckpoint == null)
        {
            transform.position = m_StartPosition;
            transform.rotation = m_StartRotation;
        }
        else
        {
            transform.position = m_CurrentCheckpoint.m_RespawnPoint.position;
            transform.rotation = m_CurrentCheckpoint.m_RespawnPoint.rotation;
        }

        transform.SetParent(null);
        m_CurrentElevator = null;
        controller.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            if (m_CurrentElevator == null && CanAttachToElevator(other))
            {
                AttachToElevator(other);
            }
        }

        if (other.CompareTag("Checkpoint"))
        {
            m_CurrentCheckpoint = other.GetComponent<CheckpointScript>();
        }

        if (other.CompareTag("Coin"))
        {
            other.GetComponent<CoinScript>().Pick();
        }
        if (other.CompareTag("Life"))
        {
            other.GetComponent<LifeScript>().Pick();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            if(m_CurrentElevator == null)
            {
                if (CanAttachToElevator(other))
                {
                    AttachToElevator(other);
                }
            }
            else
            {
                if(m_CurrentElevator == other && !CanAttachToElevator(other))
                {
                    DetachElevator();
                }
            }

        }
    }

    bool CanAttachToElevator (Collider other)
    {
        return Vector3.Dot(other.transform.forward, Vector3.up) >= Mathf.Cos(m_ElevatorMaxAngleAllowed * Mathf.Rad2Deg);
    }

    void AttachToElevator(Collider other)
    {
        transform.SetParent(other.transform);
        m_CurrentElevator = other;
    }

    void DetachElevator()
    {
        transform.SetParent(null);
        m_CurrentElevator = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Elevator") && other == m_CurrentElevator)
        {
            DetachElevator();
        }
    }

    public void DisableInput()
    {
        isInputAccepted = false;
    }
}
