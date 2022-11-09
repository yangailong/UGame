using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ProcessExcelWindow : EditorWindow
{
    [MenuItem("Tools/UGame/Excel表数据转ScriptableObject %E")]
    public static void ShowExample()
    {
        ProcessExcelWindow wnd = GetWindow<ProcessExcelWindow>();
        wnd.titleContent = new GUIContent("ProcessExcelWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ProcessExcelWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);


    }
}