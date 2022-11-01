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
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif
namespace ILRuntime.Runtime.Generated
{
    unsafe class WebSocket4Net_WebSocket_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(WebSocket4Net.WebSocket);
            args = new Type[]{typeof(System.EventHandler)};
            method = type.GetMethod("add_Opened", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_Opened_0);
            args = new Type[]{typeof(System.EventHandler)};
            method = type.GetMethod("add_Closed", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_Closed_1);
            args = new Type[]{typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)};
            method = type.GetMethod("add_Error", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_Error_2);
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)};
            method = type.GetMethod("add_MessageReceived", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_MessageReceived_3);
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)};
            method = type.GetMethod("add_DataReceived", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, add_DataReceived_4);
            args = new Type[]{typeof(System.EventHandler)};
            method = type.GetMethod("remove_Opened", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_Opened_5);
            args = new Type[]{typeof(System.EventHandler)};
            method = type.GetMethod("remove_Closed", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_Closed_6);
            args = new Type[]{typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)};
            method = type.GetMethod("remove_Error", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_Error_7);
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)};
            method = type.GetMethod("remove_MessageReceived", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_MessageReceived_8);
            args = new Type[]{typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)};
            method = type.GetMethod("remove_DataReceived", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, remove_DataReceived_9);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_EnableAutoSendPing", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_EnableAutoSendPing_10);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_AutoSendPingInterval", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_AutoSendPingInterval_11);
            args = new Type[]{};
            method = type.GetMethod("Open", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Open_12);
            args = new Type[]{};
            method = type.GetMethod("Close", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Close_13);
            args = new Type[]{};
            method = type.GetMethod("Dispose", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Dispose_14);
            args = new Type[]{typeof(System.Byte[]), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("Send", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Send_15);

            args = new Type[]{typeof(System.String), typeof(System.String), typeof(WebSocket4Net.WebSocketVersion)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* add_Opened_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler @value = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Opened += value;

            return __ret;
        }

        static StackObject* add_Closed_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler @value = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Closed += value;

            return __ret;
        }

        static StackObject* add_Error_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> @value = (System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Error += value;

            return __ret;
        }

        static StackObject* add_MessageReceived_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.MessageReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MessageReceived += value;

            return __ret;
        }

        static StackObject* add_DataReceived_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.DataReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DataReceived += value;

            return __ret;
        }

        static StackObject* remove_Opened_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler @value = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Opened -= value;

            return __ret;
        }

        static StackObject* remove_Closed_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler @value = (System.EventHandler)typeof(System.EventHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Closed -= value;

            return __ret;
        }

        static StackObject* remove_Error_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> @value = (System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>)typeof(System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Error -= value;

            return __ret;
        }

        static StackObject* remove_MessageReceived_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.MessageReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MessageReceived -= value;

            return __ret;
        }

        static StackObject* remove_DataReceived_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<WebSocket4Net.DataReceivedEventArgs> @value = (System.EventHandler<WebSocket4Net.DataReceivedEventArgs>)typeof(System.EventHandler<WebSocket4Net.DataReceivedEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DataReceived -= value;

            return __ret;
        }

        static StackObject* set_EnableAutoSendPing_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.EnableAutoSendPing = value;

            return __ret;
        }

        static StackObject* set_AutoSendPingInterval_11(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AutoSendPingInterval = value;

            return __ret;
        }

        static StackObject* Open_12(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Open();

            return __ret;
        }

        static StackObject* Close_13(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Close();

            return __ret;
        }

        static StackObject* Dispose_14(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Dispose();

            return __ret;
        }

        static StackObject* Send_15(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @length = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @offset = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Byte[] @data = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            WebSocket4Net.WebSocket instance_of_this_method = (WebSocket4Net.WebSocket)typeof(WebSocket4Net.WebSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Send(@data, @offset, @length);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            WebSocket4Net.WebSocketVersion @version = (WebSocket4Net.WebSocketVersion)typeof(WebSocket4Net.WebSocketVersion).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @subProtocol = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @uri = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new WebSocket4Net.WebSocket(@uri, @subProtocol, @version);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
