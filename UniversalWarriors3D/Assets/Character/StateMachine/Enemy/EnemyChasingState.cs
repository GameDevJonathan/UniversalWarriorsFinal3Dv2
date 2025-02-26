using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int FreeLookHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float crossFadeTime = 0.1f;
    private const float AnimatorDampTime = 0.1f;


    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Chasing State");

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookHash, crossFadeTime);

    }
    public override void Tick(float deltaTime)
    {

        if ((!stateMachine.PlayerDetector.playerInFov && !stateMachine.PlayerDetector.playerInEngageRange))
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else
        if (stateMachine.PlayerDetector.playerInEngageRange)
        {
            stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);
            stateMachine.SwitchState(new EnemyEngageState(stateMachine));
            return;
        }
        FacePlayer();
        MoveToPlayer(deltaTime);
        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);

    }
    public override void Exit()
    {
        Debug.Log("Leaving Chasing State");

        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.CharacterController.velocity;
    }

    private bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

}



