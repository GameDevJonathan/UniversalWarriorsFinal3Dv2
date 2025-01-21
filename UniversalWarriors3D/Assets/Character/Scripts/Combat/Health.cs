using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isInvicible = false;
    [SerializeField] public bool isLaunched = false;
    [SerializeField] public float launchForce = 0f;
    [SerializeField] public float stunValue = 0f;
    [SerializeField] public float stunDecreaseSpeed = 1f;
    [SerializeField] public bool isStunned = false;
    [SerializeField] public Coroutine stunDecrease;

    public event Action OnTakeDamage;
    
    private void Start()
    {
        health = maxHealth;
        
    }

    private void Update()
    {
        stunValue = Mathf.Clamp(stunValue, 0f, 100f);
        if(stunValue >= 100 && !isStunned)
            isStunned=true;

        if (isStunned && stunDecrease == null)
        {
            Debug.Log("I am stunned");
            stunDecrease = StartCoroutine(StunDecrease());
        }
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
        //Debug.Log($"{this.gameObject.name} is launched " + isLaunched);
    }

    public void SetLaunchForce(float Force)
    {
        Debug.Log($"Health LaunchForce: {launchForce}");

        launchForce = Force;
    }

    public void SetStun(float stunValue)
    {
        this.stunValue += stunValue;
    }

    IEnumerator StunDecrease()
    {
        while(stunValue > 0f)
        {
            stunValue -= Time.deltaTime * stunDecreaseSpeed;
            yield return null ;
        }
        isStunned = false;
        stunDecrease = null;
    }


}
