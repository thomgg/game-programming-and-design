﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpSpeed;
    public GameObject camera;

    public bool grounded;
    private Rigidbody rb;
    private CheckGrounded ground_collider;
    public Vector3 grav, grav_centre;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ground_collider = GetComponentInChildren<CheckGrounded>();
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float horAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");


        
        Vector3 camera_for = new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z);
        camera_for.Normalize();
        Vector3 camera_right = Quaternion.Euler(0, 90, 0) * camera_for;

        Vector3 movement = camera_for * verAxis + camera_right * horAxis;

       

        
        if (!movement.Equals(Vector3.zero))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), 0.3f);
        else { transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation, 1f); }
        

        rb.AddForce(movement * speed * Time.deltaTime);
        //rb.AddRelativeForce(movement * speed * Time.deltaTime);

        float jumpAxis = Input.GetAxis("Jump");
        if (grounded && Mathf.Abs(jumpAxis) > 0)
        {
            rb.AddRelativeForce(0, jumpAxis * jumpSpeed, 0);
        }
        if (!grounded) rb.AddForce(Physics.gravity * 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && collision.gameObject.transform.rotation.z < 45)
                grounded = true;
  
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HeadHurtbox")
            if (Input.GetButton("Jump"))
                rb.AddForce(0, jumpSpeed * 3, 0);
            else
                rb.AddForce(0, jumpSpeed, 0);
    }

    public void SetGravityDirection(float x, float y, float z)
    {
        grav_centre = new Vector3(x, y, z);
    }
}
