using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxData : MonoBehaviour
{
    public Attacks attack;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        attack = playerStateMachine.SetAttackIndex();
        if (collider.enabled && collider != null)
        {
            //Debug.Log($"Attack Force: {attack.AttackForce}");
        }
        
    }
}
