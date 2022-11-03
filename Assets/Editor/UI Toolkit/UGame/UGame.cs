using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UGame_Local;

namespace UGame_Local_Editor
{
    public class UGame : EditorWindow
    {
        [MenuItem("Tools/UGame")]
        public static void ShowExample()
        {
            UGame wnd = GetWindow<UGame>();
            wnd.titleContent = new GUIContent("UGame");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
          

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/UGame/UGame.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            root.Q<EnumField>("ILJITFlags").Init(ILRuntimeJITFlags.None);
        }
    }
}