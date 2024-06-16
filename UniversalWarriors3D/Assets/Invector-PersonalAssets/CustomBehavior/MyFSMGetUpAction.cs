using Unity.VisualScripting;
using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is a MyFSMGetUpAction Action", UnityEditor.MessageType.Info)]
#endif
   

    public class MyFSMGetUpAction : vStateAction
    {
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
            fsmBehaviour.aiController.HoldAnimation();
        }
    }

}
