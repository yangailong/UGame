using ILRuntime.Runtime.Enviorment;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    public class RegisterCrossBindingAdaptorImpl :  ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            Assembly assembly = typeof(UGame).Assembly;

           // Debug.Log($"ugame:{assembly.FullName}  hot:{System.AppDomain.CurrentDomain.GetAssemblies()}");

            var crossBindingAsaptor = assembly.GetTypes().ToList().FindAll(t => t.IsSubclassOf(typeof(CrossBindingAdaptor)));

            foreach (var type in crossBindingAsaptor)
            {
                object o = Activator.CreateInstance(type);
                CrossBindingAdaptor adaptor = o as CrossBindingAdaptor;
                if (adaptor == null) continue;
                //Debug.Log($"name:{type.Name}");
                appdomain.RegisterCrossBindingAdaptor(adaptor);
            }
        }

    }

}