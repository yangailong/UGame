using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace UGame_Local_Editor
{
    public class ExcelToScriptableObject : EditorWindow
    {

        [MenuItem("Tools/UGame/Excel表数据转ScriptableObject %E")]
        public static void ShowExample()
        {
            ExcelToScriptableObject wnd = GetWindow<ExcelToScriptableObject>();
            wnd.titleContent = new GUIContent("ExcelToScriptableObject");
        }


        private ExcelToScriptableObjectImpl impl;

        private IntegerField fieldRow = null;
        private IntegerField typeRow = null;
        private IntegerField dataFromRow = null;
        private Button processAll = null;
        private Button addNewExcel = null;

        private ScrollView ecrollView = null;

        private ExcelItem excelItem = null;

        private List<ExcelItem> excelItems = null;

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ExcelToScriptableObjectWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            impl = new ExcelToScriptableObjectImpl();

            fieldRow = root.Q<IntegerField>("FieldRow");
            typeRow = root.Q<IntegerField>("TypeRow");
            dataFromRow = root.Q<IntegerField>("DataFromRow");

            processAll = root.Q<Button>("ProcessAll");
            addNewExcel = root.Q<Button>("AddNewExcel");

            ecrollView = root.Q<ScrollView>();
            excelItem = new ExcelItem(root.Q<VisualElement>("Item"));

            //excelItem.root.visible = false;

            //VisualElement cell=new VisualElement().contentRect;

            excelItems = new List<ExcelItem>();

            processAll.clicked += ProcessAll_clicked;

            addNewExcel.clicked += AddNewExcel_clicked;

            excelItems.Add(excelItem);
            foreach (var item in excelItems)
            {
                item.OnEnable();
            }

        }



        private void AddNewExcel_clicked()
        {
            Debug.LogError("AddNewExcel_clicked");
            // impl.AddNewExcel();

            //var sv= rootVisualElement.Q<ScrollView>();
            //sv.Add(excelItem.root);

            //VisualElement ve = Object.Instantiate<VisualElement>(excelItem.root);
        }


        private void ProcessAll_clicked()
        {
            Debug.LogError("ProcessAll_clicked");
        }


        private void OnDestroy()
        {
            processAll.clicked -= ProcessAll_clicked;

            addNewExcel.clicked -= AddNewExcel_clicked;

            foreach (var item in excelItems)
            {
                item.OnDestroy();
            }
        }


        public class ExcelItem
        {
            public VisualElement root = null;

            public Toggle useHashString = null;
            public Toggle publicItemGetter = null;
            public Toggle hideAssetProperties = null;
            public Toggle compressColorIntoInteger = null;
            public Toggle generateGetMethodIfPossible = null;
            public Toggle treatUnknowTypeAsEnum = null;
            public Toggle idOrKeyMultiValues = null;
            public Toggle generateToStringMethod = null;
            public TextField nameSpace = null;

            public Button insert = null;
            public Button delete = null;
            public Button processExcel = null;

            public ExcelItem(VisualElement root)
            {
                this.root = root;
                this.useHashString = root.Q<Toggle>("UseHashString");
                this.publicItemGetter = root.Q<Toggle>("PublicItemGetter");
                this.hideAssetProperties = root.Q<Toggle>("HideAssetProperties");
                this.compressColorIntoInteger = root.Q<Toggle>("CompressColorIntoInteger");
                this.generateGetMethodIfPossible = root.Q<Toggle>("GenerateGetMethodIfPossible");
                this.treatUnknowTypeAsEnum = root.Q<Toggle>("TreatUnknowTypeAsEnum");
                this.idOrKeyMultiValues = root.Q<Toggle>("IDOrKeyMultiValues");
                this.generateToStringMethod = root.Q<Toggle>("GenerateToStringMethod");
                this.nameSpace = root.Q<TextField>("NameSpace");

                this.insert = root.Q<Button>("Insert");
                this.delete = root.Q<Button>("Delete");
                this.processExcel = root.Q<Button>("ProcessExcel");

            }


            public void OnEnable()
            {
                insert.clicked += Insert_clicked;
                delete.clicked += Delete_clicked;
                processExcel.clicked += ProcessExcel_clicked;
            }


            public void OnDestroy()
            {
                insert.clicked -= Insert_clicked;
                delete.clicked -= Delete_clicked;
                processExcel.clicked -= ProcessExcel_clicked;
            }


            private void ProcessExcel_clicked()
            {
                Debug.LogError("ProcessExcel_clicked");
            }


            private void Delete_clicked()
            {
                Debug.LogError("Delete_clicked");
            }


            private void Insert_clicked()
            {
                Debug.LogError("Insert_clicked");
            }

        }



    }
}