using UnityEngine;


///In this state Enemies just stand around doing nothing. 
///Idly looking around and playing a random animation after a while. 
///If in this state or in the patrol state if the enemy will enter the chase 
///state. 

public class EnemyIdleState : EnemyBaseState
{
    private readonly int FreeLookHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float crossFadeTime = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float RandomIdleTimer;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }


    public override void Enter()
    {
        stateMachine.Agent.updatePosition = false;
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookHash, crossFadeTime);

        RandomIdleTimer = Random.Range(stateMachine.IdleRangeTimer.x, stateMachine.IdleRangeTimer.y);
        stateMachine.CanHit = true;

        stateMachine.CurrentState();


    }
    public override void Tick(float deltaTime)
    {

        Move(deltaTime);

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }
        Debug.Log(stateMachine.PlayerDetector.alertStage);

        if (stateMachine.PlayerDetector.alertStage == AlertStage.Alerted)
        {
            if (stateMachine.Dummy) return;

            //Transition to chasing state
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        //if (IsInChaseRange())
        //{
        //    if (stateMachine.Dummy) return;
        //    Debug.Log("Entering Chasing State");
        //    //Transition to chasing state
        //    stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        //    return;
        //}



        if (RandomIdleTimer > 0)
        {
            //RandomIdleTimer = Mathf.Max(RandomIdleTimer - Time.deltaTime, 0);
        }
        else
        {
            //ToDo Trigger Random Idle Animation
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);

    }

    public override void Exit() { }

}