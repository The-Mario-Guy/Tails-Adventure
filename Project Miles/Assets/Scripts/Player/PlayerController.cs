using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maximumSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;
    public float gravity;
    public float flyCount;

    public float acceleration = 1.0f;
    public float maxSpeed = 60.0f;

    private float curSpeed = 0.0f;

    public bool isFlying;
    public bool canFly;
    public bool isHurt;

    public GameObject jumpModel;
    public GameObject playerModel;

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
        animator.SetBool("isFlying", isFlying);
        animator.SetBool("isHurt", isHurt);
        float speed = inputMagnitude * maximumSpeed;

       
             movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //Having errors
             ySpeed += Physics.gravity.y * Time.deltaTime * gravity;


       
            if (characterController.isGrounded)
            {
                isFlying = false;
                canFly = false;
                lastGroundedTime = Time.time;
                jumpModel.GetComponent<Renderer>().enabled = false;
                playerModel.SetActive(true);
                flyCount = 0;
        }

           if (Input.GetKeyDown(KeyCode.Space))
           {
                jumpButtonPressedTime = Time.time;
                jumpModel.GetComponent<Renderer>().enabled = true;
                playerModel.SetActive(false);
                canFly = true;
           }
           if (Input.GetKeyDown(KeyCode.Space) && !characterController.isGrounded && canFly == true && flyCount <3)
           {
            isFlying = true;
            StartCoroutine(Flying());
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

       


            Vector3 velocity = movementDirection * maximumSpeed; //speed
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
    private IEnumerator Flying()
    {
        flyCount = flyCount +1;
        jumpModel.GetComponent<Renderer>().enabled = false;
        playerModel.SetActive(true);
        gravity = -1.9f;
        yield return new WaitForSeconds(0.8f);
        gravity = 3.9f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isHurt = true;
            Vector3 direction = (transform.position - collision.transform.position).normalized;
        }
    }
}