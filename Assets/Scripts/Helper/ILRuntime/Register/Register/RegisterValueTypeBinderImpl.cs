using ILRuntime.Runtime.Enviorment;
using UnityEngine;
namespace UGame_Local
{
    public class RegisterValueTypeBinderImpl : Singleton<RegisterValueTypeBinderImpl>, ILRuntimeRegister
    {
        public void Register(AppDomain appDomain)
        {
            appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        }

    }

}