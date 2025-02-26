using UnityEngine;

public class AddForce : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        rb.isKinematic = false;
        rb.useGravity = true;

    }
}
