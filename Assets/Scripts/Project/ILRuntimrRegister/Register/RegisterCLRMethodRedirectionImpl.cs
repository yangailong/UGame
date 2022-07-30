using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    public class RegisterCLRMethodRedirectionImpl : Singleton<RegisterCLRMethodRedirectionImpl>, ILRuntimeRegister
    {
        public unsafe void Register(AppDomain appDomain)
        {
            Type[] args;
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                                BindingFlags.DeclaredOnly;

            //注册Add Component
            Type gameObjectType = typeof(GameObject);
            Type componentType = typeof(Component);
            var addComponentMethod = gameObjectType.GetMethods().ToList().Find(m => m.Name == "AddComponent" && m.GetGenericArguments().Length == 1);
            appDomain.RegisterCLRMethodRedirection(addComponentMethod, AddComponent);

            var addComponentMethod2 = gameObjectType.GetMethods().ToList().Find(i => i.Name == "AddComponent" && i.GetGenericArguments().Length != 1);
            appDomain.RegisterCLRMethodRedirection(addComponentMethod2, AddComponent2);


            //注册get，有2种get component，一个是GameObject调用，一个是脚本调用
            var getComponentMethod = gameObjectType.GetMethods().ToList().Find(i => i.Name == "GetComponent" && i.GetGenericArguments().Length == 1);
            appDomain.RegisterCLRMethodRedirection(getComponentMethod, GetComponent);

            var getComponentMethod2 = componentType.GetMethods().ToList().Find(i => i.Name == "GetComponent" && i.GetGenericArguments().Length == 1);
            appDomain.RegisterCLRMethodRedirection(getComponentMethod2, GetComponent);


        }


        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = DoAddComponent(type, instance, __domain);

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }


        unsafe static StackObject* AddComponent2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            AppDomain __domain = __intp.AppDomain;

            var ptr = ILIntepreter.Minus(__esp, 2);
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new NullReferenceException();
            __intp.Free(ptr);

            //成员方法的第2个参数为Type
            var ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Type _type = (Type)typeof(Type).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);
            if (_type == null)
                throw new NullReferenceException();

            IType type = __domain.LoadedTypes[_type.FullName];
            object res = DoAddComponent(type, instance, __domain);

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }


        private static object DoAddComponent(IType type, GameObject instance, AppDomain __domain)
        {
            object res;
            if (type is CLRType)
            {
                //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                res = instance.AddComponent(type.TypeForCLR);
            }
            else
            {
                //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                ILTypeInstance ilInstance = new ILTypeInstance(type as ILType, false);
                Type t = type.ReflectionType;
                bool isMonoAdapter = t.BaseType?.FullName == typeof(MonoBehaviourAdapter.Adaptor).FullName;

                if (!isMonoAdapter && Type.GetType(t.BaseType.FullName) != null)
                {
                    Type adapterType = Type.GetType(t.BaseType?.FullName);
                    if (adapterType == null)
                    {
                        Debug.LogError($"{t.FullName}, need to generate adapter");
                        return null;
                    }

                    //直接反射赋值一波了
                    var clrInstance = instance.AddComponent(adapterType);
                    var ILInstance = t.GetField("instance",
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var AppDomain = t.GetField("appdomain",
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    ILInstance.SetValue(clrInstance, ilInstance);
                    AppDomain.SetValue(clrInstance, __domain);
                    ilInstance.CLRInstance = clrInstance;
                    bool activated = false;
                    //不管是啥类型，直接invoke这个awake方法
                    var awakeMethod = clrInstance.GetType().GetMethod("Awake",
                        BindingFlags.Default | BindingFlags.Public
                                             | BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                                             BindingFlags.NonPublic | BindingFlags.Static);
                    if (awakeMethod == null)
                    {
                        awakeMethod = t.GetMethod("Awake",
                            BindingFlags.Default | BindingFlags.Public
                                                 | BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                                                 BindingFlags.NonPublic | BindingFlags.Static);
                    }
                    else
                    {
                        awakeMethod.Invoke(clrInstance, null);
                        activated = true;
                    }

                    if (awakeMethod == null)
                    {
                        Debug.LogError($"{t.FullName}不包含Awake方法，无法激活，已跳过");
                    }
                    else if (!activated)
                    {
                        awakeMethod.Invoke(t, null);
                    }
                }
                else
                {
                    //接下来创建Adapter实例
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    clrInstance.ILInstance = ilInstance;
                    clrInstance.AppDomain = __domain;
                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;
                    clrInstance.Awake(); //因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                res = ilInstance;

                var m = type.GetConstructor(Extensions.EmptyParamList);
                if (m != null)
                {
                    __domain.Invoke(m, res, null);
                }
            }
            return res;
        }



        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.GetComponent(type.TypeForCLR);
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
                            if (clrInstance.ILInstance.Type == type)
                            {
                                res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }




    }
}