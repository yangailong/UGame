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
    unsafe class UGame_Local_CfgUGame_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UGame_Local.CfgUGame);

            field = type.GetField("key", flag);
            app.RegisterCLRFieldGetter(field, get_key_0);
            app.RegisterCLRFieldSetter(field, set_key_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_key_0, AssignFromStack_key_0);


        }



        static object get_key_0(ref object o)
        {
            return ((UGame_Local.CfgUGame)o).key;
        }

        static StackObject* CopyToStack_key_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((UGame_Local.CfgUGame)o).key;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_key_0(ref object o, object v)
        {
            ((UGame_Local.CfgUGame)o).key = (System.String)v;
        }

        static StackObject* AssignFromStack_key_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @key = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((UGame_Local.CfgUGame)o).key = @key;
            return ptr_of_this_method;
        }



    }
}
