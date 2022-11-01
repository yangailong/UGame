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


    private ObjectField ObjectField = null;


    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/SceneObjectWindow.uxml");


        visualTree.CloneTree(root);

        var helpBox = new HelpBox("ͯЬ�ǣ������£�����һ����ʾ", HelpBoxMessageType.None);
        var helpBox2 = new HelpBox("ͯЬ�ǣ������£�����һ����ʾ", HelpBoxMessageType.Info);
        var helpBox3 = new HelpBox("ͯЬ�ǣ������£�����һ����ʾ", HelpBoxMessageType.Warning);
        var helpBox4 = new HelpBox("ͯЬ�ǣ������£�����һ����ʾ", HelpBoxMessageType.Error);

        var right = root.Q<VisualElement>("right");

        right.Add(helpBox);
        right.Add(helpBox2);
        right.Add(helpBox3);
        right.Add(helpBox4);

        ObjectField = root.Q<ObjectField>("ObjectField");
        ObjectField.objectType = typeof(Texture2D);
    }
}