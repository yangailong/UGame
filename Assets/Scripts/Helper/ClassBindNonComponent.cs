using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;


/// <summary>
/// 说明：
/// </summary>
public class ClassBindNonComponent : MonoBehaviour
{

}
/**
public class ClassBindNonComponentAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType => typeof(ClassBindNonComponent);

    public override Type AdaptorType => typeof(Adaptor);

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : ClassBindNonComponent, CrossBindingAdaptorType
    {

        private ILTypeInstance _instance;

        private AppDomain _appdomain;

        public ILTypeInstance ILInstance { get => _instance; set => _instance = value; }

        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            this._appdomain = appdomain;
            this._instance = instance;
        }

        private bool _destoryed;

        public bool isJBehaviour;

        IMethod _mAwakeMethod;

        public bool awaked;

        public bool isAwaking;

  

        private async void Awake()
        {
            if (awaked) return;

            try
            {
                if (_instance != null)
                {
                    if (!isAwaking)
                    {
                        isAwaking = true;

                        try
                        {
                            while (Application.isPlaying && !_destoryed && !gameObject.activeInHierarchy)
                            {
                                await Task.Delay(1);
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }

                        if (_destoryed || !Application.isPlaying)
                        {
                            return;
                        }

                        var type = _instance.Type.ReflectionType;
                        GetMethodInfo(type, "Awake")?.Invoke(_instance, ConstMgr.NullObjects);
                        if (isJBehaviour)
                        {
                            //JBehaviour额外处理
                            GetMethodInfo(type, "Check").Invoke(_instance, ConstMgr.NullObjects);
                            LifeCycleMgr.Instance.AddAwakeItem(_instance, null);//这一帧空出来
                            LifeCycleMgr.Instance.AddOnEnableItem(_instance, GetMethodInfo(type, "OnEnable"));
                            LifeCycleMgr.Instance.AddStartItem(_instance, GetMethodInfo(type, "Start"));
                        }

                        isAwaking = false;
                        awaked = true;
                    }
                }
            }
            catch (MissingReferenceException e)
            {
                //如果出现了Null，那就重新Awake
                Awake();
            }
        }


        private MethodInfo GetMethodInfo(Type type, string funcName)
        {
            if (_instance.Type.GetMethod(funcName, 0) != null)
            {
                return type.GetMethod(funcName);
            }

            return null;
        }

         

        IMethod _mToStringMethod;
        bool _mToStringMethodGot;

        public override string ToString()
        {
            if (_instance != null)
            {
                if (!_mToStringMethodGot)
                {
                    _mToStringMethod =
                        _instance.Type.GetMethod("ToString", 0);
                    _mToStringMethodGot = true;
                }

                if (_mToStringMethod != null)
                {
                    _appdomain.Invoke(_mToStringMethod, _instance, ConstMgr.NullObjects);
                }
            }

            return _instance?.Type?.FullName ?? base.ToString();
        }
        

    }
}
*/
