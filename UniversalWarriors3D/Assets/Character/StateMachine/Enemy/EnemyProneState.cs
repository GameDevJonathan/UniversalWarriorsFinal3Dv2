using UnityEngine;

public class EnemyProneState : EnemyBaseState
{
    private float upTime = 0f;
    private bool getUp = false;
    private readonly int StandUpHash = Animator.StringToHash("StandUp");
    private readonly int StandUpProneHash = Animator.StringToHash("StandUpProne");
    private const float CrossFadeDuration = 0.1f;
    private bool faceDown = false;
    public EnemyProneState(EnemyStateMachine stateMachine, bool faceDown = false) : base(stateMachine)
    {
        this.faceDown = faceDown;
    }

    public override void Enter()
    {
        //stateMachine.Agent.updatePosition = true;
        Debug.Log("Entered Prone State");
        upTime = Random.Range(stateMachine.UpTime.x, stateMachine.UpTime.y);
        stateMachine.CanHit = false;

    }
    public override void Tick(float deltaTime)
    {

        if (upTime > 0)
            upTime = Mathf.Max(upTime -= Time.deltaTime, 0);

        if (upTime <= 0 && !getUp)
        {
            getUp = !getUp;
            stateMachine.Animator.CrossFadeInFixedTime((!faceDown) ? StandUpHash : StandUpProneHash, CrossFadeDuration);
        }

        if (GetNormalizedTime(stateMachine.Animator, "GetUp") > 1f)
        {
            switch (stateMachine.Health.isStunned)
            {
                case true:
                    stateMachine.SwitchState(new EnemyStunState(stateMachine));
                    break;
                case false:
                    stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                    break;
            }
        }


    }

    public override void Exit()
    {


    }

}
