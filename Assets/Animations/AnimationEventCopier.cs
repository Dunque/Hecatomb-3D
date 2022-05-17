using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
public class AnimationEventCopier : EditorWindow
{
    private AnimationClip sourceObject;
    private AnimationClip targetObject;
    static void Init()
    {
        GetWindow(typeof(AnimationEventCopier));
    }
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        sourceObject = EditorGUILayout.ObjectField("Source", sourceObject, typeof(AnimationClip), true) as AnimationClip;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        targetObject = EditorGUILayout.ObjectField("Target", targetObject, typeof(AnimationClip), true) as AnimationClip;
        EditorGUILayout.EndHorizontal();

        if (sourceObject != null &&  targetObject != null)
            {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy"))
                CopyData();
            EditorGUILayout.EndHorizontal();
        }
    }

    void CopyData()
    {
        Undo.RegisterCompleteObjectUndo(targetObject, "Undo Generic Copy");

        AnimationClip sourceAnimClip = sourceObject as AnimationClip;
        AnimationClip targetAnimClip = targetObject as AnimationClip;

        if (sourceAnimClip && targetAnimClip)
            {

            AnimationUtility.SetAnimationEvents(targetAnimClip, AnimationUtility.GetAnimationEvents(sourceAnimClip));
        }
    }
}
#endif
