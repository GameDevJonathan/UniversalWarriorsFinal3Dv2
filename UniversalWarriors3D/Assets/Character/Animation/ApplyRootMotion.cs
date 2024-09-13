using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRootMotion : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            Debug.Log("Got Animator");
        }        
    }

    public void SetRootMotion(int apply) {

        Debug.Log("Firing");
    }

   
}
