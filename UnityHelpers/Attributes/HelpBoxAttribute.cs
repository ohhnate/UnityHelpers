// HelpBoxAttribute.cs - A custom property attribute for displaying help boxes in the Unity Inspector.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This HelpBoxAttribute class is a custom property attribute that can be applied to a field or property in a MonoBehavior or ScriptableObject script
// to display a help box with a message in the Unity Inspector. The message and message type (Info, Warning, or Error) are specified as arguments
// to the constructor of this class. This attribute can be helpful in providing additional context or guidance to users editing properties
// in the Unity Inspector.
//
// No accreditation is required but it would be highly appreciated <3

using System;
using UnityEngine;

namespace UnityHelpers.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public string Message { get; private set; }
        public MessageType Type { get; private set; }

        public HelpBoxAttribute(string message, MessageType type = MessageType.Info)
        {
            Message = message;
            Type = type;
        }

        //unity editor already has the same enum as this but made a copy of it here so this can be decoupled from the
        //unity editor assembly when it the project is built.
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error,
        }
    }
}