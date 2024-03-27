using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class AttackingState : PlayerBaseState
{
    Attacks attack;
    float previousFrameTime;
    private bool targetLock;
    private int attackIndex;
    private enum SpecialAttacks { Uppercut = 4};
    SpecialAttacks specialAttacks;

    public AttackingState(PlayerStateMachine stateMachine, int AttackIndex, bool targetLock = false) : base(stateMachine)
    {
        attack = stateMachine.Attacks[AttackIndex];
        stateMachine.InputReader.FightingStance = true;
        this.targetLock = targetLock;
        this.attackIndex = AttackIndex;


    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.Animator.applyRootMotion = true;

        setIndex();


    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();



        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime > previousFrameTime && normalizedTime < 1f)
        {
            if (stateMachine.InputReader.AttackButtonHeld)
            {
                Debug.Log("ButtonHeld");
                TryComboEnder(normalizedTime);
            }

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
                    stateMachine.SwitchState(new PlayerTargetingState(stateMachine, true));
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

        stateMachine.Targeter.SelectClosestTarget();

        stateMachine.SwitchState(new AttackingState(stateMachine, attack.ComboStateIndex, targetLock));
    }


    private void TryComboEnder(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.Targeter.SelectClosestTarget();

        switch (attack.ComboStateIndex)
        {
            case 1:
                specialAttacks = SpecialAttacks.Uppercut;
                stateMachine.SwitchState(new AttackingState(stateMachine,(int)specialAttacks, targetLock));
                break;

        }

        
    }

    private void setIndex()
    {
        //Debug.Log(attackIndex);
        stateMachine.Index = attackIndex;
    }





}
