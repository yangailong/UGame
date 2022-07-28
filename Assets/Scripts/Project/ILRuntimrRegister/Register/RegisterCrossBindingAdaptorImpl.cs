using ILRuntime.Runtime.Enviorment;
using UnityEngine;

public class RegisterCrossBindingAdaptorImpl : ILRuntimeRegister
{
    protected override void Register(AppDomain appDomain)
    {
        Debug.Log("-------------------");
        appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
    }


}

