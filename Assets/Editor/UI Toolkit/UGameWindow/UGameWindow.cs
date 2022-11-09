using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UGame_Local;

namespace UGame_Local_Editor
{
    public class UGameWindow : EditorWindow
    {
        [MenuItem("Tools/UGame/CfgUGame %G")]
        public static void ShowExample()
        {
            UGameWindow wnd = GetWindow<UGameWindow>();
            wnd.titleContent = new GUIContent("UGame");
        }

        private ObjectField ObjectField = null;
        private TextField textField = null;
        private EnumField enumField = null;
        private Toggle toggle = null;
        private Button button = null;

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;


            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/UGameWindow/UGameWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            ObjectField = root.Q<ObjectField>("UGame");
            textField = root.Q<TextField>("Key");
            enumField = root.Q<EnumField>("ILJITFlags");
            toggle = root.Q<Toggle>("UsePdb");
            button = root.Q<Button>("Confirm");

            textField.value = EditorPrefs.GetString("UGameSecretKey", "UGame.Secret.Key");

            var cfgUGame = AssetDatabase.LoadAssetAtPath<CfgUGame>($"Assets/AddressableAssets/Local/Data/ScriptableObject/Custom/UGame.asset");

            ObjectField.objectType = typeof(CfgUGame);
            ObjectField.allowSceneObjects = false;
            ObjectField.value = cfgUGame;

            enumField.Init(ILRuntimeJITFlags.None);
            enumField.value = cfgUGame.jITFlags;

            toggle.value = cfgUGame.usePdb;

            button.clicked += Confirm_clicked;

        }


        private void Confirm_clicked()
        {
            if (ObjectField != null && ObjectField.value != null)
            {
                CfgUGame cfg = ObjectField.value as CfgUGame;

                var key = textField.text;
                cfg.key = CryptoManager.MD5Encrypt(key);
                cfg.jITFlags = (ILRuntimeJITFlags)enumField.value;
                cfg.usePdb = toggle.value;

                EditorPrefs.SetString("UGameSecretKey", textField.value);

                //修改密码后需要重新加密dll文件
                DllToBytes.DLLToBytes();

                EditorUtility.SetDirty(cfg);

                Debug.Log($" key:{textField.value} to md5:{cfg.key}  save success......  ");

            }

        }
    }
}

