using System.Collections;
using UnityEngine.ResourceManagement.Util;

namespace UGame_Local
{
    public class MonoBehaviourRuntime : ComponentSingleton<MonoBehaviourRuntime>
    {
        public void DoCoroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}

