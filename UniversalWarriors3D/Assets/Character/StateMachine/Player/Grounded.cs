
using UnityEngine;


public class Grounded : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("Movement");
    private readonly int EquipHash = Animator.StringToHash("Equip");
    private readonly int UnEquipHash = Animator.StringToHash("UnEquip");
    public float AnimatorDampTime = 0.01f;
    private float freeLookValue;
    private float freeLookMoveSpeed;
    private float equipTime;
    private float _time = 2f;
    private float dropHeight;
    private bool shouldFade;
    private bool skills => stateMachine.InputReader.Skills;
    Vector3 movement;

    private const float CrossFadeDuration = 0.1f;
    private bool grounded => stateMachine.WallRun.CheckForGround();

    public bool isOnLedge { get; set; }
    ObstacleHitData1 hitData;




    public Grounded(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.freeLookMoveSpeed = stateMachine.FreeLookMovementSpeed;
        this.shouldFade = shouldFade;
        this.equipTime = (stateMachine.InputReader.FightingStance) ? 1 : 0;

    }

    public override void Enter()
    {
        if (stateMachine.InputReader.Targeting)
        {
            stateMachine.InputReader.Targeting = false;
            stateMachine._TargetCamUtil.SetActive(false);
            stateMachine.debugTransform.gameObject.SetActive(false);
        }

        if (stateMachine.rig != null)
        {
            stateMachine.rig.weight = 0f;
        }



        if (!shouldFade)
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        else
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.BlockEvent += OnBlock;
        stateMachine.InputReader.GrabEvent += OnGrab;
        stateMachine.InputReader.MeleeEvent += OnMelee;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.SpecialBeamEvent += InputReader_SpecialBeamEvent;
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
    }

    public override void Tick(float deltaTime)
    {

        Debug.Log("Skills: " + skills);
        stateMachine.EquipTime = Mathf.Clamp(stateMachine.EquipTime, 0, stateMachine.EquipTime);

        //Quality of life for whether Player is in fighting stance or not.
        switch (stateMachine.InputReader.FightingStance)
        {
            case true:
                if (stateMachine.EquipTime > 0f)
                {
                    stateMachine.EquipTime -= deltaTime;
                }

                if (stateMachine.EquipTime == 0f)
                {
                    equipTime = Mathf.Clamp(equipTime, 0, equipTime);
                    equipTime -= _time * deltaTime;
                    //Debug.Log($"equipTime: {equipTime}");
                    stateMachine.Animator.SetFloat("isEquiped", equipTime);
                }

                if (equipTime <= 0)
                {
                    equipTime = 0f;
                    stateMachine.InputReader.FightingStance = false;
                }


                break;

            case false:
                break;
        }

        //Special Moves
        if (skills)
        {
            if (stateMachine.InputReader.AttackButtonPressed)
            {
                stateMachine.SwitchState(new PlayerSpecialMoveState(stateMachine, 0));
                return;
            }
        }


        //Debug.Log($"WallRunCheck for ground function {grounded}");

        //Parkour Functions 
        hitData = stateMachine.EnviromentScaner.ObstacleCheck();

        if (grounded)
        {
            //Debug.Log("Grounded State :: I am grounded");
            if (hitData.forwardHitFound)
            {
                if (stateMachine.InputReader.MovementValue.magnitude > 0.1f)
                {
                    foreach (var action in stateMachine.ParkourActions)
                    {
                        if (action.CheckIfPossible(hitData, stateMachine.transform))
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


        //TakeDowns
        if (stateMachine.CanTakeDown && stateMachine.InputReader.TakeDownButton)
        {
            //Todo switch to takedown state
            Debug.Log("Grounded state:: switch to take down state");
            stateMachine.takeDownIndex = Random.Range(0, stateMachine.TakeDowns.Length);
            Debug.Log($"TakeDown Index: {stateMachine.takeDownIndex}");
            FaceTakeDownTarget();

            stateMachine.SwitchState(new PlayerTakeDownState(stateMachine, stateMachine.takeDownIndex));
            return;
        }

        #region Inputs
        if (!stateMachine.InputReader.equipingWeapon)
        {
            //if (stateMachine.InputReader.isAiming)
            //{
            //    stateMachine.SwitchState(new AimingState(stateMachine));
            //    return;
            //}

            //if (stateMachine.InputReader.AttackButtonPressed)
            //{
            //    stateMachine.SwitchState(new FiringState(stateMachine));
            //    return;
            //}

            //if (stateMachine.InputReader.mediumShot)
            //{
            //    stateMachine.SwitchState(new FiringState(stateMachine));
            //    return;
            //}

            //if (stateMachine.InputReader.chargedShot)
            //{
            //    stateMachine.SwitchState(new FiringState(stateMachine));
            //    return;
            //}



            if (stateMachine.InputReader.Modified)
            {
                //Debug.Log("Grounded State:: input reader value: " + stateMachine.InputReader.Modified);
            }
            else
            {
                //Debug.Log("Grounded State:: input reader value: " + stateMachine.InputReader.Modified);
            }

            //if (stateMachine.InputReader.GrabButtonPressed)
            //{
            //    Debug.Log("Grounded State:: input reader value:  pressed");

            //}

        }
        #endregion

        #region Movement
        movement = CalculateMovement().normalized;
        isOnLedge = stateMachine.EnviromentScaner.LedgeCheck(movement);

        if (isOnLedge)
        {
            Debug.Log("On Ledge");
            if (stateMachine.InputReader.MovementValue.magnitude > 0.1f && stateMachine.InputReader.Modified)
            {
                dropHeight = stateMachine.EnviromentScaner.height;
                stateMachine.SwitchState(new PlayerLedgeJumpState(stateMachine, dropHeight));
                return;
            }
        }
        Move(movement * freeLookMoveSpeed, deltaTime);






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
        float magnitude = Mathf.Clamp(stateMachine.InputReader.MovementValue.magnitude, 0f, 1f);
        //float magnitude = stateMachine.InputReader.MovementValue.magnitude;
        //Debug.Log($"Magnitude: {magnitude}");
        if (magnitude > 0 && magnitude < .3f)
        {
            freeLookValue = .5f;
            freeLookMoveSpeed = 2f;
        }
        else if (magnitude > .3f && magnitude < .6f)
        {
            freeLookValue = 1;
            freeLookMoveSpeed = 3f;
        }
        else if (magnitude > .6f && stateMachine.InputReader.Modified)
        {
            freeLookValue = 2f;
            freeLookMoveSpeed = 15f;
            Debug.Log("Should Be Running");
        }
        else if (magnitude > .6f)
        {
            freeLookValue = 1.5f;
            freeLookMoveSpeed = 5f;
        }
        else
        {
            freeLookValue = 0;
        }


        stateMachine.Animator.SetFloat(FreeLookSpeedHash, freeLookValue, AnimatorDampTime, deltaTime);



        FaceMovement(movement, deltaTime);
        #endregion






    }
    //public void OnAttack()
    //{
    //    stateMachine.SwitchState(new AttackingState(stateMachine,0));
    //    return;
    //}    

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.GrabEvent -= OnGrab;
        stateMachine.InputReader.BlockEvent -= OnBlock;
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.MeleeEvent -= OnMelee;
        stateMachine.InputReader.SpecialBeamEvent -= InputReader_SpecialBeamEvent;
        stateMachine.InputReader.DodgeEvent -= OnDodge;


    }

    public void OnDodge()
    {
        float x = stateMachine.InputReader.MovementValue.x;
        float y = stateMachine.InputReader.MovementValue.y;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, Angle(x, y)));
        return;
    }

    public void OnMelee()
    {
        if (skills) return;
        stateMachine.EquipTime = 10f;
        stateMachine.Targeter.SelectClosestTarget();

        if (stateMachine.InputReader.Modified && stateMachine.InputReader.MovementValue.magnitude >= .9f)
        {
            //Debug.Log(index);
            stateMachine.SwitchState(new AttackingState(stateMachine, stateMachine.MoveIndex("JumpKick")));
            return;
        }

        stateMachine.SwitchState(new AttackingState(stateMachine, 0));
        return;
    }


    public void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        return;
    }

    private void OnGrab()
    {
        //stateMachine.SwitchState(new DashState(stateMachine));
        //return;
    }

    private void OnBlock()
    {
        stateMachine.SwitchState(new PlayerBlockState(stateMachine));
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
                stateMachine.Animator.CrossFadeInFixedTime(EquipHash, CrossFadeDuration);
                break;

            case true:
                stateMachine.Animator.CrossFadeInFixedTime(UnEquipHash, CrossFadeDuration);
                break;
        }
    }

    public void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        //Debug.Log("GroundedState:: OnTarget() Target Selection successful");
        stateMachine.InputReader.Targeting = true;
        //stateMachine.InputReader.CinemachineCameraTarget.transform.rotation.eulerAngles.y = 0f;
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

    private void DoParkourAction()
    {
        if (stateMachine.InputReader.MovementValue.magnitude > 0.1f)
        {
            foreach (var action in stateMachine.ParkourActions)
            {
                if (action.CheckIfPossible(hitData, stateMachine.transform))
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
