using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D rb;
    private Vector2 inputVector;
    [SerializeField] private float speed_player = 3f;
    private float minspeed = 0.1f;
    private Vector2 speed_to_axis;
    private bool togetherY;
    private bool togetherX;
    private bool isRunningUp;
    private bool isRunningDown;
    private bool isRunningLeftRight;
    private bool rev;
    private bool isAttackingUp;
    private bool isAttackingDown;
    private bool isAttackingLeft;
    private bool isAttackingRight;
    private bool isUpDown;
    private bool isStandingUp;
    private bool isStandingDown;
    private bool isStandingRight;
    private bool isStandingLeft;
    private bool isShooting = false;
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
    public bool IsAttackingUp()
    {
        return isAttackingUp;
    }
    public bool IsAttackingDown()
    {
        return isAttackingDown;
    }
    public bool IsAttackingRight()
    {
        return isAttackingRight;
    }
    public bool IsAttackingLeft()
    {
        return isAttackingLeft;
    }
    public bool IsStandingUp()
    {
        return isStandingUp;
    }
    public bool IsStandingDown()
    {
        return isStandingDown;
    }
    public bool IsStandingRight()
    {
        return isStandingRight;
    }
    public bool IsStandingLeft()
    {
        return isStandingLeft;
    }
    public bool ReturnToIdle()
    {
        return !isShooting;
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
        inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1f;
        }
        moving_mode(ref isShooting);
        Animation(isShooting); 
    }

    private void moving_mode(ref bool isShooting)
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isShooting = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isShooting = false;
        }
    }
    private void Animation(bool isShooting)
    {
        isUpDown = false;

        isRunningUp = false;
        isRunningDown = false;
        isRunningLeftRight = false;
        rev = false;
        togetherY = false;
        togetherX = false;
        
        isAttackingUp = false;
        isAttackingDown = false;
        isAttackingLeft = false;
        isAttackingRight = false;

        isStandingUp = false;
        isStandingDown = false;
        isStandingRight = false;
        isStandingLeft = false;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            togetherY = true;
        }
        if (Input.GetKey(KeyCode.W) && togetherY == false)
        {
            if(isShooting == false)
            {
                isRunningUp = true;
            }
            else
            {
                isAttackingUp = true;
            }
            isUpDown = true;
        }
        if (Input.GetKeyUp(KeyCode.W) && isShooting)
        {
            isStandingUp = true;
        }     
        
        if (Input.GetKey(KeyCode.S) && togetherY == false)
        {
            if (isShooting == false)
            {
                isRunningDown = true;
            }
            else
            {
                isAttackingDown = true;
            }
            isUpDown = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && isShooting)
        {
            isStandingDown = true;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            togetherX = true;
        }
        if (Input.GetKey(KeyCode.A) && togetherX == false)
        {
            if (isUpDown == false)
            {
                if (isShooting == false)
                {
                    isRunningLeftRight = true;
                    rev = true;
                }
                else 
                { 
                    isAttackingLeft = true;
                }
            } 
        }
        if (Input.GetKeyUp(KeyCode.A) && isShooting)
        {
            isStandingLeft = true;
        }

        if (Input.GetKey(KeyCode.D) && togetherX == false)
        {
            if (isUpDown == false)
            {
                if (isShooting == false)
                {
                    isRunningLeftRight = true;
                    rev = false;
                }
                else 
                { 
                    isAttackingRight = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.D) && isShooting)
        {
            isStandingRight = true;
        }
    }
}