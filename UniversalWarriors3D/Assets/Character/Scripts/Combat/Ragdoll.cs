using System.Runtime.CompilerServices;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    private Collider[] allColliders;
    private Rigidbody[] allRigidBodies;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidBodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagDoll)
    {
        foreach(Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagDoll;
            }
        }

        foreach (Rigidbody rigidbody in allRigidBodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagDoll;
                rigidbody.useGravity = isRagDoll;
            }
        }

        characterController.enabled = !isRagDoll;
        animator.enabled = !isRagDoll;
    }   
}
