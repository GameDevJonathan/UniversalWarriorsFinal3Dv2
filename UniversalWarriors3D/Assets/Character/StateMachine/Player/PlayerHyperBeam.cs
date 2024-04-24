using UnityEngine;

public class PlayerHyperBeam : PlayerBaseState
{
    private readonly int HyperBeamHash = Animator.StringToHash("HyperBeamStart");
    private const float CrossFadeDuration = 0.05f;
    private GameObject[] SpecialBeam;
    private bool targetLock;

    public PlayerHyperBeam(PlayerStateMachine stateMachine, bool targetLock = false) : base(stateMachine)
    {
        this.SpecialBeam = stateMachine.SpecialBeam;
        this.targetLock = targetLock;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(HyperBeamHash, CrossFadeDuration);
        foreach (var part in SpecialBeam)
        {
            if (part.activeSelf == false)
            {
                part.SetActive(true);
            }
        }

    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "HyperBeam") > 1f && SpecialBeam[0].activeSelf == false)
        {
            stateMachine.Animator.SetTrigger("End");
        }

        if (GetNormalizedTime(stateMachine.Animator, "HyperBeamEnd") > 1f)
        {
            if (!targetLock)
                stateMachine.SwitchState(new Grounded(stateMachine, true));
            else
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine, targetLock));
        }
    }



    public override void Exit()
    {

    }
}
