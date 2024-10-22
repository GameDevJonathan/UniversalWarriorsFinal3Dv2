using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class NPCInteractable : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private bool canOpen = true;
    [SerializeField] private bool isOpen;
    [SerializeField] private float rotValue = 0f;
    [SerializeField] private float maxRotation = 90f;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private float time = 0f;
    private Coroutine doorRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /// if I can open the door press the button to open the door
        /// if the door isn't at certain value i can't open or close the door

        //canOpen = ((rotValue <= -maxRotation) || (rotValue >= 0f)) ? true : false;

        if (rotValue >= 0)
            isOpen = false;
        else
        if (rotValue <= -90f)
            isOpen = true;

        //if ( (transform.eulerAngles.y == 0 || transform.eulerAngles.y == 270))
        //{

        //    if (doorRoutine != null)
        //    {
        //        StopCoroutine(doorRoutine);
        //        time = 0f;
        //        canOpen = true;
        //        doorRoutine = null;
        //    }



        //}
    }

    private IEnumerator OpenDoorRoutine()
    {
        if (isOpen)
            rotValue = 0f;
        else
            rotValue = -maxRotation;

        while (time < 1)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotValue, 0), time);
            time += Time.deltaTime * lerpSpeed;
            yield return null;

        }

        Debug.Log(transform.eulerAngles.y);

        time = 0f;
        canOpen = true;
        doorRoutine = null;
    }

    public string GetInteractText()
    {
        return "OpenDoor";
    }

    public void test()
    {
        //Debug.Log("test");
    }

    public void Interact()
    {
        //Debug.Log("Interact");
        if (canOpen)
            Opendoor();
    }

    private void Opendoor()
    {
        //Debug.Log("Openning Door");

        if (doorRoutine == null)
        {
            if (canOpen)
            {
                canOpen = false;
                //Debug.Log("OpenDoor Routine");
                doorRoutine = StartCoroutine(OpenDoorRoutine());

            }
        }
    }

    //public Transform GetTransform()
    //{
    //    return animator.transform;
    //}

    /*
        lerp position from on to another
        matf.lerp (value a, value b, time.deltaTime * lerpspeed)
        I also want to use transform.rotate(vector3.up (yaxis), plus lerp value
     */
}
