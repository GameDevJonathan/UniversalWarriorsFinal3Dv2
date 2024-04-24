using EasyAudioManager;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingHash = Animator.StringToHash("TargetingBlend");
    private const float CrossFadeDuration = 0.1f;
    private LayerMask LockOnTargetMask;
    private RaycastHit LockOnTargetHit;
    private Transform debugTransform;
    Vector2 delta;
    private float angle;
    private bool dodging = false;
    public float AnimatorDampTime = 0.05f;

    //shot values
    private float _lastFireTime = -1f;
    private float _coolDownTime = .1f;


    public PlayerTargetingState(PlayerStateMachine stateMachine, bool dodging = false) : base(stateMachine)
    {
        this.LockOnTargetMask = stateMachine.lockOnTargetColliderMask;
        debugTransform = stateMachine.debugTransform;
        this.dodging = dodging;
    }



    public override void Enter()
    {

        stateMachine.Animator.CrossFadeInFixedTime(TargetingHash, CrossFadeDuration);
        stateMachine._TargetCamUtil.SetActive(true);
        //Debug.Log("PlayerTargeting State:: Entered Targeting State");        
        //stateMachine.rig.weight = .6f;
        stateMachine.InputReader.CancelEvent += OnCancel;
        stateMachine.InputReader.SpecialBeamEvent += InputReader_SpecialBeamEvent;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.MeleeEvent += OnMelee;
    }


    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new Grounded(stateMachine));
            return;
        }

        stateMachine.Targeter.CycleTarget();

        Vector3 movement = TargetedMovement().normalized;

        Move(movement * stateMachine.LockOnMovementSpeed, deltaTime);


        delta = Vector2.zero - stateMachine.InputReader.MovementValue;
        delta.Normalize();
        if (delta != Vector2.zero)
        {
            angle = Mathf.Atan2(delta.y, -delta.x) / Mathf.PI;
            angle *= 180;
            angle += 90f;
            if (angle < 0)
            {
                angle += 360;
            }
        }
        else
        {
            angle = 0f;
        }

        //Debug.Log($"JoystickAngle: {angle}");

        FaceTarget();

        AnimatorValues(deltaTime);

        if (stateMachine.Targeter.CurrentTarget != null)
        {
            RayCastDebug();
            if (stateMachine.Targeter.CurrentTarget.type == Target.Type.large)
                stateMachine._TargetCamUtil.gameObject.transform.localScale = Vector3.one * 2.5f;
            else
                stateMachine._TargetCamUtil.gameObject.transform.localScale = Vector3.one;

        }


    }

    public override void Exit()
    {
        stateMachine.Animator.SetFloat("StrafeMovement", 0);
        stateMachine.InputReader.Targeting = (dodging) ? true : false;

        stateMachine._TargetCamUtil.SetActive(dodging);
        debugTransform.gameObject.SetActive(dodging);
        //stateMachine.InputReader.ResetCamera();
        //stateMachine.rig.weight = 0f;
        stateMachine.InputReader.CancelEvent -= OnCancel;
        stateMachine.InputReader.SpecialBeamEvent -= InputReader_SpecialBeamEvent;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.MeleeEvent -= OnMelee;
    }

    private void AnimatorValues(float deltaTime)
    {

        #region unused code
        //float forwardSpeed;
        //float strafeSpeed;

        //if(Mathf.Abs(stateMachine.InputReader.MovementValue.y) >= .05f)
        //{
        //    forwardSpeed = stateMachine.InputReader.MovementValue.normalized.y;
        //    stateMachine.Animator.SetFloat("ForwardSpeed", forwardSpeed);
        //}
        //else
        //{
        //    stateMachine.Animator.SetFloat("ForwardSpeed", 0);
        //}

        //if (Mathf.Abs(stateMachine.InputReader.MovementValue.x) >= .05f)
        //{
        //    strafeSpeed = stateMachine.InputReader.MovementValue.normalized.x;
        //    stateMachine.Animator.SetFloat("StrafingSpeed", strafeSpeed);
        //}
        //else
        //{
        //    stateMachine.Animator.SetFloat("StrafingSpeed", 0);
        //}
        #endregion


        stateMachine.Animator.SetFloat("ForwardSpeed", stateMachine.InputReader.MovementValue.normalized.y, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat("StrafingSpeed", stateMachine.InputReader.MovementValue.normalized.x, AnimatorDampTime, deltaTime);

    }





    #region Input events
    public void OnCancel()
    {
        debugTransform.gameObject.SetActive(false);
        stateMachine.Targeter.Cancel();
        //stateMachine.InputReader.ResetCamera();
        dodging = false;
        stateMachine.SwitchState(new Grounded(stateMachine, true));
    }

    public void OnMelee()
    {
        dodging = true;
        stateMachine.SwitchState(new AttackingState(stateMachine, 0, dodging));
        return;
    }

    #endregion

    #region RayCast and Movement
    private Vector3 TargetedMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void RayCastDebug()
    {
        Vector3 distance = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.FirePoint.position;
        //Vector3 distance = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;

        float dist = Vector3.Distance(stateMachine.Targeter.CurrentTarget.transform.position, stateMachine.FirePoint.transform.position);


        Vector3 faceTarget = distance;

        stateMachine.FirePoint.rotation =
            Quaternion.LookRotation(faceTarget);

        Debug.DrawRay(stateMachine.FirePoint.transform.position,
            stateMachine.FirePoint.transform.forward * dist, Color.yellow);
        //Debug.DrawRay(stateMachine.transform.position,
        //    stateMachine.transform.forward * dist, Color.yellow);

        distance = distance.normalized;

        if (Physics.Raycast(stateMachine.FirePoint.transform.position, distance, out LockOnTargetHit,
            dist, LockOnTargetMask))
        {
            //Debug.Log("Hit");
            stateMachine._TargetCamUtil.transform.position = LockOnTargetHit.point;
            stateMachine._TargetCamUtil.transform.LookAt(Camera.main.transform);
            debugTransform.gameObject.SetActive(true);
            debugTransform.position = LockOnTargetHit.point;
        }
    }
    #endregion
    private void InputReader_SpecialBeamEvent()
    {
        dodging = true;
        stateMachine.SwitchState(new PlayerHyperBeam(stateMachine, dodging));
    }

    public void OnDodge()
    {
        Debug.Log($"Joystick angle: {angle}");
        Vector2 move = stateMachine.InputReader.MovementValue;
        Debug.Log($"Dodging: {dodging}");
        dodging = true;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, move, angle, dodging));
        return;
    }

}
