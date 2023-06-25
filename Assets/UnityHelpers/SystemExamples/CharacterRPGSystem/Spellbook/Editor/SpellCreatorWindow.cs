using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Spellbook;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Spells.Editor
{
    public class SpellCreationWindow : EditorWindow
    {
        private SpellType _spellTypeDisplay;
        private int _spellLevel = 1;
        private int _manaCost;
        private int _damageAmount;
        private int _healAmount;
        private List<Spellbook.Spellbook> _spellbooks;
        private string[] _spellbookNames;
        private int _selectedSpellbookIndex;
        private Spellbook.Spellbook SelectedSpellbook => _spellbooks.Count > _selectedSpellbookIndex ? _spellbooks[_selectedSpellbookIndex] : null;
        private bool _isDirty; // Flag to track if changes have been made

        [MenuItem("Window/Spell Creation")]
        public static void OpenWindow()
        {
            GetWindow<SpellCreationWindow>("Spell Creation");
        }

        private void OnEnable()
        {
            LoadSpellbooks();
        }

        private void LoadSpellbooks()
        {
            // Find all instances of Spellbook scriptable objects in the project
            string[] guids = AssetDatabase.FindAssets("t:Spellbook");
            _spellbooks = new List<Spellbook.Spellbook>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Spellbook.Spellbook spellbook = AssetDatabase.LoadAssetAtPath<Spellbook.Spellbook>(path);
                _spellbooks.Add(spellbook);
            }
            // Update the selected index if it's out of range
            if (_selectedSpellbookIndex >= _spellbooks.Count)
            {
                _selectedSpellbookIndex = Mathf.Clamp(_selectedSpellbookIndex, 0, _spellbooks.Count - 1);
            }
            _isDirty = true; // Mark as dirty after loading the spellbooks
        }

        private void OnGUI()
        {
            GUILayout.Label("Spell Creation", EditorStyles.boldLabel);

            if (_isDirty)
            {
                RefreshSpellbookNames(); // Refresh spellbook names if changes were made
                _isDirty = false; // Reset the dirty flag
            }
            _selectedSpellbookIndex = EditorGUILayout.Popup("Spellbook:", _selectedSpellbookIndex, _spellbookNames);
            _spellTypeDisplay = (SpellType)EditorGUILayout.EnumPopup("Spell Type:", _spellTypeDisplay); // Display enum selection
            _spellLevel = EditorGUILayout.IntField("Level Requirement:", _spellLevel);
            _manaCost = EditorGUILayout.IntField("Mana Cost:", _manaCost);
            _damageAmount = EditorGUILayout.IntField("Damage: ", _damageAmount);
            _healAmount = EditorGUILayout.IntField("Heal Amount: ", _healAmount);
            if (GUILayout.Button("Create Spell"))
            {
                CreateSpell();
            }
        }

        private void RefreshSpellbookNames()
        {
            string[] newSpellbookNames = _spellbooks.Select(sb => sb.DisplayName).ToArray();
            if (_selectedSpellbookIndex >= newSpellbookNames.Length)
            {
                _selectedSpellbookIndex = Mathf.Clamp(_selectedSpellbookIndex, 0, newSpellbookNames.Length - 1);
            }
            _spellbookNames = newSpellbookNames;
        }

        private void CreateSpell()
        {
            // Perform spell creation logic
            SpellType spellType = (SpellType)EditorGUILayout.EnumPopup("Spell Type:", _spellTypeDisplay);
            Spell newSpell = new Spell(spellType, _spellLevel, _manaCost, _damageAmount, _healAmount);
            // Add the new spell to the selected spellbook or any other data structure
            SelectedSpellbook.AddSpell(newSpell);
            Debug.Log("Spell created: " + newSpell);
        }
    }
}