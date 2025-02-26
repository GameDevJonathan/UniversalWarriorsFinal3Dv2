using UnityEngine;

public class EnemyEngageState : EnemyBaseState
{
    //private readonly int StrafeHash = Animator.StringToHash("StrafeBlend");
    private readonly int StrafeHash = Animator.StringToHash("Strafe Movement");
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float crossFadeTime = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int randomChance;
    private bool prepareToAttack = false;
    private Vector3 moveDirection;
    private float strafeTime;
    private float strafeRange;
    private float attackTime;
    Vector3 finalDir = Vector3.zero;
    Vector3 moveDir = Vector3.zero;
    private float circleSpeed = 2f;
    private float strafeDistance = 5f;
    private int randomDir;

    //constructor
    public EnemyEngageState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Engage State");

        strafeTime = RandomStrafeTime();
        attackTime = BossAttackTime();
        stateMachine.Animator.SetFloat("Speed", 0f);
        //stateMachine.Animator.SetBool("isStrafing", true);
        stateMachine.Animator.SetFloat("StrafingX", 0);
        stateMachine.Animator.SetFloat("StrafingY", 0);
        stateMachine.Animator.CrossFadeInFixedTime(StrafeHash, crossFadeTime);
        randomChance = Random.Range(0, 2);
        randomDir = Random.Range(0, 2) == 1 ? 1 : -1; //Random left or Right



    }
    public override void Tick(float deltaTime)
    {
        if ((stateMachine.PlayerDetector.playerInFov && !stateMachine.PlayerDetector.playerInEngageRange))
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        FacePlayer();


        BossMovement(deltaTime);
        StarfeLogic(deltaTime);
        UpdateAnimations(deltaTime);
    }


    private void StarfeLogic(float deltaTime)
    {

        if (prepareToAttack)
        {
            stateMachine.Animator.Play(LocomotionHash);
            stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);

            stateMachine.Agent.enabled = true;
            return;
        }
        else
        {
            stateMachine.Agent.enabled = false;
        }

        if (stateMachine.PlayerDetector.playerInEngageRange /*&& Vector3.Distance(stateMachine.Player.transform.position, stateMachine.transform.position) <= 6.5f*/)
        {

            if (strafeTime > 0f)
            {
                strafeTime -= Time.deltaTime * .75f;

                // Calculate circling position
                Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
                Vector3 perpendicularDir = Quaternion.AngleAxis(90 * randomDir, Vector3.up) * directionToPlayer;

                Vector3 targetPosition = stateMachine.Player.transform.position - (directionToPlayer * strafeDistance) + (perpendicularDir * strafeDistance);

                // Smooth movement towards the calculated position
                stateMachine.transform.position = Vector3.Lerp(stateMachine.transform.position, targetPosition, deltaTime * stateMachine.StrafeMovementSpeed);

                // Rotate around the player smoothly
                stateMachine.transform.RotateAround(stateMachine.Player.transform.position, Vector3.up, circleSpeed * randomDir * deltaTime);

                // Update movement direction for animations
                moveDirection = stateMachine.transform.position - stateMachine.Player.transform.position;
            }
            else
            {


                randomChance = Random.Range(0, 2);
                strafeTime = RandomStrafeTime();
                randomDir = Random.Range(0, 2) != 1 ? 1 : -1;
            }

            Move(moveDir, deltaTime);

        }
    }

    public override void Exit()
    {
        Debug.Log("Leaving Chasing State");

        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
        //stateMachine.Animator.SetBool("isStrafing", false);
        prepareToAttack = false;
        strafeTime = 0;
    }



    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.CharacterController.velocity;
    }

    private void UpdateAnimations(float deltaTime)
    {
        if (prepareToAttack)
        {
            stateMachine.Animator.SetFloat("StrafingX", 0);
            //stateMachine.Animator.SetFloat("StrafingY", y, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat("StrafingY", 0);
            return;
        }
        Vector3 localMoveDir = stateMachine.transform.InverseTransformDirection(moveDirection);
        float x = Mathf.Clamp(localMoveDir.x, -1f, 1f);
        float y = Mathf.Clamp(localMoveDir.z, -1f, 1f);

        //stateMachine.Animator.SetFloat("StrafingX", x, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat("StrafingX", x);
        //stateMachine.Animator.SetFloat("StrafingY", y, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat("StrafingY", y);
    }

    private float RandomStrafeTime()
    {
        float time;
        if (stateMachine.transform.CompareTag("Boss"))
            time = Random.Range(1f, 3f);
        else
            time = Random.Range(3f, 6f);

        return time;
    }

    private float BossAttackTime()
    {
        float bossTime;
        bossTime = Random.Range(1f, 3f);


        Debug.Log("Boss Time: " + bossTime);
        return bossTime;
    }

    private void BossMovement(float deltaTime)
    {
        if (!stateMachine.transform.CompareTag("Boss")) return;


        if (attackTime > 0f && !prepareToAttack)
        {
            attackTime -= Time.deltaTime * .75f;
        }

        if (attackTime <= 0f && !prepareToAttack)
        {
            prepareToAttack = true;
            attackTime = 0f;
            return;
        }

        if (prepareToAttack)
        {
            Debug.Log("Engaging Player");
            MoveToPlayer(deltaTime);
            //stateMachine.Animator.SetFloat(SpeedHash, 1f);


            if (stateMachine.PlayerDetector.playerInAttackRange)
            {
                Debug.Log("Going into Attacking State");
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 0));
                return;
            }
        }

    }






}
