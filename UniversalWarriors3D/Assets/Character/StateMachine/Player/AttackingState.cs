using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : PlayerBaseState
{
    Attacks attack;
    float previousFrameTime;
    private bool targetLock;

    public AttackingState(PlayerStateMachine stateMachine, int AttackIndex, bool targetLock = false) : base(stateMachine)
    {
        attack = stateMachine.Attacks[AttackIndex];
        stateMachine.InputReader.FightingStance = true;
        this.targetLock = targetLock;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.Animator.applyRootMotion = true;


    }
    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime > previousFrameTime && normalizedTime < 1f)
        {
            if (stateMachine.InputReader.AttackButtonPressed)
            {
                Debug.Log("Pressed attack button");
                TryComboAttack(normalizedTime);
            }

        }
        else
        {
            if (normalizedTime > 1f)
            {
                stateMachine.Animator.SetFloat("isEquiped",
                    (stateMachine.InputReader.FightingStance) ? 1 : 0);

                if (targetLock)
                    stateMachine.SwitchState(new PlayerTargetingState(stateMachine,true));
                else
                    stateMachine.SwitchState(new Grounded(stateMachine, true));
                return;
            }
        }


        previousFrameTime = normalizedTime;

    }



    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
    }



    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new AttackingState(stateMachine, attack.ComboStateIndex,targetLock));
    }





}
