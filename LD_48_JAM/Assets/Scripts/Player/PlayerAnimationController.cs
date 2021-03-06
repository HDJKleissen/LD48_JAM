using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public SpriteMask spriteMask;

    public RuntimeAnimatorController[] animatorControllers = new RuntimeAnimatorController[Enum.GetNames(typeof(PlayerDisguise)).Length];

    public void ChangeDisguise(PlayerDisguise disguise)
    {
        animator.runtimeAnimatorController = animatorControllers[(int)disguise];
    }

    public void UpdateAnimator(Vector3 movement)
    {
        int horizontalMovement = Math.Sign(movement.x);
        int verticalMovement = Math.Sign(movement.y);

        animator.SetInteger("HorizontalMovement", horizontalMovement);
        animator.SetInteger("VerticalMovement", verticalMovement);

        if (horizontalMovement > 0)
        {
            transform.parent.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalMovement < 0)
        {
            transform.parent.localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsAnimationFinished(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
