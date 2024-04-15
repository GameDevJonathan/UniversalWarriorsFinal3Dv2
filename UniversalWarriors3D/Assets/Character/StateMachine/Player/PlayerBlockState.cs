using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Blocking");
    private const float CrossFadeDuration = 0.2f;
    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);

    }

    public override void Enter()
    {
        Debug.Log("Entered Block State");
        
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.BlockButtonReleased)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
        }
        
    }
    public override void Exit()
    {
        Debug.Log("Leaving Block State");
    }
}
