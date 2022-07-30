using ILRuntime.Runtime.Enviorment;
using UnityEngine;
namespace UGame_Local
{
    public class RegisterCrossBindingAdaptorImpl : Singleton<RegisterCrossBindingAdaptorImpl>, ILRuntimeRegister
    {
        public void Register(AppDomain appDomain)
        {
            appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        }

    }

}