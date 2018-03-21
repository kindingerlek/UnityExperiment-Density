using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BouyanceObject))]
public class BouyanceObjectEditor : Editor {
    SerializedProperty calcTypeProp;
    SerializedProperty densityProp;

    void OnEnable()
    {
        densityProp = serializedObject.FindProperty("density");
        calcTypeProp = serializedObject.FindProperty("autoCalculate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(calcTypeProp);
        
        if (calcTypeProp.enumValueIndex != (int)BouyanceObject.CalculateType.Density)
            EditorGUILayout.PropertyField(densityProp);

        serializedObject.ApplyModifiedProperties();
    }
    
}
