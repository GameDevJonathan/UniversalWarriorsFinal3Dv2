using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    Attacks attack;
    WeaponDamage weapon;
    public Collider[] hitBoxes;

    public EnemyAttackingState(EnemyStateMachine stateMachine, int attackIndex = 0) : base(stateMachine)
    {
        this.attack = stateMachine.Attacks[attackIndex];
        weapon = stateMachine.Weapon[(int)attack.hitBox];

        if (stateMachine.hitBoxes != null)
            this.hitBoxes = stateMachine.hitBoxes;

        stateMachine.CurrentAttackIndex = attackIndex;

    }

    public override void Enter()
    {
        Debug.Log("Entering Attacking State");
        if (stateMachine.transform.CompareTag("Boss"))
            stateMachine.Animator.applyRootMotion = true;
        weapon.SetAttack(attack.AttackForce);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (attack.AnimationName == "GroundSmash")
        {
            if (normalizedTime < attack.ImpactWindow[0].x)
            {
                FacePlayer();
                MoveToPlayer(deltaTime);

            }

        }


        ActivateHitBox(normalizedTime);

        if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1)
        {
            if (stateMachine.Dummy == true)
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            else
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
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
            hitBoxes[(int)Attacks.HitBox.Right]?.gameObject.SetActive(false);
            hitBoxes[(int)Attacks.HitBox.Left]?.gameObject.SetActive(false);
            //hitBoxes[(int)Attacks.HitBox.LeftFoot]?.gameObject.SetActive(false);
            //hitBoxes[(int)Attacks.HitBox.RightFoot]?.gameObject.SetActive(false);
        }

    }




    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
        Debug.Log("Leaving Attacking State");
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;


    }

    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.CharacterController.velocity;
    }


}
