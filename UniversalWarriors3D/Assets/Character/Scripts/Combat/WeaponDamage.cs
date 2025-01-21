using UnityEngine;
using System.Collections.Generic;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    [SerializeField] Collider myTargeter;
    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();
    [SerializeField] private PlayerStateMachine player;

    private int damage;
    private bool launching;
    public float launchForce;
    private bool knockDown;
    private float stunForce;


    private void OnEnable()
    {
        alreadyCollidedWith.Clear();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor") || other == myCollider) { return; }
        
        //Debug.Log(other.gameObject.name);

        if (alreadyCollidedWith.Contains(other)) { return; }
        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine stateMachine))
        {
            stateMachine.LaunchForce = launchForce;
            stateMachine.KnockDown = knockDown;
        }
        
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.SetAttackType(launching);
            health.DealDamage(damage);
            health.SetStun(stunForce);
        }


    }

    public void SetAttack(int damage)
    {
        //Debug.Log("Damage Value: " + damage);
        this.damage = damage;
    }

    public void SetAttackType(bool launching)
    {
        this.launching = launching;
    }

    public void SetLaunchForce(float launchForce)
    {
        //Debug.Log($"WeaponDamage LaunchForce: {launchForce}");
        this.launchForce = launchForce;
    }

    public void SetKnockDown(bool knockDown)
    {
        this.knockDown = knockDown;
    }

    public void setStunForce(float stunForce)
    {
        this.stunForce = stunForce;
    }


}
