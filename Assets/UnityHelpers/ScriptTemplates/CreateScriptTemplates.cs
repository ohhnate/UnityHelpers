// CreateScriptTemplates.cs - A static class with menu items to create various types of code files in a Unity project.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This CreateScriptTemplates class provides easy-to-use menu items in the Unity Editor to create different types of code files,
// including MonoBehaviors, ScriptableObjects, custom attributes, property drawers, and custom editors.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEditor;

namespace UnityHelpers.ScriptTemplates
{
    public static class CreateScriptTemplates
    {
        [MenuItem("Assets/Create/Code/MonoBehavior", priority = 40)]
        public static void CreateMonoBehaviorMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/MonoBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewMono.cs");
        }
        
        [MenuItem("Assets/Create/Code/ScriptableObject", priority = 41)]
        public static void CreateScriptableObjectMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/ScriptableObject.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewSO.cs");
        }
        
        [MenuItem("Assets/Create/Code/CustomAttribute", priority = 42)]
        public static void CreateAttributeMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/Attribute.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewAttribute.cs");
        }
        
        [MenuItem("Assets/Create/Code/PropertyDrawer", priority = 43)]
        public static void CreatePropertyDrawerMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/PropertyDrawer.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewDrawer.cs");
        }
        
        [MenuItem("Assets/Create/Code/CustomEditor", priority = 44)]
        public static void CreateEditorMenuItem()
        {
            const string templatePath = "Assets/Scripts/Editor/Templates/Editor.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEditor.cs");
        }
    }
}