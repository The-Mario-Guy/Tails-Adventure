using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody _RB;
    public float jumpForce;


    void Start()
    {
        _RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _RB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, _RB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        if (Input.GetKeyDown(KeyCode.D))
        {
            _RB.velocity = new Vector3(_RB.velocity.x, jumpForce, _RB.velocity.z);
        }
    }
}
