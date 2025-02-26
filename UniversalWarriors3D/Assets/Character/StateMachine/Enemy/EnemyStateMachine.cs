using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;


public class EnemyStateMachine : StateMachine
{
    [field: Header("Components")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public EnemyPlayerDetector PlayerDetector { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }


    [field: Header("Attack Animation")]
    [field: SerializeField] public Attacks[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage[] Weapon { get; private set; }
    [field: SerializeField] public Collider[] hitBoxes { get; private set; }
    [field: SerializeField] public int AttackIndex { get; private set; }
    [field: SerializeField] public int CurrentAttackIndex { get; set; }


    [field: Header("TakeDown Animations")]
    [field: SerializeField] public TakeDowns[] TakeDowns { get; private set; }
    [field: SerializeField] public Counters Counters { get; private set; }
    [field: SerializeField] public GameObject Player { get; set; }

    [field: SerializeField] public float PlayerDectionRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public bool KnockDown { get; set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float StrafeMovementSpeed { get; private set; }

    [SerializeField] public float LaunchForce;
    [field: SerializeField] public float RotationSmoothValue { get; private set; }

    [field: Header("State Machine Dummy")]
    [field: SerializeField] public bool Dummy { get; private set; }

    [field: Space(10f)]
    [field: SerializeField] public bool CanHit { get; set; }
    [field: SerializeField] public bool wallSplat { get; set; }



    [Tooltip("Idle Timer to trigger random idle animation")]
    [MinMaxRangeSlider(0, 100)]
    public Vector2 IdleRangeTimer;

    [Tooltip("Time to Attack")]
    [MinMaxRangeSlider(0, 5)]
    public Vector2 AttackTimerRange;

    [Tooltip("Time To Get Up")]
    [MinMaxRangeSlider(0, 10)]
    public Vector2 UpTime;


    private void Start()
    {
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new EnemyIdleState(this));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDectionRange);
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += DamageEvent;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= DamageEvent;
    }



    private void DamageEvent()
    {
        if (!CanHit) return;
        //Debug.Log($"Enemy State Machine Damage Event: {Health.isLaunched}");
        //LaunchForce = Health.launchForce;
        //Debug.Log($"Enemy State Machine: Launch force = {LaunchForce}");
        if (CharacterController.isGrounded)
        {

            if (this.KnockDown)
            {
                Debug.Log("Enemy State Machine:: Switch to knock down State");
                SwitchState(new EnemyKnockDownState(this, LaunchForce));

            }
            else if (Health.isLaunched)
            {
                SwitchState(new EnemyLaunchedState(this, LaunchForce));
                return;
            }
            else
            {
                //if (hitStop == null)
                //    hitStop = StartCoroutine(HitStopRoutine(hitStopTime));

                SwitchState(new EnemyImpactState(this));
                return;
            }
        }
        else if (!CharacterController.isGrounded)
        {
            Debug.Log("Hit In air");
            SwitchState(new EnemyImpactAirState(this));
            return;
        }

    }



    public void CurrentState()
    {
        Debug.Log(currentState);
    }

    public void SetLaunchForce(int force)
    {
        LaunchForce = force;
    }










}
