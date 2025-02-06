using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public GameObject uiContainer;
    [SerializeField] public GameObject UIComponent;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Image stunBar;
    [SerializeField] private EnemyStateMachine EnemyStateMachine;
    [SerializeField] private LayerMask layerMask;

    public event Action OnTakeDamage;

    private void Awake()
    {
        if (uiContainer != null)
            uiContainer?.gameObject.SetActive(false);

        if (this.stunBar != null)
        {
            this.stunBar.fillAmount = stunValue;
        }
    }


    private void Start()
    {
        health = maxHealth;
        mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (EnemyStateMachine)
            Debug.Log("Grounded " + EnemyStateMachine.CharacterController.isGrounded);

        stunValue = Mathf.Clamp(stunValue, 0f, 100f);
        if (stunBar)
            stunBar.fillAmount = stunValue / 100;


        if (stunValue >= 100 && !isStunned)
            isStunned = true;

        if (isStunned && stunDecrease == null)
        {
            Debug.Log("I am stunned");
            stunDecrease = StartCoroutine(StunDecrease());
            uiContainer?.gameObject.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        if (UIComponent != null)
            UIComponent.transform.LookAt(mainCamera.transform);
    }

    public void DealDamage(int damage)
    {
        if (health == 0) { return; }

        if (!isInvicible)
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
        if (isStunned) return;
        this.stunValue += stunValue;
        if (stunBar != null)
            this.stunBar.fillAmount += stunValue / 100;
    }

    IEnumerator StunDecrease()
    {
        while (stunValue > 0f)
        {
            stunValue -= Time.deltaTime * stunDecreaseSpeed;
            yield return null;
        }
        isStunned = false;
        stunDecrease = null;
        uiContainer.gameObject.SetActive(false);
    }

    public void TakeDownReset()
    {
        isStunned = false;
        stunValue = 0f;
        StopCoroutine(StunDecrease());
        stunDecrease = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Object layer " + hit.gameObject.layer);
        int layer = LayerMask.NameToLayer("Wall");
        if (!EnemyStateMachine) return;
        if (EnemyStateMachine.CharacterController.isGrounded) return;
        if (hit.gameObject.layer != layer) return;

        Debug.LogWarning("I hit a wall");
        if (!EnemyStateMachine.wallSplat)
        {
            EnemyStateMachine.SwitchState(new EnemyWallSplatState(EnemyStateMachine));
            EnemyStateMachine.wallSplat = !EnemyStateMachine.wallSplat;
            return;

        }
    }


}
