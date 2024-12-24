using UnityEngine;
using System.Collections.Generic;
public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    [SerializeField] private PlayerStateMachine player;
    

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other == myCollider) { return; }
        if(alreadyCollidedWith.Contains(other)) { return; }
        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(10);
            
        }
        
    }
}
