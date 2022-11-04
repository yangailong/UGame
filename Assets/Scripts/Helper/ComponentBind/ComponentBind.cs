using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 绑定热更脚本
/// </summary>
namespace UGame_Local
{
    public class ComponentBind : MonoBehaviour
    {

        [Tooltip("热更脚本命名空间"), FormerlySerializedAs("m_ClassNamespace")]
        public string m_ClassNamespace = "UGame_Remove";

        [Tooltip("热更脚本名"), FormerlySerializedAs("m_ClassName")]
        public string m_ClassName = string.Empty;


        private void Awake()
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(m_ClassNamespace) || string.IsNullOrEmpty(m_ClassName))
            {
                Debug.LogError($"m_ClassNamespace or m_ClassName Cannot be empty");

                return;
            }

            UGame.appDomain.LoadedTypes.TryGetValue($"{m_ClassNamespace}.{m_ClassName}", out var type);

            var clrInstance = gameObject.AddComponent<MonoBehaviourAdapter.Adaptor>();

            clrInstance.ILInstance = new ILTypeInstance(type as ILType, false);

            clrInstance.AppDomain = UGame.appDomain;

            var awake = clrInstance.ILInstance.Type.GetMethod("Awake", 0);

            UGame.appDomain.Invoke(awake, clrInstance.ILInstance);

            Destroy(this);
        }

    }
}
