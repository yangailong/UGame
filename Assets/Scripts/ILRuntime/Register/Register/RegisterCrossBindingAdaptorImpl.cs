using ILRuntime.Runtime.Enviorment;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    public class RegisterCrossBindingAdaptorImpl : Singleton<RegisterCrossBindingAdaptorImpl>, ILRuntimeRegister
    {
        public void Register(AppDomain appDomain)
        {
            Assembly assembly = typeof(Main).Assembly;
            var crossBindingAsaptor = assembly.GetTypes().ToList().FindAll(t => t.IsSubclassOf(typeof(CrossBindingAdaptor)));

            foreach (var type in crossBindingAsaptor)
            {
                object o = Activator.CreateInstance(type);
                CrossBindingAdaptor adaptor = o as CrossBindingAdaptor;
                if (adaptor == null) continue;
                appDomain.RegisterCrossBindingAdaptor(adaptor);
            }
        }

    }

}