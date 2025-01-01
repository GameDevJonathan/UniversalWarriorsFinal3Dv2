using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine;


public class Targeter : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private List<Target> targets = new List<Target>();
    [SerializeField] public Target CurrentTarget;
    [SerializeField] public Target QuickTarget;

    [SerializeField] public int index = 0;
    [SerializeField] private bool didCycle;
    [SerializeField] private PlayerStateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo anim = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        if (!stateMachine.Animator.IsInTransition(0)
            && (anim.IsTag("Attack") || anim.IsTag("TakeDown"))
            && QuickTarget != null)
        {
            if (anim.normalizedTime > 1)
                QuickTarget = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
        //target.OnDestroyed += RemoveTarget;
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Remove(target);
        //stateMachine.InputReader.Targeting = false;
        //RemoveTarget(target);
    }

    public void SelectClosestTarget()
    {
        //if (CurrentTarget != null) return;
        //if (targets.Count == 0) return;
        //float closestDistance = Mathf.Infinity;
        //Target closestTarget = null;

        //foreach(Target target in targets)
        //{
        //    float currentDistance;
        //    currentDistance = Vector3.Distance(transform.position, target.transform.position);

        //    if(currentDistance < closestDistance)
        //    {
        //        closestDistance = currentDistance;
        //        closestTarget = target;
        //    }
        //}

        //QuickTarget = closestTarget;
        //Vector3 lookPos = QuickTarget.transform.position - stateMachine.transform.position;
        ////Debug.Log($"look position {lookPos}");
        //lookPos.y = 0f;

        //float distance = Vector3.Distance(QuickTarget.transform.position, stateMachine.transform.position);
        ////Debug.Log($"Distance {distance}");
        //if(distance < 3f)
        //    stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }





    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            targetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public bool SelectTarget()
    {


        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets)
        {
            Debug.Log("Target: " + target);
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        //Debug.Log("Targeter Component:: " + closestTarget);
        if (closestTarget == null) 
        { 
            return false;
        }
        else
        {
            Debug.Log(closestTarget);
        }
        //if (closestTarget != null)
        //{ Debug.Log("We have a target " + closestTarget); }
        //else
        //    return false;


        CurrentTarget = closestTarget;
        Debug.Log("current target transform " + CurrentTarget.transform);


        targetGroup.AddMember(CurrentTarget.transform,.1f,2f);

        //Debug.Log("Targeter Component:: CurrentTarget =  " + CurrentTarget);
        //Debug.Log("Type of  =  " + CurrentTarget);

        //if (CurrentTarget)
        //{
        //    Debug.Log("Targeter Component:: if statement hit here ");

        //    targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        //    //switch (CurrentTarget.type)
        //    //{
        //    //    case Target.Type.small:
        //    //        targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        //    //        break;
        //    //    case Target.Type.large:
        //    //        targetGroup.AddMember(CurrentTarget.transform, .25f, 2f);
        //    //        break;
        //    //}

        //}


        return true;
    }

    public void CycleTarget()
    {
        if (targets.Count == 0) { return; }
        if (CurrentTarget == null) { return; }

        //Debug.Log("Target Count: " + targets.Count);
        //Debug.Log("Current Target: " + CurrentTarget.name);

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].transform.position == CurrentTarget.transform.position)
            {
                index = i;
            }
        }


        if (stateMachine.InputReader.SelectionValue.x >= .9f)
        {

            if (index == 0 && didCycle == false)
            {
                Debug.Log("Running this block index >= targets.count");
                didCycle = true;
                index = targets.Count - 1;
                CurrentTarget = targets[index];

            }

            if (index <= targets.Count && didCycle == false)
            {
                didCycle = true;
                index -= 1;
                CurrentTarget = targets[index];
            }


        }

        if (stateMachine.InputReader.SelectionValue.x <= -.9f)
        {
            if (index >= targets.Count - 1 && didCycle == false)
            {
                Debug.Log($"Index: {index}");
                Debug.Log($"Target.Count: {targets.Count}");
                Debug.Log("Running this block index >= targets.count");
                didCycle = true;
                index = 0;
                CurrentTarget = targets[index];

            }

            if (index < targets.Count && didCycle == false)
            {
                didCycle = true;
                index += 1;
                CurrentTarget = targets[index];
            }

        }

        if (stateMachine.InputReader.SelectionValue.x == 0 && didCycle)
            didCycle = false;

    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }
        targetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
}
