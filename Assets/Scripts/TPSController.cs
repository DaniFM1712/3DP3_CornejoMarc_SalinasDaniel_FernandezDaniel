using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour
{
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

    float verticalSpeed = 0.0f;
    bool onGround = false;
    bool touchingCeiling = false;

    void Start()
    {
        
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

        //Movement: camera direction + input + speed
        //If Movement Forward = Movement
        if (movement.magnitude > 0.0f)
        {
            float currentSpeed = (Input.GetKey(runKey) ? runSpeed : walkSpeed);
            movement = movement.normalized * currentSpeed * Time.deltaTime;
            transform.forward = movement;   
        }
        animator.SetFloat("speed", movement.magnitude);

        if (Input.GetKey(jumpKey) && onGround)
            verticalSpeed = jumpSpeed;

        //Apply gravity to vertical speed
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;

        //Move
        CollisionFlags flags = controller.Move(movement);

        onGround = (flags & CollisionFlags.Below) != 0;
        touchingCeiling = (flags & CollisionFlags.Above) != 0;

        if (onGround) verticalSpeed = 0.0f;
        if (touchingCeiling && verticalSpeed > 0.0f) verticalSpeed = 0.0f;


    }
}
