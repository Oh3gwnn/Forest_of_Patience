using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    TPSCAM TPSCAM;

    Animator animator;
    string animationState = "Anistate";
    string CrouchState = "Crouch";
    string RunState = "Run";
    string FallState = "Fall";

    enum States
    {
        idle = 0,
        up = 1,
        down = 2,
        left = 3,
        right = 4,
        Jump = 5,
        Pause = 6
    }

    enum CrouchStates
    {
        idle = 0,
        up = 1,
        down = 2,
        left = 3,
        right = 4,
        Pause = 5
    }

    enum RunStates
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4,
        Pause = 5
    }

    enum FallStates
    {
        falling = 1,
        fallingdown = 2,
        Pause = 3
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 55, 0);
        if (TPSCAM.IsPause == false)
        {
            UpdateState();
            UpdateCrouchState();
            UpdateRunState();
            UpdateFallState();

            if (TPSCAM.IsJumping == true)
            {
                if (TPSCAM.IsFalling == false)
                {
                    animator.SetInteger(animationState, (int)States.Pause);
                    animator.SetInteger(CrouchState, (int)CrouchStates.Pause);
                    animator.SetInteger(RunState, (int)RunStates.Pause);
                    animator.SetInteger(animationState, (int)States.Jump);
                }
                else
                {
                    animator.SetInteger(animationState, (int)States.Pause);
                    animator.SetInteger(CrouchState, (int)CrouchStates.Pause);
                    animator.SetInteger(RunState, (int)RunStates.Pause);
                }
            }
        }
        else
        {
            UpdateFallState();
        }
    }

    void UpdateState()
    {
        if (!TPSCAM.IsCrouch == !TPSCAM.IsRun)
        {
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger(animationState, (int)States.right);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetInteger(animationState, (int)States.left);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetInteger(animationState, (int)States.up);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetInteger(animationState, (int)States.down);
            }
            else
            {
                animator.SetInteger(animationState, (int)States.idle);
            }
        }
        else
        {
            animator.SetInteger(animationState, (int)States.Pause);
        }
    }

    void UpdateCrouchState()
    {
        if (TPSCAM.IsCrouch)
        {
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger(CrouchState, (int)CrouchStates.right);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetInteger(CrouchState, (int)CrouchStates.left);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetInteger(CrouchState, (int)CrouchStates.up);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetInteger(CrouchState, (int)CrouchStates.down);
            }
            else
            {
                animator.SetInteger(CrouchState, (int)CrouchStates.idle);
            }
        }
        else
        {
            animator.SetInteger(CrouchState, (int)CrouchStates.Pause);
        }
    }

    void UpdateRunState()
    {
        if (TPSCAM.IsRun)
        {
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger(RunState, (int)RunStates.right);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetInteger(RunState, (int)RunStates.left);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, -30, 0);
                animator.SetInteger(RunState, (int)RunStates.up);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetInteger(RunState, (int)RunStates.down);
            }
        }
        else
        {
            animator.SetInteger(RunState, (int)RunStates.Pause);
        }
    }

    void UpdateFallState()
    {
        if (TPSCAM.IsFalling == true)
        {
            animator.SetInteger(FallState, (int)FallStates.falling);
        }
        else if (TPSCAM.IsPause == true)
        {
            animator.SetInteger(FallState, (int)FallStates.fallingdown);
        }
        else
        {
            animator.SetInteger(FallState, (int)FallStates.Pause);
        }
    }
}
