using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is a MyFSMGetUpDecision decision", UnityEditor.MessageType.Info)]
#endif
    public class MyFSMGetUpDecision : vStateDecision
    {
		public override string categoryName
        {
            get { return "MyCustomDecisions/"; }
        }

        public override string defaultName
        {
            get { return "MyFSMGetUpDecision"; }
        }

        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            //TO DO
            return fsmBehaviour.aiController._holdAnim;
        }
    }
}
