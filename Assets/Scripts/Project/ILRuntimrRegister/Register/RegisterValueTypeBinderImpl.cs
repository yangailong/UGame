using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class RegisterValueTypeBinderImpl : ILRuntimeRegister
{
    protected override void Register(AppDomain appDomain)
    {
        appDomain.RegisterValueTypeBinder(typeof(Vector2),new Vector2Binder());
        appDomain.RegisterValueTypeBinder(typeof(Vector3),new Vector3Binder());
    }

}

