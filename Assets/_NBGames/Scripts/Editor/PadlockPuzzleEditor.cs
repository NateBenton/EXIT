using System.Collections;
using System.Collections.Generic;
using _NBGames.Scripts.InteractionBehaviors;
using _NBGames.Scripts.Inventory.Classes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(PadlockPuzzle))]
public class PadlockPuzzleEditor : Editor
{
    private float _degreeCount;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var padlockPuzzle = (PadlockPuzzle) target;
        
        if (padlockPuzzle.Combination.Length != padlockPuzzle.PadlockWheels.Length)
        {
            EditorGUILayout.HelpBox("Combo size must match wheel count!", MessageType.Error);
        }

        if (padlockPuzzle.WheelLights.Length != padlockPuzzle.PadlockWheels.Length)
        {
            EditorGUILayout.HelpBox("There must be as many wheel lights as there are wheels!", MessageType.Error);
        }

        if (padlockPuzzle.PadlockWheels.Length != 0)
        {
            if (GUILayout.Button("Randomize Starting Values"))
            {
                foreach (var wheel in padlockPuzzle.PadlockWheels)
                {
                    if (wheel == null) return;
                    wheel.StartingNumber = Random.Range(0, 9);
                
                    UpdateWheelRotations(padlockPuzzle.PadlockWheels);
                }
            }
        }
        
        EditorGUI.BeginChangeCheck();
        {
            for (var i = 0; i < padlockPuzzle.PadlockWheels.Length; i++)
            {
                if (padlockPuzzle.PadlockWheels[i] == null) return;
                padlockPuzzle.PadlockWheels[i].StartingNumber =
                    EditorGUILayout.IntSlider($"Starting #{i + 1}:", padlockPuzzle.PadlockWheels[i].StartingNumber, 0, 9);
            }
        }

        if (!EditorGUI.EndChangeCheck()) return;
        
        UpdateWheelRotations(padlockPuzzle.PadlockWheels);
        
    }

    private void UpdateWheelRotations(PadlockWheel[] padlockWheels)
    {
        foreach (var wheel in padlockWheels)
        {
            _degreeCount = 0f;
            for (var i = 0; i < wheel.StartingNumber; i++)
            {
                _degreeCount += 36f;
            }
                
            wheel.gameObject.transform.localEulerAngles = new Vector3(-_degreeCount, 0f, 0f);
        }
    }
}
