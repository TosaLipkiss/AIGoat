using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    [Header("Gravity")]
    public float gravity;
    public float currentGravity;
    public float constantGravity;
    public float maxGravity;

    Vector3 gravityDirection;
    Vector3 gravityMovement;

    private void Awake()
    {
        gravityDirection = Vector3.down;
    }

    void Update()
    {
        CalculateGravity();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move((move * speed * Time.deltaTime) + gravityMovement);
    }

    bool IsGrounded()
    {
        return controller.isGrounded;
    }

    void CalculateGravity()
    {
        if(IsGrounded())
        {
            currentGravity = constantGravity;
        }
        else
        {
            if(currentGravity > maxGravity)
            {
                currentGravity -= gravity * Time.deltaTime;
            }
        }

        gravityMovement = gravityDirection * -currentGravity;
    }
}
