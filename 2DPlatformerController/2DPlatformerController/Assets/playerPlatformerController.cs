﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlatformerController : PhysicsObject {
    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    private float test = 23;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded )
        {
            //adds veloctiy along y access
            velocity.y = jumpTakeOffSpeed;
            //test = velocity.magnitude;
            Debug.Log("getting called when hit jump and grounded " );
        }
    //cancels jump when releasing button
        else if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("cancelling jump");
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
                
            }
        }

        //makes sprite flip back and forth depending on direction moving
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;

        }
       
        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;

       
    }
}
