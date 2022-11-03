using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UGame_Local_Editor
{
    public class UGame : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/UGame")]
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

            //root.Q<EnumField>("ILRuntimeJITFlags").Init(ILRuntimeJITFlags.None);
        }

        public enum ILRuntimeJITFlags
        {

            None = 0,

            JITOnDemand = 1,

            JITImmediately = 2,

            NoJIT = 4,

            ForceInline = 8
        }


    }

   
}