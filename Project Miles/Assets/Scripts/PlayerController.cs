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
    //1:17 https://www.youtube.com/watch?v=ynh7b-AUSPE&list=PLx7AKmQhxJFaj0IcdjGJzIq5KwrIfB1m9&index=6&ab_channel=KetraGames

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

        //transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
        characterController.SimpleMove(movementDirection * magnitude);

             if (movementDirection != Vector3.zero)
             {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
             }
        }
    
}