using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isDead = false;

    public event Action OnTakeDamage;
    
    private void Start()
    {
        health = maxHealth;
        
    }

    public void DealDamage(int damage)
    {
        if(health == 0) { return; }
        health = Mathf.Max(health - damage, 0);
        OnTakeDamage?.Invoke();
        Debug.Log(health);

    }


}
