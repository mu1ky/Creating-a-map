using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D rb;
    private Vector2 inputVector;
    [SerializeField]private float speed_player = 3f;
    private float minspeed = 0.1f;
    private Vector2 speed_to_axis;
    private bool togetherY;
    private bool togetherX;
    private bool isRunningUp;
    private bool isRunningDown;
    private bool isRunningLeftRight;
    private bool rev;
    //private bool isAttacking;

    /*
    public bool IsAttcking()
    {
        return isAttacking;
    }
    */

    public bool IsRunningUp()
    {
        return isRunningUp;
    }
    public bool IsRunningDown()
    {
        return isRunningDown;
    }
    public bool IsRunningLeftRight()
    {
        return isRunningLeftRight;
    }
    public bool Rev()
    {
        return rev;
    }
    private void HandleMovement()
    {
        speed_to_axis = inputVector * (speed_player * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + speed_to_axis);
        inputVector = inputVector.normalized;
    }
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void Update()
    {
        isRunningUp = false; 
        isRunningDown = false;
        isRunningLeftRight = false;
        togetherY = false;
        togetherX = false;
        rev = false;

        inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            togetherY = true;
        }
        if (Input.GetKey(KeyCode.W) && togetherY == false)
        {
            inputVector.y = 1f;
            isRunningUp = true;
        }

        if (Input.GetKey(KeyCode.S) && togetherY == false)
        {
            inputVector.y = -1f;
            isRunningDown = true;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            togetherX = true;
        }
        if (Input.GetKey(KeyCode.A) && togetherX == false)
        {
            inputVector.x = -1f;
            if (isRunningUp == false && isRunningDown == false)
            {
                isRunningLeftRight = true;
                rev = true;
            }
        }

        if (Input.GetKey(KeyCode.D) && togetherX == false)
        {
            inputVector.x = 1f;

            if (isRunningUp == false && isRunningDown == false)
            {
                isRunningLeftRight = true;
                rev = false;
            }
        }
    }
}
