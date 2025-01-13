using UnityEngine;

public class EnemyLaunchedState : EnemyBaseState
{
    private readonly int LaunchedHash = Animator.StringToHash("Launched");
    private const float CrossFadeDuration = 0.1f;
    private float launchForce = 20f;
    public EnemyLaunchedState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("Enetered Launched State");
        //stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(LaunchedHash, CrossFadeDuration);
        Vector3 direction = Vector3.up.normalized;
        Debug.Log($"Launch State LaunchForce: {launchForce}");
        stateMachine.ForceReceiver.Reset();
        stateMachine.ForceReceiver.SetGravity(2);
        stateMachine.ForceReceiver.Jump(launchForce);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (GetNormalizedTime(stateMachine.Animator, "Hurt") > .55f 
            && stateMachine.CharacterController.isGrounded == false)
        {
          stateMachine.Animator.speed = 0f;
            
        }

        if(stateMachine.CharacterController.isGrounded == false && stateMachine.Animator.speed < 1f)
        {
            stateMachine.Animator.speed = 1f;
            stateMachine.ForceReceiver.SetGravity(0);
        }
    }


    public override void Exit()
    {

    }


}
