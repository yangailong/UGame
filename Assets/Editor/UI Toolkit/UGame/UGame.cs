using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UGame_Local;
using System.Globalization;

namespace UGame_Local_Editor
{
    public class UGame : EditorWindow
    {
        [MenuItem("Tools/UGame/CfgUGame %G")]
        public static void ShowExample()
        {
            UGame wnd = GetWindow<UGame>();
            wnd.titleContent = new GUIContent("UGame");
        }

        private ObjectField cfgUgame = null;

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;


            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/UGame/UGame.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            root.Q<EnumField>("ILJITFlags").Init(ILRuntimeJITFlags.None);

            cfgUgame = root.Q<ObjectField>("UGame");
            cfgUgame.objectType = typeof(CfgUGame);
            cfgUgame.allowSceneObjects = false;

            cfgUgame.value = AssetDatabase.LoadAssetAtPath<CfgUGame>($"Assets/UGame.asset");

            root.Q<Button>("Confirm").clicked += Confirm_clicked;
        }


        private void Confirm_clicked()
        {
            if (cfgUgame != null && cfgUgame.value != null)
            {
                CfgUGame cfg = cfgUgame.value as CfgUGame;

                var key = rootVisualElement.Q<TextField>("Key").text;
                cfg.md5Key = CryptoManager.MD5Encrypt(key);
                cfg.jITFlags = (ILRuntimeJITFlags)rootVisualElement.Q<EnumField>("ILJITFlags").value;
                cfg.usePdb = rootVisualElement.Q<Toggle>("UsePdb").value;
            }

        }
    }
}

