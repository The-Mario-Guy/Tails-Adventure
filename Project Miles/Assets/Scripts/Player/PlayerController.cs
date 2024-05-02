using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maximumSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    //[SerializeField]
    public Transform cameraTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }
        // Update is called once per frame
        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }
        animator.SetFloat("Input Magnitude", inputMagnitude);
        float speed = inputMagnitude * maximumSpeed;
          
        
             movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //Having errors
             ySpeed += Physics.gravity.y * Time.deltaTime;


       
            if (characterController.isGrounded)
            {
                lastGroundedTime = Time.time;
            }

           if (Input.GetKeyDown(KeyCode.Z))
            {
                jumpButtonPressedTime = Time.time;
            }
            

            

                if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
            {
                characterController.stepOffset = originalStepOffset;
                ySpeed = -0.5f;

                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
                {
                     ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
                }
                else
                {
                    characterController.stepOffset = 0;
                }
            }

       


            Vector3 velocity = movementDirection * maximumSpeed;
            velocity.y = ySpeed;


            characterController.Move(velocity * Time.deltaTime);

             if (movementDirection != Vector3.zero)
             {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
             }
        }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}