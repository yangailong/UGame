using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;
using Unity.VisualScripting;

namespace UGame_Local_Editor
{
    public class ProcessExcelWindow : EditorWindow
    {

        [MenuItem("Tools/UGame/Excel������תScriptableObject %E")]
        public static void ShowExample()
        {
            ProcessExcelWindow wnd = GetWindow<ProcessExcelWindow>();
            wnd.titleContent = new GUIContent("ProcessExcelWindow");
        }

        public const string SETTINGS_PATH = "ProjectSettings/ExcelToScriptableObjectSettings.asset";


        private List<ExcelVisualElement> excelVisuals = null;

        private ProcessExcelImpl impl = null;

        private IntegerField FieldRow = null, TypeRow = null, DataFromRow = null;

        private ScrollView ScrollView = null;

        private Button AddNewExcel = null, ProcessAll = null;


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ProcessExcelWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            impl = new ProcessExcelImpl();
            excelVisuals = new List<ExcelVisualElement>();

            FieldRow = root.Q<IntegerField>(nameof(FieldRow));
            TypeRow = root.Q<IntegerField>(nameof(TypeRow));
            DataFromRow = root.Q<IntegerField>(nameof(DataFromRow));
            AddNewExcel = root.Q<Button>(nameof(AddNewExcel));
            ProcessAll = root.Q<Button>(nameof(ProcessAll));
            ScrollView = root.Q<ScrollView>();

            AddNewExcel.clicked += AddNewExcel_clicked;
            ProcessAll.clicked += ProcessAll_clicked;

            OnLoadCache();
        }


        private void OnDisable()
        {
            OnSaveCache();
        }



        /// <summary>
        /// ������ȡ��������
        /// </summary>
        private void OnLoadCache()
        {
            string json = File.Exists(SETTINGS_PATH) ? File.ReadAllText(SETTINGS_PATH, Encoding.UTF8) : null;

            if (!string.IsNullOrEmpty(json))
            {
                CacheData cache = JsonUtility.FromJson<CacheData>(json);

                FieldRow.value = cache.head.FieldRow;
                TypeRow.value = cache.head.TypeRow;
                DataFromRow.value = cache.head.DataFromRow;

                foreach (var item in cache.body)
                {
                    ExcelVisualElement element = new ExcelVisualElement(ScrollView, InsertBtn_clicked, DeleteBtn_clicked, ProcessBtn_clicked);
                    element.SetData(item);
                    excelVisuals.Add(element);
                }

            }
        }


        /// <summary>
        /// �ر�����¼���õ�����
        /// </summary>
        private void OnSaveCache()
        {
            CacheData cache = new CacheData();
            cache.head = new Head();
            cache.head.FieldRow = FieldRow.value;
            cache.head.TypeRow = TypeRow.value;
            cache.head.DataFromRow = DataFromRow.value;

            cache.body = (from p in excelVisuals select p.data).ToArray();

            File.WriteAllText(SETTINGS_PATH, JsonUtility.ToJson(cache, true), Encoding.UTF8);
        }


        /// <summary>
        /// ���Excel
        /// </summary>
        private void AddNewExcel_clicked()
        {
            Debug.Log($"���excel");

            string path = EditorUtility.OpenFilePanel("ѡ��Exce", ".", "xlsx");
            if (!string.IsNullOrEmpty(path))
            {

                string projPath = Application.dataPath;
                projPath = projPath.Substring(0, projPath.Length - 6);
                if (path.StartsWith(path))
                {
                    string name = Path.GetFileNameWithoutExtension(path);
                    string excelPath = path.Substring(projPath.Length, path.Length - projPath.Length);

                    ExcelVisualElement element = new ExcelVisualElement(ScrollView, InsertBtn_clicked, DeleteBtn_clicked, ProcessBtn_clicked);
                    element.SetData(new Body() { ExcelName = name, ExcelPath = path });

                    excelVisuals.Add(element);
                }


            }
        }


        /// <summary>
        /// ����ȫ��Excel
        /// </summary>
        private void ProcessAll_clicked()
        {
            ProcessExcel(excelVisuals);
        }


        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="eve"></param>
        private void DeleteBtn_clicked(ExcelVisualElement eve)
        {
            ScrollView.Remove(eve.root);
            excelVisuals.Remove(eve);
            eve = null;
        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="eve"></param>
        private void InsertBtn_clicked(ExcelVisualElement eve)
        {

        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="eve"></param>
        private void ProcessBtn_clicked(ExcelVisualElement eve)
        {
            ProcessExcel(new List<ExcelVisualElement>() { eve });
        }


        private void ProcessExcel(List<ExcelVisualElement> eves)
        {
            if (eves?.Count == 0) return;

            List<string> excelPaths = new List<string>();

            var head = new Head() { TypeRow = TypeRow.value, DataFromRow = DataFromRow.value, FieldRow = FieldRow.value };

            for (int i = 0; i < eves.Count; i++)
            {
                var item = eves[i];

                if (!Directory.Exists(item.data.ScriptFolder))
                {
                    EditorUtility.DisplayDialog("����ʧ��", $"�ű�����·��������:{item.data.ScriptFolder}", "OK");
                    return;
                }

                if (!Directory.Exists(item.data.AssetFolder))
                {
                    EditorUtility.DisplayDialog("����ʧ��", $"��������·��������:{item.data.AssetFolder}", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(item.data.NameSpace) && !Regex.IsMatch(item.data.NameSpace, @"(\S+\s*\.\s*)*\S+"))
                {
                    EditorUtility.DisplayDialog("����ʧ��", $"{item.data.NameSpace}.{item.ExcelName.text}�����ռ䲻�Ϸ�", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(item.data.ExcelPath))
                {
                    EditorUtility.DisplayDialog("����ʧ��", $"δ�ҵ�excel{item.data.ExcelPath}", "OK");
                    return;
                }

                if (impl.Process(head, item.data, true))
                {
                    excelPaths.Add(item.data.ExcelPath);
                }
            }

            if (excelPaths.Count > 0)
            {
                EditorPrefs.SetString("process_excels", string.Join("#", excelPaths.ToArray()));
                mToWriteAssets = true;
                AssetDatabase.Refresh();
            }

        }



        bool mToWriteAssets = false;
        void Update()
        {
            if (mToWriteAssets && !EditorApplication.isCompiling)
            {
                mToWriteAssets = false;
                string[] names = EditorPrefs.GetString("process_excels", "").Split('#');
                EditorPrefs.DeleteKey("process_excels");

                var baseRow = new Head() { TypeRow = TypeRow.value, DataFromRow = DataFromRow.value, FieldRow = FieldRow.value };

                foreach (string name in names)
                {
                    if (string.IsNullOrEmpty(name)) continue;

                    foreach (var item in excelVisuals)
                    {
                        if (item.data.ExcelPath.Equals(name))
                        {
                            impl.Process(baseRow, item.data, false); break;
                        }
                    }
                }


            }
        }




        public class ExcelVisualElement
        {
            public VisualElement root = null;

            public TextField NameSpace = null;
            public Label ExcelName = null;

            public Toggle UseHashString = null, PublicItemsGetter = null, HideaAssetProperties = null, CompressColorintoInteger = null, GenerateGetMethodIfPossoble = null, TreaUnknowTypesasEnum = null, IDorKeytoMuitiValues = null, GengrateToStringMethod = null;

            public Button ScriptFolder = null, AssetFolder = null, InsertBtn = null, DeleteBtn = null, ProcessBtn = null;

            public ExcelVisualElement(VisualElement parent, Action<ExcelVisualElement> insertCallback, Action<ExcelVisualElement> deleteCallback, Action<ExcelVisualElement> processCallback)
            {
                var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ExcelItem.uxml");
                VisualElement root = item.Instantiate();
                parent.Add(root);

                this.root = root;
                this.NameSpace = root.Q<TextField>(nameof(NameSpace));
                this.ScriptFolder = root.Q<Button>(nameof(ScriptFolder));
                this.AssetFolder = root.Q<Button>(nameof(AssetFolder));
                this.UseHashString = root.Q<Toggle>(nameof(UseHashString));
                this.PublicItemsGetter = root.Q<Toggle>(nameof(PublicItemsGetter));
                this.HideaAssetProperties = root.Q<Toggle>(nameof(HideaAssetProperties));
                this.CompressColorintoInteger = root.Q<Toggle>(nameof(CompressColorintoInteger));
                this.GenerateGetMethodIfPossoble = root.Q<Toggle>(nameof(GenerateGetMethodIfPossoble));
                this.TreaUnknowTypesasEnum = root.Q<Toggle>(nameof(TreaUnknowTypesasEnum));
                this.IDorKeytoMuitiValues = root.Q<Toggle>(nameof(IDorKeytoMuitiValues));
                this.GengrateToStringMethod = root.Q<Toggle>(nameof(GengrateToStringMethod));
                this.InsertBtn = root.Q<Button>(nameof(InsertBtn));
                this.DeleteBtn = root.Q<Button>(nameof(DeleteBtn));
                this.ProcessBtn = root.Q<Button>(nameof(ProcessBtn));
                this.ExcelName = root.Q<Label>(nameof(ExcelName));


                UseHashString.RegisterCallback<ChangeEvent<bool>>(UseHashStringRegisterCallback);
                PublicItemsGetter.RegisterCallback<ChangeEvent<bool>>(PublicItemsGetterRegisterCallback);
                HideaAssetProperties.RegisterCallback<ChangeEvent<bool>>(HideaAssetPropertiesRegisterCallback);
                CompressColorintoInteger.RegisterCallback<ChangeEvent<bool>>(CompressColorintoIntegerRegisterCallback);
                GenerateGetMethodIfPossoble.RegisterCallback<ChangeEvent<bool>>(GenerateGetMethodIfPossobleRegisterCallback);
                TreaUnknowTypesasEnum.RegisterCallback<ChangeEvent<bool>>(TreaUnknowTypesasEnumRegisterCallback);
                IDorKeytoMuitiValues.RegisterCallback<ChangeEvent<bool>>(IDorKeytoMuitiValuesRegisterCallback);
                GengrateToStringMethod.RegisterCallback<ChangeEvent<bool>>(GengrateToStringMethodRegisterCallback);
                this.NameSpace.RegisterCallback<ChangeEvent<string>>(demo);

                ScriptFolder.clicked += ScriptDirectory_clicked;
                AssetFolder.clicked += AssetDirectory_clicked;

                InsertBtn.clicked += () => { insertCallback?.Invoke(this); };
                DeleteBtn.clicked += () => { deleteCallback?.Invoke(this); };
                ProcessBtn.clicked += () => { processCallback?.Invoke(this); };
            }


            /// <summary>ѡ��ű�����·��</summary>
            private void ScriptDirectory_clicked()
            {
                if (data == null) return;

                string tmp = $"{Application.dataPath}/../HotFixAssembly/Scripts/Game/Data/Normal/ScriptableObject";
                string path = EditorUtility.OpenFolderPanel("�ű�����·��", Application.dataPath, string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    data.ScriptFolder = ScriptFolder.text = path;
                }
            }


            /// <summary>ѡ����������·��</summary>
            private void AssetDirectory_clicked()
            {
                if (data == null) return;
                string path = EditorUtility.OpenFolderPanel("������������·��", $"{Application.dataPath}", string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    data.AssetFolder = AssetFolder.text = path;
                }
            }


            private void UseHashStringRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.UseHashString = value.newValue;
            }


            private void PublicItemsGetterRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.PublicItemsGetter = value.newValue;
            }


            private void HideaAssetPropertiesRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.HideaAssetProperties = value.newValue;
            }


            private void CompressColorintoIntegerRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.CompressColorintoInteger = value.newValue;
            }


            private void GenerateGetMethodIfPossobleRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.GenerateGetMethodIfPossoble = value.newValue;
            }


            private void TreaUnknowTypesasEnumRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.TreaUnknowTypesasEnum = value.newValue;
            }


            private void IDorKeytoMuitiValuesRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.IDorKeytoMultiValues = value.newValue;
            }


            private void GengrateToStringMethodRegisterCallback(ChangeEvent<bool> value)
            {
                if (data == null) return;
                data.GengrateToStringMethod = value.newValue;
            }


            public void demo(ChangeEvent<string> value)
            {
                Debug.Log($"{value.newValue}");
            }


            public Body data = null;

            public void SetData(Body data)
            {
                this.data = data;
                this.NameSpace.value = data.NameSpace;
                this.ScriptFolder.text = data.ScriptFolder;
                this.AssetFolder.text = data.AssetFolder;
                this.UseHashString.value = data.UseHashString;
                this.PublicItemsGetter.value = data.PublicItemsGetter;
                this.HideaAssetProperties.value = data.HideaAssetProperties;
                this.CompressColorintoInteger.value = data.CompressColorintoInteger;
                this.GenerateGetMethodIfPossoble.value = data.GenerateGetMethodIfPossoble;
                this.TreaUnknowTypesasEnum.value = data.TreaUnknowTypesasEnum;
                this.IDorKeytoMuitiValues.value = data.IDorKeytoMultiValues;
                this.GengrateToStringMethod.value = data.GengrateToStringMethod;
                this.ExcelName.text = data.ExcelName;
            }

        }


        [Serializable]
        public class CacheData
        {
            //��ͷ
            public Head head = null;

            //����
            public Body[] body = null;
        }


        [Serializable]
        public class Head
        {
            public int FieldRow = 0;
            public int TypeRow = 1;
            public int DataFromRow = 2;
        }


        [Serializable]
        public class Body
        {
            public string ExcelName = string.Empty, ExcelPath = string.Empty, NameSpace = "UGame.Remove", ScriptFolder = "Select", AssetFolder = "Select";


            public bool UseHashString = false;


            /// <summary>����Item����</summary>
            public bool PublicItemsGetter = false;


            /// <summary>inspector����ʾ���Ա���</summary>
            public bool HideaAssetProperties = false;


            /// <summary>����ɫѹ��������</summary>
            public bool CompressColorintoInteger = false;


            /// <summary>�������Ի�ȡ����</summary>
            public bool GenerateGetMethodIfPossoble = true;


            /// <summary>�ֶ�����Ϊö��</summary>
            public bool TreaUnknowTypesasEnum = false;


            /// <summary>id����key��ֵ</summary>
            public bool IDorKeytoMultiValues = false;


            /// <summary>��дToString����</summary>
            public bool GengrateToStringMethod = true;

        }

    }
}