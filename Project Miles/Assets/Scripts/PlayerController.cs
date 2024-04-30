using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
  

    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }
        // Update is called once per frame
        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            float magnitude = Mathf.Clamp01(movementDirection.magnitude) * moveSpeed;
        //movementDirection.Normalize(); <--- grid movement

            ySpeed += Physics.gravity.y * Time.deltaTime;


        if (characterController.isGrounded)
        {
            ySpeed = -0.5f;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                 ySpeed = jumpSpeed;
            }
        }
            



            Vector3 velocity = movementDirection * magnitude;
            velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
             if (movementDirection != Vector3.zero)
             {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
             }
        }
    
}