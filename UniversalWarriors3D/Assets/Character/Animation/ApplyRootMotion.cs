using UnityEngine;

public class ApplyRootMotion : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterController characterController;
    public float upSpeed = 3f;
    public float forwardSpeed = 3f;
    private Vector3 upperCutDirection;
    private Vector3 upperCutUpDirection;
    private Vector3 upperCutFwdDirection;
    private bool startLifting;
    [SerializeField] private Transform Daku;

    

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (animator != null)
        {
            Debug.Log("Got Animator");
        }
        
        if(characterController != null)
        {
            Debug.Log("Got Controller");
        }

        Debug.Log("lift off");
       
    }

    private void FixedUpdate()
    {
        upperCutUpDirection = Daku.transform.up * upSpeed;
        upperCutFwdDirection = Daku.transform.forward * forwardSpeed;
        upperCutDirection = upperCutUpDirection + upperCutFwdDirection;
        if ( startLifting) characterController.Move(upperCutDirection * Time.deltaTime) ;

    }

    public void SetRootMotion(int apply) {

        Debug.Log("Firing");
    }

    public void PlaySound()
    {
        Debug.Log("Firing");
    }

    public void LiftOff()
    {
        startLifting = true;
    }

    public void StopMotion()
    {
        startLifting = false;
    }

   
}
