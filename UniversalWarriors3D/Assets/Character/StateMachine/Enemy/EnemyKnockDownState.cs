using UnityEngine;

public class EnemyKnockDownState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("KnockedDown");
    private const float CrossFadeDuration = 0.1f;
    private float upTime = 0f;
    private float launchForce;
    private Vector3 direction;

    public EnemyKnockDownState(EnemyStateMachine stateMachine, float launchForce) : base(stateMachine)
    {
        this.launchForce = launchForce;
    }

    public override void Enter()
    {
        Debug.Log("Entered KnockDown State");
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        stateMachine.Animator.applyRootMotion = true;
        direction = Vector3.up.normalized * launchForce;

        stateMachine.ForceReceiver.AddForce(direction);

        //stateMachine.Ragdoll.ToggleRagdoll(true);
        //upTime = Random.Range(stateMachine.UpTime.x, stateMachine.UpTime.y);

    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (GetNormalizedTime(stateMachine.Animator, "Hurt") > 1f)
        {
            stateMachine.SwitchState(new EnemyProneState(stateMachine));
            return;
        }






    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
        //stateMachine.Ragdoll.ToggleRagdoll(false);
    }

}
