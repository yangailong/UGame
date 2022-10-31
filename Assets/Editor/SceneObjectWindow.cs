using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class SceneObjectWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/SceneObjectWindow")]
    public static void ShowExample()
    {
        SceneObjectWindow wnd = GetWindow<SceneObjectWindow>();
        wnd.titleContent = new GUIContent("SceneObjectWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

      
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/SceneObjectWindow.uxml");


        visualTree.CloneTree(root);


    }
}