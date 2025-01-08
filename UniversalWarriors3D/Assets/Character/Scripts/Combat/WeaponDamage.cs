using UnityEngine;
using System.Collections.Generic;
public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    [SerializeField] Collider myTargeter;
    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();
    [SerializeField] private PlayerStateMachine player;

    private int damage;


    private void OnEnable()
    {
        alreadyCollidedWith.Clear();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor") || other == myCollider) { return; }
        
        Debug.Log(other.gameObject.name);

        if (alreadyCollidedWith.Contains(other)) { return; }
        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);

        }

    }

    public void SetAttack(int damage)
    {
        Debug.Log("Damage Value: " + damage);
        this.damage = damage;
    }


}
