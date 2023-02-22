// HelpBoxAttributeDrawer.cs - A custom property drawer for displaying HelpBoxAttribute in the Unity Editor.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate/
//
// This HelpBoxAttributeDrawer class is a custom property drawer that can be applied to a property in a MonoBehavior or ScriptableObject script
// marked with the HelpBoxAttribute. It displays the message and type of the attribute in a HelpBox style.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEditor;
using UnityEngine;

namespace UnityHelpers.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            if (attribute is not HelpBoxAttribute helpBoxAttribute) return base.GetHeight();

            GUIStyle style = GUI.skin.GetStyle("HelpBox");
            float height = style.CalcHeight(new GUIContent(helpBoxAttribute.Message), EditorGUIUtility.currentViewWidth);
            return height + style.margin.top + style.margin.bottom;
        }

        public override void OnGUI(Rect position)
        {
            if (attribute is not HelpBoxAttribute helpBoxAttribute) return;

            GUIStyle style = GUI.skin.GetStyle("HelpBox");
            position.x += style.margin.left;
            position.width -= style.margin.left + style.margin.right;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, helpBoxAttribute.Message, (MessageType)helpBoxAttribute.Type);
        }
    }
}