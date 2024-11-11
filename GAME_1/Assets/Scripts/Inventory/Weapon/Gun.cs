using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Net;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }
    public Transform _shotpoint; //пустой объект - первоначальное положение пули
    private Vector3 _shotpoint_dir;
    [SerializeField] private int dam = 1;
    public GameObject DamageEffect;
    public bool isAttacking = false;
    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Player.Instance.NowIsShooting() == true)
        {
            Shoot();
        }
    }
    private void Shoot()
    {

        if (Player.Instance.IsShootingDown())
        {
            _shotpoint_dir = -_shotpoint.up;
        }
        if (Player.Instance.IsShootingUp())
        {
            _shotpoint_dir = _shotpoint.up;
        }
        if (Player.Instance.IsShootingLeft())
        {
            _shotpoint_dir = -_shotpoint.right;
        }
        if (Player.Instance.IsShootingRight())
        {
            _shotpoint_dir = _shotpoint.right;
        }
        Debug.Log("Shoot!");
        if (InputControl.Instance.IsGetSpace_() == true)
        {
            isAttacking = true;
            /*
            RaycastHit2D hit = Physics2D.Raycast(_shotpoint.position, _shotpoint_dir);
            if (hit)
            {
                Enemy_movement en = hit.transform.GetComponent<Enemy_movement>();
                if (en != null)
                {
                    en.TakeDamage(dam);
                }
                Instantiate(DamageEffect, hit.point, Quaternion.identity);
            }
            */
        }
        else
        {
            isAttacking = false;
        }
    }
}