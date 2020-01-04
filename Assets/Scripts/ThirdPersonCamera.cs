/**
 * Most of this script was created with the help of a tutorial by Sebastian Lague.
 * Tutorial link: https://youtu.be/sNmeK3qK7oA
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 7;
    public Transform target;
    public float dstFromTarget = 10;
    public Vector2 pitchMinMax = new Vector2(0, 60);
    public float rotationSmoothTime;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    float yaw;
    float pitch;

    void Start()
    {
        rotationSmoothTime = .12f;
    }
    
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);


        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
