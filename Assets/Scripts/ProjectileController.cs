﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    float speed = 2.5f;
    GameObject prompt;
    Transform target;
    Rigidbody rb;
    bool isLockedOn = true;
    PlayerController playerController;
    float sweetSpotUpper = 2f;
    float sweetSpotLower = .75f;
    bool primedForSelfDestruction = false;

    void Start()
    {
        prompt = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.GetChild(0).gameObject;
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (isLockedOn) // Player has yet to dodge
        {
            FollowPlayer();

            if (IsInSweetSpot() && playerController.isGrounded)
            {
                prompt.SetActive(true); // Jump prompt
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isLockedOn = false; // Disengage homing if the player successfully dodged
                }
            }
            else
            {
                prompt.SetActive(false); // Turn off prompt if the user manages to get out of sweetspot
            }
        }
        else { // Completely pacify the projectile
            prompt.SetActive(false); // Turn off prompt if the user successfully dodged
            transform.gameObject.tag = "RogueProjectile"; // Replace Enemy tag to indicate that the missile is not hostile
            Physics.IgnoreCollision(playerController.GetComponent<Collider>(),
                GetComponent<Collider>(), true); // Forgive player for colliding with the projectile if they dodged already
            transform.position += transform.forward * Time.deltaTime * speed; // Continue the projectile on its original path prior to dodge
            if (!primedForSelfDestruction)
            {
                Destroy(gameObject, 5); // Begin timer to destroy the projectile
                primedForSelfDestruction = true;
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized; // Get the normalized direction of the projectile
        transform.rotation = Quaternion.LookRotation(direction); // Make the projectile face the player
        Vector3 movementRequired = speed * direction * Time.deltaTime; // Multiply the direction with custom coefficients
        rb.MovePosition(transform.position + movementRequired); // Increment the current position with the movement required
    }
    
    bool IsInSweetSpot()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= sweetSpotUpper && distance >= sweetSpotLower)
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Ground") || 
            (collision.gameObject.tag == "Enemy") ||
            (collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject); // Destroy the missile if it hits anything
        }
    }
}