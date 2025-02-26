using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private float angle;
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private readonly int DodgeLeftHash = Animator.StringToHash("DodgeLeft");
    private readonly int DodgeBackHash = Animator.StringToHash("DodgeBack");
    private const float CrossFadeDuration = 0.1f;
    bool stillTargeting;

    public PlayerDodgingState(PlayerStateMachine stateMachine, float angle, bool stillTargeting = false) : base(stateMachine)
    {
        this.angle = angle;

        this.stillTargeting = stillTargeting;
    }

    public override void Enter()
    {
        Debug.Log(angle);

        if (angle == 0)
        {
            setAnimProperties(3);
            return;
        }




        /*if ((angle <= 45 && angle >= 0) || (angle <= 360 && angle >= 315))*/ // right quadrent
        if ((angle <= 45 || angle >= 315)) // up quardrent
        {
            setAnimProperties(1);//dodging forward
        }
        else

        if (angle > 45 && angle < 135) // right quadrent
        {
            setAnimProperties(2);// dodge right
        }
        else

        if (angle > 135 && angle < 225) //down quadrent
        {
            setAnimProperties(3);// dodge back
        }
        else

        if (angle == 0 || (angle > 225 && angle < 315)) // left quadrent
        {
            setAnimProperties(4);// dodge left
        }

        //if (dodgingInput.y < -.3f && (dodgingInput.x > -.4f && dodgingInput.x < .4f))
        //{
        //    setAnimProperties(0, -1);//dodging backwards
        //}

        //if ((dodgingInput.y > .5 && dodgingInput.x > .5) || (dodgingInput.y > .5 && dodgingInput.x < -.5))
        //    setAnimProperties(0, 1);

        //if (dodgingInput.x > .3f && (dodgingInput.y > -.5f && dodgingInput.y < .5f))
        //{
        //    setAnimProperties(1, 0); //dodging right
        //}

        //if (dodgingInput.x < -.3f && (dodgingInput.y > -.5f && dodgingInput.y < .5f))
        //{
        //    setAnimProperties(-1, 0); //dodging left
        //}


    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Dodge") > 1)
        {
            if (stateMachine.InputReader.Targeting)
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine, stillTargeting));
            else
                stateMachine.SwitchState(new Grounded(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
    }

    /// <summary>
    /// Set animation Roll 1: Forward |  
    ///  2: Right |
    ///  3: Back |
    ///  4: Left
    /// </summary>
    /// 
    /// 
    private void setAnimProperties(int direction)
    {
        switch (direction)
        {
            case 1: //forard
                stateMachine.Animator.CrossFadeInFixedTime(DodgeForwardHash, CrossFadeDuration);
                break;
            case 2: //Right
                stateMachine.Animator.CrossFadeInFixedTime(DodgeRightHash, CrossFadeDuration);
                break;
            case 3: //Down
                stateMachine.Animator.CrossFadeInFixedTime(DodgeBackHash, CrossFadeDuration);
                break;
            case 4: //Left
                stateMachine.Animator.CrossFadeInFixedTime(DodgeLeftHash, CrossFadeDuration);
                break;
        }
        stateMachine.Animator.applyRootMotion = true;
    }
}
