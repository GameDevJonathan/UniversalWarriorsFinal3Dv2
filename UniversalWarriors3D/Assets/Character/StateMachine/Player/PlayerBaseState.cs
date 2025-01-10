using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

   

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharacterController.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceMovement(Vector3 movement, float deltatime)
    {
        stateMachine.transform.rotation =
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltatime * stateMachine.RotationSmoothValue);
    }

    protected void FaceTarget()
    {
        if(stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = 
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(lookPos),
            Time.deltaTime * stateMachine.RotationSmoothValue);

    }


    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new Grounded(stateMachine, true));
        if (stateMachine.Animator.applyRootMotion)
        {
            stateMachine.Animator.applyRootMotion = false;
        }        
    }
}
