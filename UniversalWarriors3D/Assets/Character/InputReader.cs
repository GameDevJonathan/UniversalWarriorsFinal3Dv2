using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    [SerializeField] private PlayerStateMachine stateMachine;

    public Vector2 MovementValue { get; private set; }
    public Vector2 CameraValue { get; private set; }

    [Range(0, 1)] public float TimeScale = 1f;

    private Controls controls;

    #region CameraReset
    [SerializeField, Range(0, 20)] private float _lerpSpeed = 5f;
    [SerializeField, Range(0, 1)] private float _lerpTime = 1f;
    [SerializeField] private float _Time = 0f;
    [SerializeField] private float _InitialCameraYaw = 0f;
    [SerializeField] private float _InitialCameraPitch = 0f;
    private Coroutine _resetCamera;
    [SerializeField] private bool _softReset = false;
    #endregion

    #region Gameplay bools
    [Header("Gameplay bools")]
    public bool Modified;
    public bool Skills;
    public float triggerValue;

    public bool JumpButtonPressed => controls.Player.Jump.WasPressedThisFrame();
    public bool AttackButtonPressed => controls.Player.LightAttack.WasPressedThisFrame();
    public bool AttackButtonHeld => controls.Player.LightAttack.WasPerformedThisFrame();
    public bool HeavyAttackButtonPressed => controls.Player.HeavyAttack.WasPressedThisFrame();
    public bool BlockButtonReleased => controls.Player.Block.WasReleasedThisFrame();
    public bool GrabButtonPressed => controls.Player.Grab.WasPressedThisFrame();

    public bool TakeDownButton => controls.Player.TakeDown.WasPerformedThisFrame();

    [HideInInspector] public bool shoot;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool fire;
    [HideInInspector] public bool mediumShot = false;
    [HideInInspector] public bool chargedShot = false;
    #endregion

    #region Aiming
    [Header("Aiming/TargetSelection")]
    [field: SerializeField] public bool isDashing;
    [field: SerializeField] public bool isAiming { get; private set; }
    [field: SerializeField] public Vector2 SelectionValue { get; private set; }
    public bool Targeting;
    #endregion

    #region Weapons
    [field: Space]
    [field: Header("Weapons")]
    [SerializeField] public bool FightingStance;
    [field: SerializeField] public float FightingStanceCoolDown { get; private set; } = .3f;
    [field: SerializeField] public float FightingStanceCoolDownTime { get; private set; } = 0;
    [field: SerializeField] public bool equipingWeapon { get; private set; } = false;
    #endregion

    #region Shader
    [Header("Shader Glow")]
    public string AnimChargeRef = "ChargeLevel";
    public int chargeLevel;
    public float chargeAmount;
    public float chargeRate;
    public float _minRate = 25f;
    public float _midRate = 50f;
    public float _maxRate = 100f;
    #endregion

    [Header("Energy Gathering Effect")]
    [SerializeField] private GameObject _maxChargeEffect;
    [SerializeField] private GameObject _minChargeEffect;

    //EVENTS ACTIONS
    #region action events
    //Actions
    public event Action BlockEvent;
    public event Action CancelEvent;
    public event Action DashEvent;
    public event Action DodgeEvent;
    public event Action EquipEvent;
    public event Action GrabEvent;
    public event Action JumpEvent;
    public event Action MeleeEvent;
    public event Action HeavyMeleeEvent;
    public event Action TargetEvent;
    public event Action SpecialBeamEvent;
    #endregion



    public Transform Player;
    //public event Action AttackEvent;

    [field: Header("Attacking")]
    [field: SerializeField] public bool isAttacking { get; private set; }
    //camera    
    //Transform cam;

    #region Cinemachine
    [Header("Cinemachine")]
    [Tooltip("Camera Sensitivity")]
    public float Sensitivity = 1;

    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    public GameObject CinemachineInitPos;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;


    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("Camera Rotation Speed")]
    public float CameraRotationSpeed = 3f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;


    // cinemachine
    [SerializeField] private float _cinemachineTargetYaw;
    [SerializeField] private float _cinemachineTargetPitch;
    private const float _threshold = 0.01f;
    #endregion

    //Material
    [Header("Material Glow")]
    //[SerializeField] private Material _material;
    [SerializeField] private bool _canFlicker;
    [SerializeField] public float _targetValue;
    [SerializeField] private float _minTarget;
    [SerializeField] private float _midTarget;
    [SerializeField] private float _maxTarget;
    [SerializeField] private string _boolRef;
    [SerializeField] private string _targetRef;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //_InitialCameraYaw = CinemachineInitPos.transform.rotation.eulerAngles.y;

        //_material?.SetFloat(_targetRef, _targetValue);
    }



    private void Update()
    {
        //Time.timeScale = TimeScale;
        _InitialCameraYaw = CinemachineInitPos.transform.rotation.eulerAngles.y;
        _InitialCameraPitch = CinemachineInitPos.transform.rotation.eulerAngles.x;

        _targetValue = Mathf.Clamp(_targetValue, .1f, 1f);
        triggerValue = Mathf.Clamp(triggerValue, 0f, 1f);
        Modified = (triggerValue >= 0.01f);

        FightingStanceCoolDownTime = Mathf.Clamp(FightingStanceCoolDownTime, 0f, 1f);
        if (FightingStanceCoolDownTime > 0f) FightingStanceCoolDownTime -= Time.deltaTime;


        if (controls.Player.ResetCamera.WasPressedThisFrame() && _softReset == false)
        {
            //_softReset = true;
            //StartCoroutine(SoftReset());
        }

        //FlickerMaterial(_material, _boolRef, _canFlicker);

        _canFlicker = chargeLevel >= 1 ? true : false;

        if (charge)
        {
            chargeAmount += chargeRate * Time.deltaTime;
        }

        if (!charge && chargeAmount > 0)
        {
            chargeAmount = 0;
        }

        chargeAmount = Mathf.Clamp(chargeAmount, 0, 100);
    }



    private void LateUpdate()
    {

        if (stateMachine.SpecialMove || Targeting) return;
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (CameraValue.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            if (isAiming)
            {
                _cinemachineTargetYaw += CameraValue.x * Sensitivity;
                _cinemachineTargetPitch -= CameraValue.y * Sensitivity;
            }
            else if (!isAiming && _softReset == false)
            {
                _cinemachineTargetYaw += CameraValue.x;
                _cinemachineTargetPitch -= CameraValue.y;
            }
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void EquipingWeapon()
    {

        equipingWeapon = false;
    }

    public void FlickerMaterial(Material mat, string ShaderBoolRef, bool on_off)
    {
        //Debug.Log($"on/off: {mat.GetInt(ShaderBoolRef)}");


    }

    public void OnBlock(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            BlockEvent?.Invoke();
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        CameraValue = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //Debug.Log($"Trigger value: {context.ReadValue<float>()}");
        //if (!context.performed) { return; }
        //DashEvent?.Invoke();
        triggerValue = context.ReadValue<float>();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DodgeEvent?.Invoke();
        }

    }

    public void OnEquip(InputAction.CallbackContext context)
    {
        //if(context.performed && FightingStanceCoolDownTime == 0f)
        //{
        //    EquipEvent?.Invoke();
        //    FightingStance = !FightingStance;
        //    FightingStanceCoolDownTime = FightingStanceCoolDown;
        //}

    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        //Debug.Log("Context...:" + context);
        //if (!context.performed) { return; }
        //GrabEvent?.Invoke();
    }


    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.performed && !Targeting)
        {
            //Debug.Log("ButtonPressed");
            //rotation = new Quaternion(0, 0, 0,0);            
            TargetEvent?.Invoke();
        }

        else if (context.performed && Targeting)
        {

            Targeting = false;
            CancelEvent?.Invoke();
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("Context...:" + context);
        if (context.started)
        {
            MeleeEvent?.Invoke();
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HeavyMeleeEvent?.Invoke();
        }
    }

    public void OnSkills(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Skills = true;
        }

        if (context.canceled)
        {
            Skills = false;
        }
        //Debug.Log($"Skills context: {context}");
        //Debug.Log("On Skills: " + Modified);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //MovementValue = context.ReadValue<Vector2>();
        MovementValue = context.ReadValue<Vector2>();

    }

    public void OnSpecialBeam(InputAction.CallbackContext context)
    {
        Debug.Log("Context...:" + context);
        if (context.performed)
        {
            SpecialBeamEvent?.Invoke();
        }
    }

    public void OnTargetSelection(InputAction.CallbackContext context)
    {
        SelectionValue = context.ReadValue<Vector2>();
    }

    public void ResetCamera()
    {
        _cinemachineTargetYaw = _InitialCameraYaw;
        _cinemachineTargetPitch = 0;
        //StartCoroutine(SoftReset());
    }

    IEnumerator SoftReset()
    {
        while ((_cinemachineTargetYaw != _InitialCameraYaw) && (_cinemachineTargetPitch != _InitialCameraPitch))
        {
            yield return null;
            Debug.Log($"Appoximately: {Mathf.Approximately(_cinemachineTargetYaw, _InitialCameraYaw)}");
            _cinemachineTargetYaw = Mathf.Lerp(_cinemachineTargetYaw, _InitialCameraYaw, _Time);
            _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, _InitialCameraPitch, _Time);
            _Time += Time.deltaTime * _lerpSpeed;
        }
        _softReset = false;
        _Time = 0;
    }


    public void OnResetCamera(InputAction.CallbackContext context)
    {

    }

    public void OnParkour(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Parkour Context: ${context}");
        }

        if (context.canceled)
        {
            Debug.Log($"Parkour Context: ${context}");
        }
    }

    public void OnTakeDown(InputAction.CallbackContext context)
    {

    }

}
