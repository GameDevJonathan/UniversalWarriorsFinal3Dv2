using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isInvicible = false;
    [SerializeField] public bool isLaunched = false;
    [SerializeField] public float launchForce = 0f;

    public event Action OnTakeDamage;
    
    private void Start()
    {
        health = maxHealth;
        
    }

    public void DealDamage(int damage)
    {
        if(health == 0) { return; }
        
        if(!isInvicible)
        health = Mathf.Max(health - damage, 0);
        
        OnTakeDamage?.Invoke();
        //Debug.Log(health);

    }

    public void SetAttackType(bool attackType)
    {
        isLaunched = attackType;
        Debug.Log($"{this.gameObject.name} is launched " + isLaunched);
    }

    public void SetLaunchForce(float Force)
    {
        Debug.Log($"Health LaunchForce: {launchForce}");

        launchForce = Force;
    }


}
