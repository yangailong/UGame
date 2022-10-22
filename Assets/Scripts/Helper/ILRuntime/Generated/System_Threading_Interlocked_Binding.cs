using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class System_Threading_Interlocked_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Threading.Interlocked);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(System.EventHandler)};
            if (genericMethods.TryGetValue("CompareExchange", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.EventHandler), typeof(System.EventHandler).MakeByRefType(), typeof(System.EventHandler), typeof(System.EventHandler)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, CompareExchange_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)};
            if (genericMethods.TryGetValue("CompareExchange", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>), typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).MakeByRefType(), typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>), typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, CompareExchange_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)};
            if (genericMethods.TryGetValue("CompareExchange", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>), typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).MakeByRefType(), typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>), typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, CompareExchange_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)};
            if (genericMethods.TryGetValue("CompareExchange", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>), typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).MakeByRefType(), typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>), typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, CompareExchange_3);

                        break;
                    }
                }
            }


        }


        static StackObject* CompareExchange_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler @comparand = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.EventHandler @value = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.EventHandler @location1 = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(__intp.RetriveObject(ptr_of_this_method, __mStack), (CLR.Utils.Extensions.TypeFlags)8);


            var result_of_this_method = System.Threading.Interlocked.CompareExchange<System.EventHandler>(ref @location1, @value, @comparand);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        object ___obj = @location1;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @location1);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @location1);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.EventHandler[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @location1;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CompareExchange_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> @comparand = (System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> @value = (System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> @location1 = (System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).CheckCLRTypes(__intp.RetriveObject(ptr_of_this_method, __mStack), (CLR.Utils.Extensions.TypeFlags)8);


            var result_of_this_method = System.Threading.Interlocked.CompareExchange<System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>>(ref @location1, @value, @comparand);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        object ___obj = @location1;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @location1);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @location1);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @location1;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CompareExchange_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.DataReceivedEventArgs> @comparand = (System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.EventHandler<WebSocket4Net.DataReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.EventHandler<WebSocket4Net.DataReceivedEventArgs> @location1 = (System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).CheckCLRTypes(__intp.RetriveObject(ptr_of_this_method, __mStack), (CLR.Utils.Extensions.TypeFlags)8);


            var result_of_this_method = System.Threading.Interlocked.CompareExchange<System.EventHandler<WebSocket4Net.DataReceivedEventArgs>>(ref @location1, @value, @comparand);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        object ___obj = @location1;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @location1);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @location1);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.EventHandler<WebSocket4Net.DataReceivedEventArgs>[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @location1;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CompareExchange_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.MessageReceivedEventArgs> @comparand = (System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.EventHandler<WebSocket4Net.MessageReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.EventHandler<WebSocket4Net.MessageReceivedEventArgs> @location1 = (System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).CheckCLRTypes(__intp.RetriveObject(ptr_of_this_method, __mStack), (CLR.Utils.Extensions.TypeFlags)8);


            var result_of_this_method = System.Threading.Interlocked.CompareExchange<System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>>(ref @location1, @value, @comparand);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        object ___obj = @location1;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @location1);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @location1;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @location1);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @location1;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
