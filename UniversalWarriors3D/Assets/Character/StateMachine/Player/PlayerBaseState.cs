using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }



    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharacterController.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceMovement(Vector3 movement, float deltatime)
    {
        stateMachine.transform.rotation =
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltatime * stateMachine.RotationSmoothValue);
    }

    protected void FaceTarget()
    {

        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation =
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(lookPos),
            Time.deltaTime * stateMachine.RotationSmoothValue);
    }

    protected void FaceTakeDownTarget()
    {
        if (stateMachine.Targeter.TakeDownTarget == null) { return; }
        Vector3 lookPos = stateMachine.Targeter.TakeDownTarget.transform.position - stateMachine.transform.position;
        //Debug.Log($"look position {lookPos}");
        lookPos.y = 0f;

        float distance = Vector3.Distance(stateMachine.Targeter.TakeDownTarget.transform.position, stateMachine.transform.position);
        //Debug.Log($"Distance {distance}");        
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }


    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new Grounded(stateMachine, true));
        if (stateMachine.Animator.applyRootMotion)
        {
            stateMachine.Animator.applyRootMotion = false;
        }
    }

    public float Angle(float xValue, float yValue)
    {

        // Get left stick input (assuming Xbox controller or similar)
        float x = xValue; // Left stick X-axis
        float y = yValue;   // Left stick Y-axis
        float angle = 0f;

        if (!stateMachine.InputReader.Targeting)
        {

            if (stateMachine.InputReader.MovementValue != Vector2.zero)
                angle = 1f;

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
                angle = 136f;
        }


        // Check if the stick is being moved
        if ((x != 0 || y != 0) && stateMachine.InputReader.Targeting)
        {
            Debug.Log("targeting");

            // Calculate angle in radians and convert to degrees
            angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

            // Convert to 360-degree system (0° at up, clockwise)
            if (angle < 0) angle += 360;

            //Debug.Log("Left Stick Angle: " + angle);
        }
        Debug.Log("Angle " + angle);
        return angle;
    }
}
