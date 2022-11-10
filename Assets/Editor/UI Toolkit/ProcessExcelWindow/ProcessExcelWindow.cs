using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.ComponentModel;
using System.Drawing;

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


        private ProcessExcelImpl impl = null;

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ProcessExcelWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            impl = new ProcessExcelImpl();

            TextElement textElement = new Button() { text = "C# Button" };
            textElement.name = "Button";
            textElement.text = "CfgLanager";
            //textElement  ProcessExcelImpl
            root.Q<ScrollView>().Add(textElement);


        }
    }
}