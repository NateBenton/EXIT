using _NBGames.Scripts.InteractionBehaviors;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _NBGames.Scripts.Editor
{
    [CustomEditor(typeof(DrawerOpen))]
    
    public class DrawerOpenEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var drawer = (DrawerOpen) target;
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Set opened position to current"))
                {
                    drawer.OpenPosition = drawer.transform.position;
                }
            
                if (GUILayout.Button("Set closed position to current"))
                {
                    drawer.ClosedPosition = drawer.transform.position;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Open Drawer"))
                {
                    drawer.transform.position = drawer.OpenPosition;
                }

                if (GUILayout.Button("Close Drawer"))
                {
                    drawer.transform.position = drawer.ClosedPosition;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
