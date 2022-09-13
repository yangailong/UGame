using System.Collections;
using UnityEngine.ResourceManagement.Util;

namespace UGame_Local
{
    public class MonoBehaviourRuntime : ComponentSingleton<MonoBehaviourRuntime>
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

