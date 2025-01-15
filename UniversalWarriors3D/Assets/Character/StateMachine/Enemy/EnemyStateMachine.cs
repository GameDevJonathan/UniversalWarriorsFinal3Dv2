using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.AI;


public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    public GameObject Player { get; private set; }

    [field: SerializeField] public float PlayerDectionRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [SerializeField] public float LaunchForce;  
    [field: SerializeField] public float RotationSmoothValue { get; private set; }
    [field: SerializeField] public bool Dummy { get; private set; }

    [Tooltip("Idle Timer to trigger random idle animation")]
    [MinMaxRangeSlider(0, 100)]
    public Vector2 IdleRangeTimer;

    [Tooltip("Time to Attack")]
    [MinMaxRangeSlider(0, 5)]
    public Vector2 AttackTimerRange;


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
        //Debug.Log($"Enemy State Machine Damage Event: {Health.isLaunched}");
        //LaunchForce = Health.launchForce;
        Debug.Log($"Enemy State Machine: Launch force = {LaunchForce}"); 
        if (Health.isLaunched)
        {
            SwitchState(new EnemyLaunchedState(this,LaunchForce));
        }
        else
            SwitchState(new EnemyImpactState(this));
    }

    public void SetLaunchForce(int force)
    {
        LaunchForce = force;
    }

}
