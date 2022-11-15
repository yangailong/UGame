﻿using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    public class RegisterFunctionDelegateImpl : ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Type, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Type, System.String>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Component>();
            appdomain.DelegateManager.RegisterFunctionDelegate<global::IMessageAdapt.Adaptor>();

        }
    }

}