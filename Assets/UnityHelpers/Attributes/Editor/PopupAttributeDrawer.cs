// PopupAttributeDrawer.cs - A custom property drawer for displaying popup menus in the Unity Editor.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate/
//
// This PopupAttributeDrawer class is a custom property drawer that can be applied to an integer, float, or string serialized property
// in a MonoBehavior or ScriptableObject script to display a popup menu in the Unity Editor. The options for the menu can be specified
// as an array of objects in the PopupAttribute applied to the property. When an option is selected, the corresponding value will be
// assigned to the property.
//
// No accreditation is required but it would be highly appreciated <3

using System;
using UnityEditor;
using UnityEngine;

namespace UnityHelpers.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    DrawIntPopup(position, property, label);
                    break;
                case SerializedPropertyType.Float:
                    DrawFloatPopup(position, property, label);
                    break;
                case SerializedPropertyType.String:
                    DrawStringPopup(position, property, label);
                    break;
                default:
                    DrawUnsupportedField(position, property, label);
                    break;
            }
        }

        private static void DrawIntPopup(Rect position, SerializedProperty property, GUIContent label)
        {
            int selectedIndex = -1;
            int[] optionValues = new int[property.enumNames.Length];
            for (int i = 0; i < optionValues.Length; i++)
            {
                optionValues[i] = i;
                if (property.enumValueIndex == i)
                {
                    selectedIndex = i;
                }
            }
            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUI.IntPopup(position, label.text, selectedIndex, property.enumNames, optionValues);
            if (EditorGUI.EndChangeCheck())
            {
                property.enumValueIndex = selectedIndex;
            }
        }

        private void DrawFloatPopup(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is not PopupAttribute popupAttribute) return;
            
            object[] options = popupAttribute.Options;
            string[] optionLabels = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                optionLabels[i] = options[i].ToString();
            }
            float value = property.floatValue;
            int index = Mathf.Max(Array.IndexOf(options, value), 0);
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(position, label.text, index, optionLabels);
            if (EditorGUI.EndChangeCheck())
            {
                property.floatValue = (float)options[index];
            }
        }

        private void DrawStringPopup(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is not PopupAttribute popupAttribute) return;

            object[] options = popupAttribute.Options;
            string[] optionLabels = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                optionLabels[i] = options[i].ToString();
            }
            string value = property.stringValue;
            int index = Mathf.Max(Array.IndexOf(options, value), 0);
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(position, label.text, index, optionLabels);
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = (string)options[index];
            }
        }

        private static void DrawUnsupportedField(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, label.text, "PopupAttribute doesn't support the " + property.propertyType + " property type.");
        }
    }
}