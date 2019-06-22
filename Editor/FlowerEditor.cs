using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flower))]
public class FlowerEditor : Editor
{
    private Flower myPlant;
    float ssdMinLimit = 0;
    float ssdMaxLimit = 30;
    float easMinLimit = 0;
    float easMaxLimit = 15;

    private void OnEnable()
    {
        myPlant = (Flower)target;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.MinMaxSlider("Spawn Seed Delay", ref myPlant.ssdMin, ref myPlant.ssdMax, ssdMinLimit, ssdMaxLimit);
        EditorGUILayout.LabelField("\t\t\tMin Val:" + myPlant.ssdMin.ToString("0") + "   Max Val: " + myPlant.ssdMax.ToString("0"));

        EditorGUILayout.MinMaxSlider("Eject After Spawning", ref myPlant.easMin, ref myPlant.easMax, easMinLimit, easMaxLimit);
        EditorGUILayout.LabelField("\t\t\tMin Val:" + myPlant.easMin.ToString("0") + "   Max Val: " + myPlant.easMax.ToString("0"));

        /*
        var otherSerializedObj = new SerializedObject(myPlant);
        otherSerializedObj.ApplyModifiedProperties();*/

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myPlant);
        }

    }
}
