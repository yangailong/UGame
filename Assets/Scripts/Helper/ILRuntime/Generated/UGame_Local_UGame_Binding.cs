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
    unsafe class UGame_Local_UGame_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UGame_Local.UGame);

            field = type.GetField("Instance", flag);
            app.RegisterCLRFieldGetter(field, get_Instance_0);
            app.RegisterCLRFieldSetter(field, set_Instance_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Instance_0, AssignFromStack_Instance_0);
            field = type.GetField("cfgUGame", flag);
            app.RegisterCLRFieldGetter(field, get_cfgUGame_1);
            app.RegisterCLRFieldSetter(field, set_cfgUGame_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_cfgUGame_1, AssignFromStack_cfgUGame_1);


        }



        static object get_Instance_0(ref object o)
        {
            return UGame_Local.UGame.Instance;
        }

        static StackObject* CopyToStack_Instance_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = UGame_Local.UGame.Instance;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Instance_0(ref object o, object v)
        {
            UGame_Local.UGame.Instance = (UGame_Local.UGame)v;
        }

        static StackObject* AssignFromStack_Instance_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UGame_Local.UGame @Instance = (UGame_Local.UGame)typeof(UGame_Local.UGame).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            UGame_Local.UGame.Instance = @Instance;
            return ptr_of_this_method;
        }

        static object get_cfgUGame_1(ref object o)
        {
            return ((UGame_Local.UGame)o).cfgUGame;
        }

        static StackObject* CopyToStack_cfgUGame_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((UGame_Local.UGame)o).cfgUGame;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_cfgUGame_1(ref object o, object v)
        {
            ((UGame_Local.UGame)o).cfgUGame = (UGame_Local.CfgUGame)v;
        }

        static StackObject* AssignFromStack_cfgUGame_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UGame_Local.CfgUGame @cfgUGame = (UGame_Local.CfgUGame)typeof(UGame_Local.CfgUGame).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((UGame_Local.UGame)o).cfgUGame = @cfgUGame;
            return ptr_of_this_method;
        }



    }
}
