using UnityEngine;



public enum AlertStage
{
    Peaceful,
    Intrigued,
    Alerted
}


public class EnemyPlayerDetector : MonoBehaviour
{
    public float fov;
    [Range(0, 360)] public float fovAngle; //in degrees

    public float engageRange;
    public float attackRange;

    public AlertStage alertStage;
    [Range(0, 100)] public float alertLevel; //0: Peaceful, 100: Alerted
    public bool playerInFov = false;
    public bool playerInEngageRange = false;
    public bool playerInAttackRange = false;

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
    }

    private void Update()
    {
        Collider[] targetsInFov = Physics.OverlapSphere(transform.position, fov);
        Collider[] targetsInEngageRange = Physics.OverlapSphere(transform.position, engageRange);
        Collider[] targetsInAttackRange = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider c in targetsInFov)
        {
            if (c.CompareTag("Player"))
            {
                float signedAngle = Vector3.Angle(
                    transform.forward,
                    c.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle / 2)
                    playerInFov = true;
                break;
            }

            playerInFov = false;
        }

        foreach (Collider c in targetsInEngageRange)
        {
            if (c.CompareTag("Player"))
            {
                playerInEngageRange = true;
                break;
            }

            playerInEngageRange = false;
        }

        foreach (Collider c in targetsInAttackRange)
        {
            if (c.CompareTag("Player"))
            {
                playerInAttackRange = true;
                break;
            }

            playerInAttackRange = false;
        }

        _UpdateAlertState(playerInFov);
    }

    private void _UpdateAlertState(bool playerInFov)
    {
        switch (alertStage)
        {
            case AlertStage.Peaceful:
                if (playerInFov)
                    alertStage = AlertStage.Intrigued;
                break;

            case AlertStage.Intrigued:
                if (playerInFov)
                {
                    alertLevel++;
                    if (alertLevel >= 100)
                        alertStage = AlertStage.Alerted;
                }
                else
                {
                    alertLevel--;
                    if (alertLevel <= 0)
                        alertStage = AlertStage.Peaceful;
                }
                break;

            case AlertStage.Alerted:
                if (!playerInFov)
                {
                    alertStage = AlertStage.Intrigued;
                }
                break;
        }
    }
}
