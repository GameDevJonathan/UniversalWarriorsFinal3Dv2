#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace FS_CombatSystem
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(MeleeFighter))]
    public class MeleeFighterEditor : Editor
    {
        public SerializedProperty onAttackEvent;
        public SerializedProperty onGotHitEvent;
        public SerializedProperty OnWeaponEquipEvent;
        public SerializedProperty OnWeaponUnEquipEvent;
        public SerializedProperty OnCounterMisusedEvent;
        public SerializedProperty OnKnockDownEvent;
        public SerializedProperty OnGettingUpEvent;
        public SerializedProperty OnDeathEvent;

        bool eventFoldOut;

        private void OnEnable()
        {
            onAttackEvent = serializedObject.FindProperty("OnAttackEvent");
            onGotHitEvent = serializedObject.FindProperty("OnGotHitEvent");
            OnWeaponEquipEvent = serializedObject.FindProperty("OnWeaponEquipEvent");
            OnWeaponUnEquipEvent = serializedObject.FindProperty("OnWeaponUnEquipEvent");
            OnCounterMisusedEvent = serializedObject.FindProperty("OnCounterMisusedEvent");
            OnDeathEvent = serializedObject.FindProperty("OnDeathEvent");
            OnKnockDownEvent = serializedObject.FindProperty("OnKnockDownEvent");
            OnGettingUpEvent = serializedObject.FindProperty("OnGettingUpEvent");
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(5f);

            serializedObject.Update();

            eventFoldOut = EditorGUILayout.Foldout(eventFoldOut, "Events");
            if (eventFoldOut)
            {
                EditorGUILayout.PropertyField(onAttackEvent);
                EditorGUILayout.PropertyField(onGotHitEvent);
                EditorGUILayout.PropertyField(OnWeaponEquipEvent);
                EditorGUILayout.PropertyField(OnWeaponUnEquipEvent);
                EditorGUILayout.PropertyField(OnCounterMisusedEvent);
                EditorGUILayout.PropertyField(OnKnockDownEvent);
                EditorGUILayout.PropertyField(OnGettingUpEvent);
                EditorGUILayout.PropertyField(OnDeathEvent);
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
#endif