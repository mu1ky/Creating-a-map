using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private const string is_run_up = "IsRunningUp";
    private const string is_run_down = "IsRunningDown";
    private const string is_run_left_right = "IsRunningLeftRight";
    private const string is_attack_up = "IsAttackingUp";
    private const string is_attack_down = "IsAttackingDown";
    private const string is_attack_left = "IsAttackingLeft";
    private const string is_attack_right = "IsAttackingRight";
    //private const string is_attack = "IsAttacking"; //?
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Update()
    {
        anim.SetBool(is_run_up, Player.Instance.IsRunningUp());
        anim.SetBool(is_run_down, Player.Instance.IsRunningDown());
        anim.SetBool(is_run_left_right, Player.Instance.IsRunningLeftRight());
        ReversePlayer();
        anim.SetBool(is_attack_up, Gun.Instance.IsAttackingUp());
        anim.SetBool(is_attack_down, Gun.Instance.IsAttackingDown());
        anim.SetBool(is_attack_left, Gun.Instance.IsAttackingLeft());
        anim.SetBool(is_attack_right, Gun.Instance.IsAttackingRight());
    }
    private void ReversePlayer()
    {
        if (Player.Instance.Rev() && Player.Instance.IsRunningLeftRight())
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
