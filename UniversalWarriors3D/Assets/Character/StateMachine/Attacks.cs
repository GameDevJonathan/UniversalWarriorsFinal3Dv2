using System;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class Attacks
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }

    [field: SerializeField] public int AttackForce { get; private set; }

    [SerializeField] public bool MultiHitCombo = false;
    [SerializeField] public int MultiHitIndex = 0;
    [field: SerializeField] public bool ShouldAddAttackForce { get; private set; }
    [field: SerializeField] public bool LauncherAttack { get; private set; }
    [field: SerializeField] public bool KnockDown { get; set; }

    [Range(0f, 1f)]
    [SerializeField] public float TimeForce;

    [MinMaxRangeSlider(0, 1)]
    public Vector2[] ImpactWindow;

    [field: SerializeField] public float UpForce { get; private set; }
    [field: SerializeField] public float ForwardForce { get; private set; }
    [field: SerializeField] public float LaunchForce { get; private set; }
    [field: SerializeField] public float stunForce { get; private set; }

    public enum HitBox { Right = 0, Left = 1, RightFoot = 2, LeftFoot = 3, Special = 4 };
    public HitBox hitBox;
    public GameObject VFX;



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


[Serializable]
public class AirAttacks
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }

    [field: SerializeField] public int AttackForce { get; private set; }

    [SerializeField] public bool MultiHitCombo = false;
    [SerializeField] public int MultiHitIndex = 0;
    [field: SerializeField] public bool ShouldAddAttackForce { get; private set; }
    [field: SerializeField] public bool LauncherAttack { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] public float TimeForce;

    [MinMaxRangeSlider(0, 1)]
    public Vector2[] ImpactWindow;

    [field: SerializeField] public float UpForce { get; private set; }
    [field: SerializeField] public float ForwardForce { get; private set; }
    [field: SerializeField] public float LaunchForce { get; private set; }

    public enum HitBox { Right = 0, Left = 1, RightFoot = 2, LeftFoot = 3, Special = 4 };
    public HitBox hitBox;

    public void SetHitBox(HitBox box)
    {
        hitBox = box;
    }
}
[Serializable]
public class TakeDowns
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public float Takedowndistance { get; private set; }

    /// <summary>    
    ///  Move to point 
    ///  Argument 1: Point where target Object should move to 
    ///  Argument 2: Point where second Object should move to 
    ///  Argument 3: Distance from target Object    
    /// </summary>


    public void MoveToPoint(Transform target, Transform point, float distance)
    {
        //target.transform.position = new Vector3(point.transform.position.x, point.transform.position.y, point.transform.position.z + distance);
        target.transform.position = point.transform.position;

    }

}
[Serializable]
public struct Counters
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }

}