// PopupAttribute.cs - A custom property attribute for displaying a popup menu in the Unity Inspector.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This PopupAttribute class is a custom property attribute that can be applied to a field in a MonoBehavior or ScriptableObject script
// to display a popup menu in the Unity Inspector. The menu options are specified as arguments to the constructor of this class.
//
// No accreditation is required but it would be highly appreciated <3

using System;
using UnityEngine;

namespace UnityHelpers.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PopupAttribute : PropertyAttribute
    {
        public readonly object[] Options;

        public PopupAttribute(params object[] options)
        {
            Options = options;
        }
    }
}