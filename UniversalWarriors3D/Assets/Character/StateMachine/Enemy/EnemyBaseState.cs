using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation =
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(lookPos),
            Time.deltaTime * stateMachine.RotationSmoothValue);

    }

    protected bool IsInChaseRange()
    {
        float toPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return toPlayer <= stateMachine.PlayerDectionRange * stateMachine.PlayerDectionRange;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharacterController.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

}
