/**
 * Most of this script was created with the help of a tutorial by Sebastian Lague.
 * Tutorial link: https://youtu.be/ZwD1UHNCzOc
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float turnSmoothVelocity;
    public bool isGrounded = false;

    float speed = 5;
    float turnSmoothTime = 0.2f;
    float jumpHeight = 7.5f;
    Rigidbody rigidBody;
    Transform cameraT;
 
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;
    }
    
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        transform.Translate(transform.forward * speed * inputDir.magnitude * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }

        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("TestRange");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("TestRange");
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
