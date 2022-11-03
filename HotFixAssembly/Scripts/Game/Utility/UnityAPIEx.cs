using UGame_Local;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGame_Remove
{
    public static class UnityAPIEx
    {

        public static T GetMountChind<T>(this Transform transform, string childName) where T : Component
        {
            var com = transform.GetComponent<MountObject>();

            return com == null ? null : com.GetChild<T>(childName);
        }


        public static Object GetMountOther(this MonoBehaviour mono, int index)
        {
            var com = mono.GetComponent<MountObject>();

            return com == null ? null : com.GetOther(index);
        }




    }
}
