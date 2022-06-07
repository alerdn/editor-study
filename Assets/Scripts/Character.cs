using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "RPG/Character")]
    public class Character : ScriptableObject
    {
        public enum Classe
        {
            Guerreiro,
            Mago,
            Guardião
        }

        [SerializeField]
        private string nome;
        [SerializeField]
        private Classe classe;

        public string GetNome()
        {
            return nome;
        }

        public Classe GetClasse()
        {
            return classe;
        }

#if UNITY_EDITOR
        public void SetNome(string newNome)
        {
            if (newNome != nome)
            {
                Undo.RecordObject(this, "Updated nome");
                nome = newNome;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetClasse(Classe newClasse)
        {
            if (newClasse != classe)
            {
                Undo.RecordObject(this, "Updated classe");
                classe = newClasse;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}