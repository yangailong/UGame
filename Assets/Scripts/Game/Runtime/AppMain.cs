using UnityEngine;

namespace UGame_Local
{
    public class AppMain : MonoBehaviour
    {
        public static HotFixAssembly hotFixAssembly = new HotFixAssembly();

        private void Awake()
        {
            hotFixAssembly.Run();
        }

      
    }
}