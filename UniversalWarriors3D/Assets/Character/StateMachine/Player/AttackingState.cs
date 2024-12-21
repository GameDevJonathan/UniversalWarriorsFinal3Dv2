using System.Linq;
using UnityEngine;

public class AttackingState : PlayerBaseState
{
    Attacks attack;
    float previousFrameTime;
    private bool targetLock;
    private int attackIndex;
    private bool alreadyAppliedForce;
    private bool grounded => stateMachine.WallRun.CheckForGround();
    private enum SpecialAttacks { Uppercut = 5, SpinningKick = 6 };

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

        if (attack.AnimationName == "Uppercut")
        {
            stateMachine.Animator.applyRootMotion = false;

        }
        else
            stateMachine.Animator.applyRootMotion = true;

        setIndex();


    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        if (attack.ShouldAddAttackForce)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Attack") >= attack.TimeForce)
            {
                Debug.Log("adding Force");
                TryApplyForce(stateMachine.transform.up, stateMachine.transform.forward, attack.UpForce, attack.ForwardForce);
            }
        }



        //if (attack.AnimationName == "Uppercut")
        //{
        //    if (GetNormalizedTime(stateMachine.Animator, "UpperCut") >= attack.TimeForce && attack.ShouldAddAttackForce)
        //    {
        //        Debug.Log("adding Force");
        //        TryApplyForce(stateMachine.transform.up, stateMachine.transform.forward, attack.UpForce, attack.ForwardForce);
        //    }
        //}

        //if (attack.AnimationName == "SpinningKick")
        //{
        //    if (GetNormalizedTime(stateMachine.Animator, "Attack") >= attack.TimeForce && attack.ShouldAddAttackForce)
        //    {
        //        Debug.Log("adding Force");
        //        TryApplyForce(stateMachine.transform.up, stateMachine.transform.forward, attack.UpForce, attack.ForwardForce);
        //    }
        //}


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


        if (GetNormalizedTime(stateMachine.Animator, "UpperCut") > 1f)
        {
            Debug.Log($"Grounded Return Value {grounded}");

            if (!grounded)
            {
                stateMachine.SwitchState(new PlayerFallState(stateMachine));
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
                //stateMachine.ForceReceiver.Jump(20f);
                stateMachine.SwitchState(new AttackingState(stateMachine, (int)specialAttacks, targetLock));
                break;

            case 2:
                specialAttacks = SpecialAttacks.SpinningKick;
                //stateMachine.ForceReceiver.Jump(20f);
                stateMachine.SwitchState(new AttackingState(stateMachine, (int)specialAttacks, targetLock));
                break;

        }


    }


    /// <summary>
    /// transformDirection for which Direction I want to apply force
    /// </summary>


    private void TryApplyForce(Vector3 UpwardDirection, Vector3 ForwardDirection, float UpForce = 1, float ForwardForce = 1)
    {
        if (alreadyAppliedForce) return;
        stateMachine.ForceReceiver.AddForce((UpwardDirection * UpForce) + (ForwardDirection * ForwardForce));
        alreadyAppliedForce = true;

    }

    private void setIndex()
    {
        //Debug.Log(attackIndex);
        stateMachine.Index = attackIndex;
    }

    private void AttackWindow(Attacks attack)
    {
        Debug.Log("Attack Window: Min[0] Max[1]");
        attack.ImpactWindow


    }





}