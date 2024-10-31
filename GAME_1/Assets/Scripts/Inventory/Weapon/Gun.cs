using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }
    public Transform _shotpoint; //пустой объект - первоначальное положение пули
    private Vector3 _shotpoint_dir;
    [SerializeField] private int dam = 1;
    public GameObject DamageEffect;
    //public LineRenderer line_rend;
    private bool isAttackingUp = false;
    private bool isAttackingDown = false;
    private bool isAttackingLeft = false;
    private bool isAttackingRight = false;
    private bool returnToIdle = true;
    public bool IsAttackingUp()
    {
        return isAttackingUp;
    }
    public bool IsAttackingDown()
    {
        return isAttackingDown;
    }
    public bool IsAttackingLeft()
    {
        return isAttackingLeft;
    }
    public bool IsAttackingRight()
    {
        return isAttackingRight;
    }
    public bool ReturnToIdle()
    {
        return returnToIdle;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        //Instantiate(_bullet, _shotpoint.position, _shotpoint.rotation);
        if (Input.GetKey(KeyCode.S))
        {
            _shotpoint_dir = -_shotpoint.up;
            isAttackingDown = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _shotpoint_dir = _shotpoint.up;
            isAttackingUp = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _shotpoint_dir = -_shotpoint.right;
            isAttackingLeft = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _shotpoint_dir = _shotpoint.right;
            isAttackingRight = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        }
    }
}