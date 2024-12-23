using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxData : MonoBehaviour
{
    public Attacks attack;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = (col) ? col : GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        attack = playerStateMachine.SetAttackIndex();
        if (col.enabled && col != null)
        {
            
        }
        
    }
}
