// ButtonAttributeEditor.cs - A custom editor script for Unity that allows for methods marked with ButtonAttribute to be
// displayed as buttons in the inspector.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate/
//
// This custom editor script extends the Unity Editor to display buttons in the inspector for methods marked with the
// ButtonAttribute. The label on the button can be specified using the Label property of the ButtonAttribute. If no
// Label is provided, the name of the method is used as the label. The custom editor script also refreshes all editor
// windows when scripts are reloaded to ensure that changes to the buttons are reflected in the inspector.
//
// No accreditation is required but it would be highly appreciated <3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityHelpers.Attributes.Editor
{
    public class ButtonAttributeEditor : UnityEditor.Editor
    {
        [CustomEditor(typeof(MonoBehaviour), true)]
        public class ButtonDrawer : UnityEditor.Editor
        {
            [UnityEditor.Callbacks.DidReloadScripts]
            private static void OnScriptsReloaded()
            {
                // Refresh all editor windows when scripts are reloaded, otherwise changes might not be reflected in 
                // the inspector until the user adjusts values on the inspector that forces a repaint.
                foreach (EditorWindow editorWindow in Resources.FindObjectsOfTypeAll<EditorWindow>())
                {
                    editorWindow.Repaint();
                }
            }
        
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                MonoBehaviour monoBehaviour = target as MonoBehaviour;
                if (monoBehaviour == null) return;
                IEnumerable<MethodInfo> methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(info => Attribute.IsDefined(info, typeof(ButtonAttribute)));
                foreach (MethodInfo method in methods)
                {
                    // Get the ButtonAttribute instance for the method
                    ButtonAttribute buttonAttribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                    // Use the label from the attribute if available, otherwise use the method name
                    string buttonText = string.IsNullOrEmpty(buttonAttribute.Label) ? method.Name : buttonAttribute.Label;
                    if (GUILayout.Button(buttonText))
                    {
                        method.Invoke(monoBehaviour, null);
                    }
                }
            }
        }
    }
}