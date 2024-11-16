using UnityEngine;
using System.Collections.Generic;

using System.IO;

namespace AutoIssue
{


    public class IssueStorageManager
    {
        private const string JsonPath = "Assets/IssueStorage.json";
        private static IssueStorageManager _instance;

        public static IssueStorageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IssueStorageManager();
                }
                return _instance;
            }
        }

        [SerializeField]
        public List<IssueData> Issues { get; private set; } 

        private IssueStorageManager()
        {
            LoadIssuesFromJson();
        }

        /// <summary>
        /// Carga los issues desde el archivo JSON. Si no existe, inicializa una lista vacía.
        /// </summary>
        private void LoadIssuesFromJson()
        {
            if (!File.Exists(JsonPath))
            {
                Debug.LogWarning($"No se encontró el archivo JSON en {JsonPath}. Inicializando lista vacía.");
                Issues = new List<IssueData>();
                return;
            }

            try
            {
                string json = File.ReadAllText(JsonPath);
                Issues = JsonUtility.FromJson<IssueStorageWrapper>(json)?.Issues ?? new List<IssueData>();
                Debug.Log($"Datos cargados desde {JsonPath}. Issues encontrados: {Issues.Count}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al cargar el archivo JSON: {ex.Message}");
                Issues = new List<IssueData>();
            }
        }

        /// <summary>
        /// Guarda los issues actuales en un archivo JSON.
        /// </summary>
        public void SaveIssuesToJson()
        {
            try
            {
                string json = JsonUtility.ToJson(new IssueStorageWrapper { Issues = Issues }, true);
                File.WriteAllText(JsonPath, json);
                //Debug.Log($"Datos guardados en {JsonPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al guardar el archivo JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Añade un nuevo issue y guarda los cambios en el archivo JSON.
        /// </summary>
        public void AddIssue(IssueData issue)
        {
            Issues.Add(issue);
            
        }

        /// <summary>
        /// Limpia la lista de issues y guarda los cambios en el archivo JSON.
        /// </summary>
        public void ClearIssues()
        {
            Issues.Clear();
            SaveIssuesToJson();
        }

        /// <summary>
        /// Clase de envoltura para serializar/deserializar la lista de issues.
        /// </summary>
        [System.Serializable]
        private class IssueStorageWrapper
        {
            public List<IssueData> Issues = new List<IssueData>();
        }
    }
}