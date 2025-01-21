using UnityEngine;

public class AttackingState : PlayerBaseState
{
    Attacks attack;
    WeaponDamage weapon;
    float previousFrameTime;
    private bool targetLock;
    private int attackIndex;
    private bool alreadyAppliedForce;
    private bool grounded => stateMachine.WallRun.CheckForGround();
    private enum SpecialAttacks { Uppercut = 5, SpinningKick = 6, Launcher = 7 };

    SpecialAttacks specialAttacks;

    public Collider[] hitBoxes;

    public AttackingState(PlayerStateMachine stateMachine, int AttackIndex, bool targetLock = false) : base(stateMachine)
    {
        attack = stateMachine.Attacks[AttackIndex];
        weapon = stateMachine.weapon[(int)attack.hitBox];
        stateMachine.InputReader.FightingStance = true;
        this.targetLock = targetLock;
        this.attackIndex = AttackIndex;

        if (stateMachine.hitBoxes != null)
            this.hitBoxes = stateMachine.hitBoxes;


    }

    public override void Enter()
    {
        //Debug.Log($"HitBox: {(int)attack.hitBox} ");
        weapon.SetAttack(attack.AttackForce);
        weapon.SetKnockDown(attack.KnockDown);
        weapon.SetAttackType(attack.LauncherAttack);
        weapon.SetLaunchForce(attack.LaunchForce);
        weapon.setStunForce(attack.stunForce);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        if (attack.AnimationName == "Uppercut" )
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

        //Debug.Log(attack.MultiHitIndex);

        if (attack.ShouldAddAttackForce)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Attack") >= attack.TimeForce)
            {
                //Debug.Log("adding Force");
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


        


        ActivateHitBox(normalizedTime);
        

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

            if(stateMachine.InputReader.HeavyAttackButtonPressed)
            {
                TryLauncher(normalizedTime);
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

    private void TryLauncher(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }
        if(normalizedTime < attack.ComboAttackTime) {return; }
        stateMachine.Targeter.SelectClosestTarget();        
        stateMachine.SwitchState(new AttackingState(stateMachine, 7, targetLock));
        
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
        Debug.Log("Attack Window: " + attack.ImpactWindow[attack.MultiHitIndex].x + " " + attack.ImpactWindow[attack.MultiHitIndex].y);

    }

    private void ActivateHitBox(float impactTime)
    {
        //Debug.Log(attack.ImpactWindow[0]);
        if (attack.ImpactWindow.Length < 1) return;


        if (impactTime > attack.ImpactWindow[attack.MultiHitIndex].x && impactTime < attack.ImpactWindow[attack.MultiHitIndex].y)
        {
            switch (attack.hitBox)
            {
                case Attacks.HitBox.Right:
                    hitBoxes[(int)attack.hitBox].gameObject.SetActive(true);
                    break;
                case Attacks.HitBox.Left:
                    hitBoxes[(int)attack.hitBox].gameObject.SetActive(true);
                    break;
                case Attacks.HitBox.RightFoot:
                    hitBoxes[(int)attack.hitBox].gameObject.SetActive(true);
                    break;
                case Attacks.HitBox.LeftFoot:
                    hitBoxes[(int)attack.hitBox].gameObject.SetActive(true);
                    break;
            }
        }
        else
        {


            hitBoxes[(int)Attacks.HitBox.RightFoot].gameObject.SetActive(false);
            hitBoxes[(int)Attacks.HitBox.LeftFoot].gameObject.SetActive(false);
            hitBoxes[(int)Attacks.HitBox.Right].gameObject.SetActive(false);
            hitBoxes[(int)Attacks.HitBox.Left].gameObject.SetActive(false);
        }


    }

    private void setHitBox()
    {
        if(attack.MultiHitCombo == false) { return; }
        if(attack.AnimationName == "Attack1")
        {
            switch (attack.MultiHitIndex)
            {
                case 1:
                    attack.hitBox = Attacks.HitBox.Right;
                    break;
            }
        }
    }

    



}