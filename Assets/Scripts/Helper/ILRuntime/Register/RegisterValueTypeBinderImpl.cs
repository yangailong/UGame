using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using UnityEngine;
namespace UGame_Local
{
    public class RegisterValueTypeBinderImpl : ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
        }

    }

}