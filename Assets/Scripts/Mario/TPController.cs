using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPController : MonoBehaviour
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
    
    [Header("Colliders")]
    [SerializeField] Collider m_RightHandCollider;
    [SerializeField] Collider m_LeftHandCollider;
    [SerializeField] Collider m_KickCollider;

    [Header("Punch")]
    [SerializeField] float m_PunchComboTime = 2.5f;
    TPunchType m_CurrentPunch = TPunchType.RIGHT_HAND;
    float m_CurrentPunchTime;
    bool m_PunchActive = false;

    float verticalSpeed = 0.0f;
    bool onGround = false;
    bool touchingCeiling = false;
    int jumpCount = 0;

    [SerializeField] Transform cameraDummy;

    float lastTimeMoved;
    Vector3 lastMouseCoords = Vector3.zero;
    [SerializeField] private float m_BridgeForce = 3;

    void Start()
    {
        lastTimeMoved = Time.time;
        
        m_CurrentPunchTime = -m_PunchComboTime;
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_KickCollider.gameObject.SetActive(false);
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

        //Movement: camera direction + input + speed
        //If Movement Forward = Movement
        float currentSpeed = 0f;
        if (movement.magnitude > 0.0f)
        {
            currentSpeed = (Input.GetKey(runKey) ? runSpeed : walkSpeed);
            movement = movement.normalized * currentSpeed * Time.deltaTime;
            transform.forward = movement;   
        }
        Debug.Log(movement.magnitude);
        animator.SetBool("isRunning", Input.GetKey(runKey));
        animator.SetFloat("speed", currentSpeed);

        if (Input.GetKeyDown(jumpKey) && jumpCount < 3)
        {
            jumpCount++;
            switch (jumpCount)
            {
                case 1:
                    verticalSpeed = jumpSpeed;
                    break;
                case 2:
                    verticalSpeed = jumpSpeed * 1.5f;
                    break;
                case 3:
                    verticalSpeed = jumpSpeed * 2f;
                    break;
            }
            
            animator.SetInteger("jumpCount", jumpCount);
        }


        //Apply gravity to vertical speed
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
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
            jumpCount = 0;
            animator.SetInteger("jumpCount", jumpCount);
        }
        if (touchingCeiling && verticalSpeed > 0.0f) verticalSpeed = 0.0f;

        Vector3 mouseDelta = Input.mousePosition - lastMouseCoords;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            float l_MouseAxisX = Input.GetAxis("Mouse X");
            float l_MouseAxisY = Input.GetAxis("Mouse Y");
            if (lastTimeMoved + 5f <= Time.time)
            {
                if ((l_MouseAxisX == 0.00f && l_MouseAxisY == 0.0f))
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, cameraDummy.position, Time.deltaTime * 3f);
                }
                else lastTimeMoved = Time.time;

            }
            if (lastTimeMoved + 10f <= Time.time)
            {
                animator.SetBool("isCrouching", true);
            }
            else animator.SetBool("isCrouching", false);


            //isMoving = false;

        }
        else
        {
            //isMoving = true;
            lastTimeMoved = Time.time;
        }

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
        return !m_PunchActive;
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
    }
}
