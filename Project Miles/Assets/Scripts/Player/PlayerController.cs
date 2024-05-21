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

    //public float acceleration = 1.0f;
    //public float maxSpeed = 60.0f;

    //private float curSpeed = 0.0f;

    public bool isFlying;
    public bool canFly;
    public bool isHurt;
    public bool inWater;

    public GameObject jumpModel;
    public GameObject playerModel;
    public GameObject waterEffect;

    public AudioSource Audio;
    public AudioClip jumpSFX;
    public AudioClip flyingSFX;
 

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
        Audio = GetComponent<AudioSource>();
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

       
             movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //Camera movement
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

           if (Input.GetKeyDown(KeyCode.Space) && canFly == false)
           {
                 Audio.PlayOneShot(jumpSFX, 1f);
                jumpButtonPressedTime = Time.time;
                jumpModel.GetComponent<Renderer>().enabled = true;
                playerModel.SetActive(false);
                canFly = true;
           }
            if (Input.GetKeyDown(KeyCode.Space) && !characterController.isGrounded && canFly == true && flyCount <3)
           {
            Audio.PlayOneShot(flyingSFX);
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
        gravity = -2.1f;
        maximumSpeed = 21;
        yield return new WaitForSeconds(1.1f);
        gravity = 3.9f;
        maximumSpeed = 17;
    }

    private IEnumerator WaterStuff()
    {

        inWater = true;
        maximumSpeed = maximumSpeed / 2;
        jumpSpeed = jumpSpeed / 2;
        gravity = gravity - 3.4f;
        yield return new WaitForSeconds(18f);
    }

    private IEnumerator hurt()
    {
       
        isHurt = true;
        yield return new WaitForSeconds(1.2f);
        isHurt = false;
    }


    void OnTriggerEnter(UnityEngine.Collider other)
    {
        PlayerInventory PlayerInventory = gameObject.GetComponent<PlayerInventory>();
        if (other.gameObject.CompareTag("Enemy"))
        {
           // PlayerInventory.NumberOfDiamonds = 0;
            StartCoroutine(hurt());
        }
        if (other.gameObject.CompareTag("Water"))
        {
            inWater = true;
            waterEffect.SetActive(true);
            StartCoroutine(WaterStuff());
        }
        
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            waterEffect.SetActive(false);
            inWater = false;
            maximumSpeed = maximumSpeed * 2;
            jumpSpeed = jumpSpeed * 2;
            gravity = gravity + 2.9f;
        }
    }


}