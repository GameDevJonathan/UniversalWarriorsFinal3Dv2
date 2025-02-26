using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    [SerializeField] Collider myTargeter;
    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private EnemyStateMachine enemy;

    private int damage;
    private bool launching;
    public float launchForce;
    public bool knockDown;
    private float stunForce;


    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
        //Debug.Log(this.transform.gameObject.transform.root.tag);

        if (this.transform.gameObject.transform.root.CompareTag("Boss"))
        {
            enemy = transform.gameObject.transform.root.GetComponent<EnemyStateMachine>();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor") || other == myCollider) { return; }

        //Debug.Log(other.gameObject.name);

        if (alreadyCollidedWith.Contains(other)) { return; }
        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
        {
            //Debug.Log(player.gameObject.name);
        }

        if (other.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine stateMachine))
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

        if (other.transform.CompareTag("Floor"))
        {
            if (this.transform.gameObject.transform.root.CompareTag("Boss"))
            {
                Debug.Log("I hit the floor");
                Vector3 contact = other.ClosestPoint(transform.position);
                Debug.Log(contact);
                if (enemy.CurrentAttackIndex == 0)
                    Instantiate(enemy.Attacks[enemy.CurrentAttackIndex].VFX, contact, quaternion.identity);
            }
        }

        //player.SetAttackIndex();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Floor"))
        {
            if (this.transform.gameObject.transform.root.CompareTag("Boss"))
            {
                Debug.Log("I hit the floor");
                ContactPoint contact = other.contacts[0];
                Debug.Log(contact.point);
            }
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
