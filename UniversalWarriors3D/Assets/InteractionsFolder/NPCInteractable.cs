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


        ///check to see if canOpen is true of false. if it's false swing the door open.
        //if (!canOpen) //if i open the door canOpen is false and the door should open
        //{
        //    switch (isOpen)
        //    {
        //        case false:
        //            if (doorRoutine == null)
        //            {
        //                doorRoutine = StartCoroutine(OpenDoorRoutine(isOpen));                    
        //            }
        //            break;
        //    }
        //}
        //rotValue = Mathf.Clamp(rotValue,0f, maxRotation);

        //if ( (rotValue <= -maxRotation) )
        //{

        //}



        //switch (isOpen)
        //{
        //    case true:
        //        if (rotValue < 0f)
        //        {
        //            rotValue = Mathf.Lerp(rotValue, 0, Time.deltaTime * lerpSpeed);
        //            transform.Rotate(Vector3.up, rotValue);
        //        }
        //        break;

        //    case false:
        //        if(rotValue > -maxRotation)
        //        {
        //            rotValue = Mathf.Lerp(rotValue, -maxRotation, Time.deltaTime * lerpSpeed);
        //            transform.Rotate(Vector3.up,rotValue);
        //        }
        //        break;
        //}
    }

    private IEnumerator OpenDoorRoutine()
    {
        while (time < 1)
        {

            rotValue = -maxRotation;
            transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, rotValue, 0), time);
            time += Time.deltaTime * lerpSpeed;
            yield return null;
        }

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
        Debug.Log("test");
    }

    public void Interact()
    {
        Debug.Log("Interact");
        if (canOpen)
            Opendoor();
    }

    private void Opendoor()
    {
        Debug.Log("Openning Door");
        
        if (doorRoutine == null)
        {
            if(canOpen && !isOpen)
            {
                canOpen = false;
                Debug.Log("OpenDoor Routine");
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
