using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetScanner
{
    public bool isDrawing = false;
    public float detectionRadius = 10f;
    public float followRadius = 7f;
   
    public GameObject Detect(Transform detector)
    {
        GameObject closedTarget = null;
        return closedTarget;
    }

#if UNITY_EDITOR

    public void EditorGizmo(Transform transform)
    {
        Color c_detect = new Color(0.5f, 0.7f, 0.0f, 0.4f);
        Color c_Follow = new Color(0.2f, 0.5f, 0f, 0.4f);

        UnityEditor.Handles.color = c_detect;
        Vector3 rotatedForward = Quaternion.Euler(0, -360f * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, 360f, detectionRadius);

        UnityEditor.Handles.color = c_Follow;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, 360f, followRadius);  
    }

#endif
}