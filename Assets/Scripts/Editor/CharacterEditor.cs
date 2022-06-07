using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Characters.Editor
{
    public class CharacterEditor : EditorWindow
    {
        [SerializeField]
        Character selectedCharacter;

        [MenuItem("Window/RPG/Character Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(CharacterEditor), false, "Character Editor", false);
        }

        public void ShowEditorWindow(Character c)
        {
            GetWindow(typeof(CharacterEditor), false, "Character Editor", false);
            selectedCharacter = c;
            Repaint();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Character character = EditorUtility.InstanceIDToObject(instanceID) as Character;
            if (character != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            Character character = Selection.activeObject as Character;
            if (character != null)
            {
                // Character atual ainda não foi salvo
                if (selectedCharacter != null && AssetDatabase.GetAssetPath(selectedCharacter) == "")
                {
                    switch (EditorUtility.DisplayDialogComplex("Alerta", "Character atual ainda não foi salvo!", "Salvar", "Cancelar", "Não salvar"))
                    {
                        case 0:
                            SaveCharacter(selectedCharacter);
                            break;
                        case 1: break;
                        case 2: return;
                    }
                }

                selectedCharacter = character;
                Repaint();
            }
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperCenter;
            style.normal.textColor = Color.white;

            EditorGUILayout.BeginVertical(style);
            EditorGUILayout.LabelField("Character Editor", new GUIStyle(style) { alignment = TextAnchor.UpperCenter });

            if (selectedCharacter == null)
            {
                // EditorGUILayout.LabelField("No character selected", new GUIStyle(style) { alignment = TextAnchor.UpperCenter });
                if (GUILayout.Button("NOVO CHARACTER"))
                {
                    selectedCharacter = CreateInstance<Character>();
                    Repaint();
                }
            }
            else
            {
                selectedCharacter.SetNome(EditorGUILayout.TextField("Nome", selectedCharacter.GetNome()));
                selectedCharacter.SetClasse((Character.Classe)EditorGUILayout.EnumPopup("Classe", selectedCharacter.GetClasse()));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Atributos");

                Rect rect = GUILayoutUtility.GetRect(16, 760, 9, 240);

                Vector3[] points = {
                    // Ponta 1
                    new Vector3(rect.x + 20, rect.y + rect.height / 4),

                    // Central
                    new Vector3(rect.width / 2, rect.y + 10),

                    // Ponta 2
                    new Vector3(rect.width - rect.x - 20, rect.y + rect.height / 4),

                    // Ponto 4
                    new Vector3(rect.width - rect.x - rect.width / 4, rect.height + rect.y - 40),

                    // Ponto 5
                    new Vector3(rect.width / 4 + rect.x, rect.height + rect.y - 40),

                    // Ponto final
                    new Vector3(rect.x + 20, rect.y + rect.height / 4)
                };

                Debug.Log($"{rect.x} {rect.y}");
                Handles.DrawPolyLine(points);

                EditorGUILayout.Space();

                if (GUILayout.Button("SALVAR"))
                {
                    SaveCharacter(selectedCharacter);
                }
            }



            EditorGUILayout.EndVertical();
        }

        private void SaveCharacter(Character c)
        {
            if (AssetDatabase.GetAssetPath(c) == "")
            {
                AssetDatabase.CreateAsset(
                    c, $"Assets/{c.GetNome()}.asset"
                );
            }
        }
    }
}