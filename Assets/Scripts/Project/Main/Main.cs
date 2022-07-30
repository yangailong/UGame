using UnityEngine;
using UnityEngine.UI;
namespace UGame_Local
{
    public class Main : MonoBehaviour
    {
        private HotFixAssembly hotFixAssembly = null;


        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            hotFixAssembly.Start();
        }


        private void Init()
        {
            hotFixAssembly = new HotFixAssembly();
        }



        private void OnApplicationQuit()
        {
            hotFixAssembly.Close();
        }
    }
}