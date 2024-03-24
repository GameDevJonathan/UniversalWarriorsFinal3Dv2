using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is a MyFSMGetUpAction Action", UnityEditor.MessageType.Info)]
#endif
   

    public class MyFSMGetUpAction : vStateAction
    {
        bool knockdown = false;
        public float time = 3f;
       


        public override string categoryName
        {
            get { return "MyCustomActions/"; }
        }
        public override string defaultName
        {
            get { return "MyFSMGetUpAction"; }
        }

        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType executionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            if (fsmBehaviour == null) return;
            if (fsmBehaviour.aiController.isDead) return;
            time = Mathf.Clamp(time, 0, 5);

            if (!knockdown) knockdown = !knockdown;

            if (knockdown)
            {
                if (time > 0f)
                {
                    time -= Time.deltaTime;
                }
                Debug.Log(time);
            }

        }
    }

}
