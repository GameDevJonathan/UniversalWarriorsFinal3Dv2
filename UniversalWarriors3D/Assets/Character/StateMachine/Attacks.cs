using System;
using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public class Attacks 
{
    [field:SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = - 1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    
    [field: SerializeField] public float AttackForce { get; private set; } 
    [field: SerializeField] public bool ShouldAddAttackForce { get; private set; }

    [Range(0f,1f)]
    [SerializeField] public float TimeForce;

    [MinMaxRangeSlider(0, 1)] 
    public Vector2[] attackTime;
    
    [field: SerializeField] public float UpForce { get; private set; } 
    [field: SerializeField] public float ForwardForce { get; private set; } 
    
}

[Serializable]
public class SpecialMoves
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    [field: SerializeField] public float AttackForce { get; private set; }
    [field: SerializeField] public float TimeForce { get; private set; }

}