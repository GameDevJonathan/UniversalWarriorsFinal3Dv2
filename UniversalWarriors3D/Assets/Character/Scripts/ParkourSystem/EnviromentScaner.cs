using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class EnviromentScaner : MonoBehaviour
{

    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, .5f, 0);
    [SerializeField] float forwardRayLength = 0.2f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] float ledgeRayLength = 10f;
    [SerializeField] float ledgeheightThreshold = 0.75f;
    [SerializeField] public float height;
    [SerializeField] float originoffset = 0.5f;
    [SerializeField] Vector3 Direction;
    [SerializeField] LayerMask obstacleLayer;

    public ObstacleHitData ObstacleCheck()
    {
        var hitdata = new ObstacleHitData();
        var forwardOrigin = transform.position + forwardRayOffset;

        hitdata.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward, out hitdata.forwardHit,
             forwardRayLength, obstacleLayer);

        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, (hitdata.forwardHitFound) ? Color.red : Color.white);



        if (hitdata.forwardHitFound)
        {
            var heightOrigin = hitdata.forwardHit.point + Vector3.up * heightRayLength;
            hitdata.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitdata.heightHit, heightRayLength, obstacleLayer);
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitdata.heightHitFound) ? Color.red : Color.white);

        }



        return hitdata;
    }

    public bool LedgeCheck(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero)
            return false;

        var origin = transform.position + moveDir * originoffset + Vector3.up;
        Direction = moveDir;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer))
        {
            height = transform.position.y - hit.point.y;

            Debug.Log($"height distance: {height}");

            if (height > ledgeheightThreshold)
            {
                return true;
            }

        }


        return false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            var origin = transform.position + Direction * originoffset + Vector3.up;
            Debug.DrawRay(origin, Vector3.down * ledgeRayLength, Color.green);

        }

    }
}




public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;

}