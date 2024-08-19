using UnityEngine;
using Invector;

public class Health : MonoBehaviour
{
    [SerializeField] private vHealthController health;
    [SerializeField] private float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = health.currentHealth;
    }
}
