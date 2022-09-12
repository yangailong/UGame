using UnityEngine;

namespace UGame_Remove
{
    /// <summary>单例Component基类</summary>
    public class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
    {

        private static T instance = null;


        public static bool Exists => instance != null;


        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindInstance() ?? CreateNewSingleton();
                }
                return instance;
            }
        }


        private static T FindInstance()
        {

#if UNITY_EDITOR
            foreach (T cb in Resources.FindObjectsOfTypeAll(typeof(T)))
            {
                if (!UnityEditor.EditorUtility.IsPersistent(cb.gameObject.transform.root.gameObject)
                 && !(cb.gameObject.hideFlags == HideFlags.NotEditable || cb.gameObject.hideFlags == HideFlags.HideAndDontSave))
                {
                    return cb;
                }
            }

            return null;
#else

            return FindObjectOfType<T>();
#endif
        }


        private static T CreateNewSingleton()
        {
            var go = new GameObject($"[{typeof(T).Name}]");

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(go);
                go.hideFlags = HideFlags.DontSave;
            }
            else
            {
                go.hideFlags = HideFlags.HideAndDontSave;
            }

            return go.AddComponent<T>();
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this as T;
        }


        public static void DestroySingleton()
        {
            if (Exists)
            {
                DestroyImmediate(Instance?.gameObject);
                instance = null;
            }
        }



#if UNITY_EDITOR

        void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
        }

        void PlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                DestroySingleton();
            }
        }

#endif


    }
}