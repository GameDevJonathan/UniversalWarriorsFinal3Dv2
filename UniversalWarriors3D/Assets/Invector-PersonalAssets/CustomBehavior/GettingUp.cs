using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is a GettingUp decision", UnityEditor.MessageType.Info)]
#endif
    public class GettingUp : vStateDecision 
    {
		public override string categoryName
        {
            get { return "MyCustomDecisions/"; }
        }

        public override string defaultName
        {
            get { return "GettingUp"; }
        }

        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            //TO DO
            return true;
        }

        
        

       
    }
}
