using UnityEngine;

public abstract class State 
{
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tag, int slot = 0)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(slot);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(slot);

        if (animator.IsInTransition(slot) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(slot) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }

}
