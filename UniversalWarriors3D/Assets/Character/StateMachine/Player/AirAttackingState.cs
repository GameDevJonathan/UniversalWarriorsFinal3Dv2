using UnityEngine;

public class AirAttackingState : PlayerBaseState
{
    AirAttacks AirAttacks;
    WeaponDamage weapon;
    float previousFrameTime;
    private bool targetLock;
    private int attackIndex;
    private bool alreadyAppliedForce;

    public Collider[] hitBoxes;

    public AirAttackingState(PlayerStateMachine stateMachine, int AttackIndex, bool targetLock = false) : base(stateMachine)
    {
        AirAttacks = stateMachine.AirAttacks[AttackIndex];
        weapon = stateMachine.weapon[((int)AirAttacks.hitBox)];
        stateMachine.InputReader.FightingStance = true;
        this.targetLock = targetLock;
        this.attackIndex = AttackIndex;

        if (stateMachine.hitBoxes != null)
            this.hitBoxes = stateMachine.hitBoxes;
    }

    public override void Enter()
    {
        //Debug.Log("WEapon: " + weapon.gameObject.name);
        weapon.SetAttack(AirAttacks.AttackForce);
        weapon.SetAttackType(AirAttacks.LauncherAttack);
        weapon.SetLaunchForce(AirAttacks.LaunchForce);
        stateMachine.Animator.CrossFadeInFixedTime(AirAttacks.AnimationName, AirAttacks.TransitionDuration);
        stateMachine.ForceReceiver.Reset();
        stateMachine.ForceReceiver.useGravity = false;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        if (AirAttacks.ShouldAddAttackForce)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Attack") >= AirAttacks.TimeForce)
            {
                //Debug.Log("adding Force");
                TryApplyForce(stateMachine.transform.up, stateMachine.transform.forward, AirAttacks.UpForce, AirAttacks.ForwardForce);
            }
        }

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        ActivateHitBox(normalizedTime);

        if (normalizedTime > previousFrameTime && normalizedTime < 1f)
        {
            //if (stateMachine.InputReader.AttackButtonHeld)
            //{
            //    Debug.Log("ButtonHeld");
            //    TryComboEnder(normalizedTime);
            //}


            if (stateMachine.InputReader.AttackButtonPressed)
            {
                //Debug.Log("Pressed attack button");
                TryComboAttack(normalizedTime);
            }

        }
        else
        {
            if (normalizedTime > 1f)
            {
                stateMachine.Animator.SetFloat("isEquiped",
                    (stateMachine.InputReader.FightingStance) ? 1 : 0);

                //if (targetLock)
                //    stateMachine.SwitchState(new PlayerTargetingState(stateMachine, true));
                //else
                    stateMachine.SwitchState(new PlayerFallState(stateMachine));
                return;
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.ForceReceiver.Reset();
        stateMachine.ForceReceiver.useGravity = true;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (AirAttacks.ComboStateIndex == -1) { return; }

        if (normalizedTime < AirAttacks.ComboAttackTime) { return; }

        stateMachine.Targeter.SelectClosestTarget();

        stateMachine.SwitchState(new AirAttackingState(stateMachine, AirAttacks.ComboStateIndex, targetLock));
    }

    private void TryApplyForce(Vector3 UpwardDirection, Vector3 ForwardDirection, float UpForce = 1, float ForwardForce = 1)
    {
        if (alreadyAppliedForce) return;
        stateMachine.ForceReceiver.AddForce((UpwardDirection * UpForce) + (ForwardDirection * ForwardForce));
        alreadyAppliedForce = true;

    }

    private void ActivateHitBox(float impactTime)
    {
        //Debug.Log(attack.ImpactWindow[0]);
        if (AirAttacks.ImpactWindow.Length < 1) return;


        if (impactTime > AirAttacks.ImpactWindow[0].x && impactTime < AirAttacks.ImpactWindow[0].y)
        {
            switch (AirAttacks.hitBox)
            {
                case AirAttacks.HitBox.Right:
                    hitBoxes[(int)AirAttacks.hitBox].gameObject.SetActive(true);
                    break;
                case AirAttacks.HitBox.Left:
                    hitBoxes[(int)AirAttacks.hitBox].gameObject.SetActive(true);
                    break;
                case AirAttacks.HitBox.RightFoot:
                    hitBoxes[(int)AirAttacks.hitBox].gameObject.SetActive(true);
                    break;
                case AirAttacks.HitBox.LeftFoot:
                    hitBoxes[(int)AirAttacks.hitBox].gameObject.SetActive(true);
                    break;
            }
        }
        else
        {
            hitBoxes[(int)AirAttacks.HitBox.RightFoot].gameObject.SetActive(false);
            hitBoxes[(int)AirAttacks.HitBox.LeftFoot].gameObject.SetActive(false);
            hitBoxes[(int)AirAttacks.HitBox.Right].gameObject.SetActive(false);
            hitBoxes[(int)AirAttacks.HitBox.Left].gameObject.SetActive(false);
        }


    }
}
