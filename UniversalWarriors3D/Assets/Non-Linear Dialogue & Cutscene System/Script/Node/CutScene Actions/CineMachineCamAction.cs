#if cinemachine
using Cinemachine;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
 

namespace FC_CutsceneSystem
{
    public class CineMachineCamAction : CutSceneAction
    {
#if cinemachine
        CinemachineBrain mainCamera;
        public CinemachineVirtualCameraBase newCinemachineCam;
#endif
        public float blendTime = 2f;
        public bool foldout;
        public CineMachineCamAction(CineMachineCamAction cineMachineCam = null)
        {
            actionType = ActionType.Cinemachine;
#if cinemachine
           if (GameObject.FindObjectOfType<CutsceneEditorManager>().mainCinemachineBrain != null)
                mainCamera = GameObject.FindObjectOfType<CutsceneEditorManager>().mainCinemachineBrain;
#if UNITY_EDITOR
            if (cineMachineCam != null)
                EditorUtility.CopySerializedManagedFieldsOnly(cineMachineCam, this);
#endif
#endif
        }

#if cinemachine
        public override IEnumerator ExecuteAction()
        {
            if (mainCamera == null && GameObject.FindObjectOfType<CutsceneEditorManager>().mainCinemachineBrain != null)
                mainCamera = GameObject.FindObjectOfType<CutsceneEditorManager>().mainCinemachineBrain;
            if (mainCamera == null)
                mainCamera = GameObject.FindObjectOfType<CinemachineBrain>();
            if (!ActionValidationWhilePlaying(mainCamera) || !ActionValidationWhilePlaying(newCinemachineCam))
                yield break;
            mainCamera.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
            newCinemachineCam.gameObject.SetActive(true);

            mainCamera.m_DefaultBlend.m_Time = blendTime;
            yield return new WaitForSeconds(blendTime);

            yield break;
        }
#endif

#if UNITY_EDITOR

        public override void CustomEditor(CutSceneNode cutSceneNode, CutsceneGraph graph, List<CutSceneNode> selectedNodes = null)
        {
#if cinemachine
            var node = cutSceneNode.currentAction as CineMachineCamAction;
            List<CineMachineCamAction> actions = new List<CineMachineCamAction>() { node };
            if (selectedNodes != null)
            {
                actions = selectedNodes.Select(n => n.currentAction as CineMachineCamAction).ToList();
                var firstNode = actions.First();
                node.newCinemachineCam = (CinemachineVirtualCameraBase)GetFieldValue(actions.All(n => n.newCinemachineCam == firstNode.newCinemachineCam), null, firstNode.newCinemachineCam);
                node.foldout = (bool)GetFieldValue(actions.All(n => n.foldout == firstNode.foldout), false, firstNode.foldout);
                node.blendTime = (float)GetFieldValue(actions.All(n => n.blendTime == firstNode.blendTime), 0f, firstNode.blendTime);
            }

            var newCinemachineCam = (CinemachineVirtualCameraBase)EditorGUILayout.ObjectField("New CineCam", node.newCinemachineCam, typeof(CinemachineVirtualCameraBase), true);
            if (newCinemachineCam != node.newCinemachineCam)
            {
                UndoGraph(graph);
                foreach (var n in actions)
                    n.newCinemachineCam = newCinemachineCam;
            }
            ValidationWarning(node.newCinemachineCam, "New CineCam not assigned", graph);
            GUILayout.Space(5);

            var foldout = EditorGUILayout.Foldout(node.foldout, "Additional");
            if (foldout != node.foldout)
            {
                UndoGraph(graph);
                foreach (var n in actions)
                    n.foldout = foldout;
            }
            if (node.foldout)
            {
                GUILayout.Space(5);
                EditorGUI.indentLevel++;
                var blendTime = EditorGUILayout.FloatField("Blend Time", node.blendTime);
                if (blendTime != node.blendTime)
                {
                    UndoGraph(graph);
                    foreach (var n in actions)
                        n.blendTime = blendTime;
                }
                EditorGUI.indentLevel--;
            }
#else
            EditorGUILayout.HelpBox("Please enable cinemachine from the welcome window", MessageType.Error);

#endif

        }
#if cinemachine
        public override bool Validation()
        {
            return (newCinemachineCam == null);
        }
#endif
#endif
    }
}
