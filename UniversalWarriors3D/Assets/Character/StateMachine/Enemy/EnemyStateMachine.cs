using UnityEngine;
using Unity.Cinemachine;
 

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }

    [MinMaxRangeSlider(0,100)]
    [Tooltip("Idle Timer to trigger random idle animation")]
    [field:SerializeField] public Vector2 IdleRangeTimer { get; private set; }


    private void Start()
    {
        SwitchState(new EnemyIdleState(this));
    }

}
