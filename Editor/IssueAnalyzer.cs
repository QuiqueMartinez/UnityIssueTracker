using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Compilation;
using UnityEditor.UIElements;

namespace AutoIssue
{
    public class IssueAnalyzer : EditorWindow
    {
        //private List<IssueData> issues = new List<IssueData>();



        private ScrollView issuesList;

        private bool autoScanEnabled = false;

        private Button scanButton;

        public List<string> targetAssemblies = new List<string> { "Assembly-CSharp" }; // Array de ensamblados a analizar, editable desde el Inspector

        [MenuItem("Window/Issue Tracker")]
        public static void ShowWindow()
        {
            var window = GetWindow<IssueAnalyzer>("Issue Tracker");
            window.minSize = new Vector2(600, 400);
        }

        public void OnEnable()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty targetAssembliesProperty = serializedObject.FindProperty("targetAssemblies");

            var assembliesField = new PropertyField(targetAssembliesProperty, "Target Assemblies")
            {
                bindingPath = "targetAssemblies"
            };
            rootVisualElement.Add(assembliesField);

        }

        public void CreateGUI()
        {
            // Crear contenedor para Auto y Scan alineados a la derecha
            var toolbarContainer = new VisualElement();
            toolbarContainer.style.flexDirection = FlexDirection.Row;
            toolbarContainer.style.justifyContent = Justify.FlexEnd;
            toolbarContainer.style.marginBottom = 10;

 
            scanButton = new Button() { text = "Scan", name = "scanButton" };
            //scanButton.clicked += () => { if (!autoScanEnabled) AnalyzeCode(); };

            toolbarContainer.Add(scanButton);
            rootVisualElement.Add(toolbarContainer);

            scanButton.clicked += () => {
                if (!autoScanEnabled)
                {
                    AnalyzeCode();
                    scanButton.style.backgroundColor = new StyleColor(Color.clear); // Restablecer el color a su estado original
                }
            };

            if (issuesList == null)
            {
                issuesList = new ScrollView();
                rootVisualElement.Add(issuesList);
            }
            rootVisualElement.Add(issuesList);

            scanButton.style.backgroundColor = new StyleColor(Color.yellow);


            UpdateIssuesList();
        }

        private void OnCompilationFinished(string assemblyPath, CompilerMessage[] messages)
        {
            scanButton.style.backgroundColor = new StyleColor(Color.yellow);
        }

        private void AnalyzeCode()
        {
            // issueStorage.issues.Clear();
            //var issueStorage = IssueAnalyzerInitializer.GetOrCreateIssueStorage();
            IssueStorageManager.Instance.ClearIssues();

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    // Obtener el script asociado al tipo
                    MonoScript script = FindScriptFromType(type);
                    string filePath = script != null ? AssetDatabase.GetAssetPath(script) : string.Empty;

                    // Analizar clases
                    var classAttribute = type.GetCustomAttribute<IssueAttribute>();
                    if (classAttribute != null)
                    {
                        IssueStorageManager.Instance.AddIssue(new IssueData
                        {
                            Priority = classAttribute.Priority,
                            Tag = classAttribute.Tag,
                            Status = classAttribute.Status,
                            Description = classAttribute.Description,
                            TypeName = type.Name,
                            FilePath = filePath,
                            LineNumber = 1 // Aproximación, mejorar si se conoce la línea exacta
                        });


                    }

                    // Analizar métodos
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var methodAttribute = method.GetCustomAttribute<IssueAttribute>();
                        if (methodAttribute != null)
                        {
                            IssueStorageManager.Instance.AddIssue(new IssueData
                            {
                                Priority = methodAttribute.Priority,
                                Tag = methodAttribute.Tag,
                                Status = methodAttribute.Status,
                                Description = methodAttribute.Description,
                                TypeName = type.Name,
                                FilePath = filePath,
                                LineNumber = method.GetMethodBody()?.GetILAsByteArray().Length ?? 1 // Mejorar cálculo de línea
                            });
                        }
                    }
                }
            }
            IssueStorageManager.Instance.SaveIssuesToJson();
            UpdateIssuesList();



        }

        private MonoScript FindScriptFromType(Type type)
        {
            foreach (MonoScript script in MonoImporter.GetAllRuntimeMonoScripts())
            {
                if (script.GetClass() == type)
                {
                    return script;
                }
            }
            return null;
        }

        private void UpdateIssuesList()
        {
            issuesList.Clear();
            foreach (var issue in IssueStorageManager.Instance.Issues)
            {
                var issueElement = new VisualElement();
                issueElement.style.flexDirection = FlexDirection.Row;
                issueElement.style.justifyContent = Justify.SpaceBetween;
                issueElement.style.alignItems = Align.Center;
                issueElement.style.marginBottom = 5;
                issueElement.style.paddingRight = 5;
                issueElement.style.borderBottomWidth = 1;
                issueElement.style.borderBottomColor = Color.gray;
                issueElement.style.backgroundColor = new StyleColor(new Color(0.85f, 0.85f, 0.85f)); // Fondo gris

                // Crear etiquetas con estilo aplicado directamente
                var priorityLabel = issue.Priority switch
                {
                    Priority.Low => new Label(issue.Priority.ToString())
                    {
                        style = { width = 80, color = Color.black, unityTextAlign = TextAnchor.MiddleCenter, backgroundColor = new StyleColor(new Color(0.8f, 1.0f, 0.8f)), marginRight = 5 }
                    },
                    Priority.Medium => new Label(issue.Priority.ToString())
                    {
                        style = { width = 80, color = Color.black, unityTextAlign = TextAnchor.MiddleCenter, backgroundColor = new StyleColor(new Color(1f, 0.9f, 0.5f)), marginRight = 5 }
                    },
                    Priority.High => new Label(issue.Priority.ToString())
                    {
                        style = { width = 80, color = Color.black, unityTextAlign = TextAnchor.MiddleCenter, backgroundColor = new StyleColor(new Color(1f, 0.5f, 0.5f)), marginRight = 5 }
                    },
                    _ => throw new NotImplementedException(),
                };





                var tagLabel = new Label(issue.Tag.ToString())
                {
                    style = { width = 80, color = Color.black, unityTextAlign = TextAnchor.MiddleCenter, backgroundColor = new StyleColor(new Color(0.80f, 0.55f, 0.85f)), marginRight = 5 }
                };

                var statusLabel = new Label(issue.Status.ToString())
                {
                    style = { width = 80, color = Color.black, unityTextAlign = TextAnchor.MiddleCenter, backgroundColor = new StyleColor(new Color(0.7f, 0.9f, 1f)), marginRight = 5 }
                };

                var descriptionLabel = new Label("(" + issue.TypeName + ")" + issue.Description)
                {
                    style = { flexGrow = 1, color = Color.black, unityTextAlign = TextAnchor.MiddleLeft, paddingLeft = 5 }
                };

                var goToButton = new Button(() =>
                {
                    if (!string.IsNullOrEmpty(issue.FilePath))
                    {
                        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(issue.FilePath), issue.LineNumber);
                    }
                })
                {
                    text = "Go to file",
                    style = { marginLeft = 5 }
                };

                // Agregar los elementos al contenedor de la fila
                issueElement.Add(priorityLabel);
                issueElement.Add(tagLabel);
                issueElement.Add(statusLabel);
                issueElement.Add(descriptionLabel);
                issueElement.Add(goToButton);

                // Añadir la fila al contenedor de issues
                issuesList.Add(issueElement);
            }
            Debug.Log($"Actualizando lista de issues en la UI: {IssueStorageManager.Instance.Issues.Count}.");

            Repaint();
        }
    }

    [System.Serializable]
    public class IssueData
    {
        [SerializeField] public Priority Priority;
        [SerializeField] public Tag Tag;
        [SerializeField] public Status Status;
        [SerializeField] public string Description;
        [SerializeField] public string TypeName;// Nombre de la clase o método
        [SerializeField] public string FilePath; // Ruta del archivo del script
        [SerializeField] public int LineNumber;
    }
}
