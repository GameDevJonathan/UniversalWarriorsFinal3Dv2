using AmplifyShaderEditor;
using UnityEngine;


public class Grounded : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("Movement");
    private readonly int EquipHash = Animator.StringToHash("Equip");
    private readonly int UnEquipHash = Animator.StringToHash("UnEquip");
    public float AnimatorDampTime = 0.05f;
    private float freeLookValue;
    private float freeLookMoveSpeed;
    private bool shouldFade;
    private const float CrossFadeDuration = 0.1f;
    private bool grounded => stateMachine.WallRun.CheckForGround();
    


    public Grounded(PlayerStateMachine stateMachine, bool shouldFade = false) : base(stateMachine)
    {
        this.freeLookMoveSpeed = stateMachine.FreeLookMovementSpeed;
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        
        if (stateMachine.rig != null)
        {
            stateMachine.rig.weight = 0f;
        }
       

        if (!shouldFade)
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        else
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DashEvent += OnDash;
        stateMachine.InputReader.EquipEvent += OnEquip;
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.MeleeEvent += OnMelee;
        stateMachine.InputReader.SpecialBeamEvent += InputReader_SpecialBeamEvent;
    }

    public override void Tick(float deltaTime)
    {
        var hitData = stateMachine.EnviromentScaner.ObstacleCheck();        

        if (grounded)
        {
            //Debug.Log("Grounded State :: I am grounded");
            if (hitData.forwardHitFound)
            {
                if (stateMachine.InputReader.MovementValue.magnitude > 0.1f)
                {
                    foreach (var action in stateMachine.ParkourActions)
                    {
                        if(action.CheckIfPossible(hitData,stateMachine.transform))
                        Debug.Log(action.CheckIfPossible(hitData, stateMachine.transform));
                        if (action.CheckIfPossible(hitData, stateMachine.transform))
                        {
                            Debug.Log("Obstacle Found" + hitData.forwardHit.transform.name);
                            stateMachine.SwitchState(new PlayerParkourState(stateMachine, action.AnimName, action.RotateToObstacle,
                                action.TargetRotation, action.EnableTargetMatching, action.MatchPos, action));
                            return;
                        }
                    }

                }

            }
        }



        #region Inputs
        if (!stateMachine.InputReader.equipingWeapon)
        {
            if (stateMachine.InputReader.isAiming)
            {
                stateMachine.SwitchState(new AimingState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.AttackButtonPressed)
            {
                stateMachine.SwitchState(new FiringState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.mediumShot)
            {
                stateMachine.SwitchState(new FiringState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.chargedShot)
            {
                stateMachine.SwitchState(new FiringState(stateMachine));
                return;
            }



            if (stateMachine.InputReader.Modified)
            {
                //Debug.Log("Grounded State:: input reader value: " + stateMachine.InputReader.Modified);
            }
            else
            {
                //Debug.Log("Grounded State:: input reader value: " + stateMachine.InputReader.Modified);
            }

        }
        #endregion

        #region Movement
        Vector3 movement = CalculateMovement().normalized;        
        Move(movement * freeLookMoveSpeed, deltaTime);

        if (GetNormalizedTime(stateMachine.Animator, "Stance") > 1f)
        {
            stateMachine.Animator.SetFloat("isEquiped",
                (stateMachine.InputReader.FightingStance) ? 1 : 0);
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }
        else if(GetNormalizedTime(stateMachine.Animator,"Stance") < 1f && stateMachine.InputReader.MovementValue.magnitude > 0f )
        {
            stateMachine.Animator.SetFloat("isEquiped",
                (stateMachine.InputReader.FightingStance) ? 1 : 0);
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }




        //Debug.Log($"Grounded State::{grounded}");
        if (!grounded)
        {
            Debug.Log("Not Touching Ground");
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
            return;
        }

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }


        //freeLookValue = Mathf.Clamp(
        //    stateMachine.InputReader.MovementValue.magnitude, 0f, 1f);
        //float magnitude = Mathf.Clamp(stateMachine.InputReader.MovementValue.magnitude, 0f, 1f);
        float magnitude = stateMachine.InputReader.MovementValue.magnitude;
        //Debug.Log($"Magnitude: {magnitude}");
        if (magnitude > 0 && magnitude < .3f)
        {
            Debug.Log("slight tilt");
            freeLookValue = .5f;
            freeLookMoveSpeed = 2f;
        }
        
        if (magnitude > .3f && magnitude < .6f)
        {
            Debug.Log("medium tilt");
            freeLookValue = 1;
            freeLookMoveSpeed = 3f;
        }
                
        if (magnitude > .6f)
        {
            Debug.Log("Full tilt");
            freeLookValue = 1.5f;
            freeLookMoveSpeed = 5f;
        }
        //Debug.Log($"FreeLook Value: {freeLookValue}");
        //else
        //{
        //    freeLookValue = 0;
        //}

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, freeLookValue, AnimatorDampTime, deltaTime);


        FaceMovement(movement, deltaTime);
        #endregion






    }
    //public void OnAttack()
    //{
    //    stateMachine.SwitchState(new AttackingState(stateMachine,0));
    //    return;
    //}



    public void OnMelee()
    {
        stateMachine.SwitchState(new AttackingState(stateMachine, 0));
        return;
    }


    public void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        return;
    }

    private void OnDash()
    {
        stateMachine.SwitchState(new DashState(stateMachine));
        return;
    }

    private void InputReader_SpecialBeamEvent()
    {
        stateMachine.SwitchState(new PlayerHyperBeam(stateMachine));
    }

    private void OnEquip()
    {
        Debug.Log(stateMachine.InputReader.FightingStance);
        switch (stateMachine.InputReader.FightingStance)
        {

            case false:
                stateMachine.Animator.Play(EquipHash);
                break;

            case true:
                stateMachine.Animator.Play(UnEquipHash);
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DashEvent -= OnDash;
        stateMachine.InputReader.EquipEvent -= OnEquip;
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.MeleeEvent -= OnMelee;
        stateMachine.InputReader.SpecialBeamEvent -= InputReader_SpecialBeamEvent;

    }

    public void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        stateMachine.InputReader.Targeting = true;
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        return;

    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();



        return forward * stateMachine.InputReader.MovementValue.y +
               right * stateMachine.InputReader.MovementValue.x;
    }
}
