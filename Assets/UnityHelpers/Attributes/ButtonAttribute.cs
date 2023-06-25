// ButtonAttribute.cs - A custom method attribute for adding buttons to the Unity Inspector.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This ButtonAttribute class is a custom method attribute that can be applied to a method in a MonoBehavior or ScriptableObject script
// to add a button to the Unity Inspector. The label for the button can be specified as an argument to the constructor of this class.
// When the button is clicked, the method marked with this attribute will be invoked.
//
// No accreditation is required but it would be highly appreciated <3

using System;

namespace UnityHelpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public readonly string Label;

        /// <summary>
        /// Creates a new instance of the ButtonAttribute without a label
        /// </summary>
        public ButtonAttribute()
        {
            Label = null;
        }

        /// <summary>
        /// Creates a new instance of the ButtonAttribute with a label
        /// </summary>
        /// <param name="label">The label to display on the button</param>
        public ButtonAttribute(string label)
        {
            Label = label;
        }
    }
}