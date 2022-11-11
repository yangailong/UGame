using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace UGame_Local_Editor
{
    public class ProcessExcelWindow : EditorWindow
    {

        [MenuItem("Tools/UGame/Excel表数据转ScriptableObject %E")]
        public static void ShowExample()
        {
            ProcessExcelWindow wnd = GetWindow<ProcessExcelWindow>();
            wnd.titleContent = new GUIContent("ProcessExcelWindow");
        }

        public const string SETTINGS_PATH = "ProjectSettings/ExcelToScriptableObjectSettings.asset";

        private Dictionary<Button, ExcelVisualElementParams> keyValuePairs = null;

        private ProcessExcelImpl impl = null;

        private IntegerField FieldRow = null, TypeRow = null, DataFromRow = null;

        private ScrollView ScrollView = null;

        private Button AddNewExcel = null, ProcessAll = null;

        private ExcelVisualElement excelVisualElement = null;


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ProcessExcelWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            keyValuePairs = new Dictionary<Button, ExcelVisualElementParams>();
            impl = new ProcessExcelImpl();

            FieldRow = root.Q<IntegerField>(nameof(FieldRow));
            TypeRow = root.Q<IntegerField>(nameof(TypeRow));
            DataFromRow = root.Q<IntegerField>(nameof(DataFromRow));
            AddNewExcel = root.Q<Button>(nameof(AddNewExcel));
            ProcessAll = root.Q<Button>(nameof(ProcessAll));

            ScrollView = root.Q<ScrollView>();

            excelVisualElement = new ExcelVisualElement(root.Q<VisualElement>(nameof(excelVisualElement)), OnClickInsertBtnCallback, OnClickDeleteBtnCallback, OnClickProcessBtnCallback);

            excelVisualElement.SetActive(false);

            ProcessAll.clicked += ProcessAll_clicked;
            AddNewExcel.clicked += AddNewExcel_clicked;

            OnLoad();
        }


        private void OnDisable()
        {
            OnSave();
        }


        private void OnLoad()
        {
            string json = File.Exists(SETTINGS_PATH) ? File.ReadAllText(SETTINGS_PATH, Encoding.UTF8) : null;

            if (!string.IsNullOrEmpty(json))
            {
                ExcelToScriptableObjectSettings settings = JsonUtility.FromJson<ExcelToScriptableObjectSettings>(json);

                FieldRow.value = settings.FieldRow;
                TypeRow.value = settings.TypeRow;
                DataFromRow.value = settings.DataFromRow;

                foreach (var item in settings.excels)
                {
                    var excelBtn = new Button() { text = $"{item.ExcelPath}" };
                    excelBtn.clicked += () => { SelExcelBtn(excelBtn); };
                    ScrollView.Add(excelBtn);
                    keyValuePairs.Add(excelBtn, item);
                }
            }
        }


        private void OnSave()
        {
            if (keyValuePairs?.Count == 0) return;

            ExcelToScriptableObjectSettings data = new ExcelToScriptableObjectSettings();
            data.FieldRow = FieldRow.value;
            data.TypeRow = TypeRow.value;
            data.DataFromRow = DataFromRow.value;
            data.excels = keyValuePairs.Values.ToArray();
            File.WriteAllText(SETTINGS_PATH, JsonUtility.ToJson(data, true), Encoding.UTF8);
        }



        private void AddNewExcel_clicked()
        {
            Debug.Log($"添加excel");

            string path = EditorUtility.OpenFilePanel("Select Excel File", $"/..{Application.dataPath}", "xlsx");
            if (!string.IsNullOrEmpty(path))
            {
                string projPath = Application.dataPath;
                projPath = projPath.Substring(0, projPath.Length - 6);
                if (path.StartsWith(projPath))
                {
                    string tmpPath = path.Substring(projPath.Length, path.Length - projPath.Length);

                    var excelBtn = new Button() { text = $"{tmpPath}" };
                    excelBtn.clicked += () => { SelExcelBtn(excelBtn); };
                    ScrollView.Add(excelBtn);
                    keyValuePairs.Add(excelBtn, new ExcelVisualElementParams() { ExcelPath = path });
                }
            }
        }



        private void ProcessAll_clicked()
        {
            Debug.Log($"解析所有excel");
        }


        private void OnClickInsertBtnCallback()
        {
            Debug.Log($"插入excel");
        }


        private void OnClickDeleteBtnCallback()
        {
            Debug.Log($"删除excel");
        }


        private void OnClickProcessBtnCallback()
        {
            Debug.Log($"解析excel");

            foreach (var item in keyValuePairs.Values)
            {
                impl.Process(item, true);
            }
        }


        private void SelExcelBtn(Button button)
        {
            Debug.Log($"excel name:{button.text}");

            excelVisualElement.SetData(keyValuePairs[button]);
            excelVisualElement.SetActive(true);
        }


        public class ExcelVisualElement
        {
            public VisualElement root;

            public TextField NameSpace = null;

            public Toggle UseHashString = null, PublicItemsGetter = null, HideaAssetProperties = null, CompressColorintoInteger = null, GenerateGetMethodIfPossoble = null, TreaUnknowTypesasEnum = null, IDorKeytoMuitiValues = null, GengrateToStringMethod = null;

            public Button ScriptFolder = null, AssetFolder = null, InsertBtn = null, DeleteBtn = null, ProcessBtn = null;

            public ExcelVisualElement(VisualElement root, Action onClickInsertBtnCallback, Action onClickDeleteBtnCallback, Action onClickProcessBtnCallback)
            {
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


                UseHashString.RegisterCallback<ChangeEvent<bool>>(UseHashStringRegisterCallback);
                PublicItemsGetter.RegisterCallback<ChangeEvent<bool>>(PublicItemsGetterRegisterCallback);
                HideaAssetProperties.RegisterCallback<ChangeEvent<bool>>(HideaAssetPropertiesRegisterCallback);
                CompressColorintoInteger.RegisterCallback<ChangeEvent<bool>>(CompressColorintoIntegerRegisterCallback);
                GenerateGetMethodIfPossoble.RegisterCallback<ChangeEvent<bool>>(GenerateGetMethodIfPossobleRegisterCallback);
                TreaUnknowTypesasEnum.RegisterCallback<ChangeEvent<bool>>(TreaUnknowTypesasEnumRegisterCallback);
                IDorKeytoMuitiValues.RegisterCallback<ChangeEvent<bool>>(IDorKeytoMuitiValuesRegisterCallback);
                GengrateToStringMethod.RegisterCallback<ChangeEvent<bool>>(GengrateToStringMethodRegisterCallback);


                ScriptFolder.clicked += ScriptDirectory_clicked;
                AssetFolder.clicked += AssetDirectory_clicked;
                InsertBtn.clicked += onClickInsertBtnCallback;
                DeleteBtn.clicked += onClickDeleteBtnCallback;
                ProcessBtn.clicked += onClickProcessBtnCallback;
            }


            private void ScriptDirectory_clicked()
            {
                if (elementParams == null) return;
                string path = EditorUtility.OpenFolderPanel("脚本生成路径", $"{Application.dataPath}", string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    elementParams.ScriptFolder = ScriptFolder.text = path;
                }
            }


            private void AssetDirectory_clicked()
            {
                if (elementParams == null) return;
                string path = EditorUtility.OpenFolderPanel("解析数据生成路径", $"{Application.dataPath}", string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    elementParams.AssetFolder = AssetFolder.text = path;
                }
            }


            private void UseHashStringRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.UseHashString = value.newValue;
            }


            private void PublicItemsGetterRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.PublicItemsGetter = value.newValue;
            }


            private void HideaAssetPropertiesRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.HideaAssetProperties = value.newValue;
            }


            private void CompressColorintoIntegerRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.CompressColorintoInteger = value.newValue;
            }


            private void GenerateGetMethodIfPossobleRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.GenerateGetMethodIfPossoble = value.newValue;
            }


            private void TreaUnknowTypesasEnumRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.TreaUnknowTypesasEnum = value.newValue;
            }


            private void IDorKeytoMuitiValuesRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.IDorKeytoMuitiValues = value.newValue;
            }


            private void GengrateToStringMethodRegisterCallback(ChangeEvent<bool> value)
            {
                if (elementParams == null) return;
                elementParams.GengrateToStringMethod = value.newValue;
            }



            public void SetActive(bool active)
            {
                root.visible = active;
            }


            private ExcelVisualElementParams elementParams = null;

            public void SetData(ExcelVisualElementParams elementParams)
            {
                this.elementParams = elementParams;
                this.NameSpace.value = elementParams.NameSpace;
                this.ScriptFolder.text = elementParams.ScriptFolder;
                this.AssetFolder.text = elementParams.AssetFolder;
                this.UseHashString.value = elementParams.UseHashString;
                this.PublicItemsGetter.value = elementParams.PublicItemsGetter;
                this.HideaAssetProperties.value = elementParams.HideaAssetProperties;
                this.CompressColorintoInteger.value = elementParams.CompressColorintoInteger;
                this.GenerateGetMethodIfPossoble.value = elementParams.GenerateGetMethodIfPossoble;
                this.TreaUnknowTypesasEnum.value = elementParams.TreaUnknowTypesasEnum;
                this.IDorKeytoMuitiValues.value = elementParams.IDorKeytoMuitiValues;
                this.GengrateToStringMethod.value = elementParams.GengrateToStringMethod;
            }

        }


        [System.Serializable]
        public class ExcelVisualElementParams
        {
            public string ExcelPath = "", NameSpace = "UGame.Remove", ScriptFolder = "Select", AssetFolder = "Select";

            public bool UseHashString = false, PublicItemsGetter = false, HideaAssetProperties = false, CompressColorintoInteger = false, GenerateGetMethodIfPossoble = false, TreaUnknowTypesasEnum = false, IDorKeytoMuitiValues = false, GengrateToStringMethod = false;
        }


        [System.Serializable]
        public class ExcelToScriptableObjectSettings
        {
            public int FieldRow = 0;
            public int TypeRow = 1;
            public int DataFromRow = 4;

            public ExcelVisualElementParams[] excels;
        }


    }
}