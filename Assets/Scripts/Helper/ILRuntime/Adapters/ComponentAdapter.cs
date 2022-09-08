using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class ComponentAdapter : CrossBindingAdaptor
{

    public override Type BaseCLRType => typeof(Component);

    public override Type AdaptorType => typeof(Adaptor);

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        throw new NotImplementedException();
    }


    public class Adaptor : Component, CrossBindingAdaptorType
    {
        public Adaptor() { }

        private ILTypeInstance instance;

        AppDomain appDomain;

        public Adaptor(AppDomain appDomain, ILTypeInstance instance)
        {
            this.appDomain = appDomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } set { instance = value; } }

        public AppDomain AppDomain { get { return AppDomain; } set { appDomain = value; } }


        public override string ToString()
        {
            IMethod m = appDomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
            {
                return instance.Type.FullName;
            }
        }
    }
}


