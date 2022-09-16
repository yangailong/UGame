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
    unsafe class UGame_Local_AssetsMappingConst_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UGame_Local.AssetsMappingConst);

            field = type.GetField("needListenerAssetsRootPath", flag);
            app.RegisterCLRFieldGetter(field, get_needListenerAssetsRootPath_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_needListenerAssetsRootPath_0, null);
            field = type.GetField("creatPath", flag);
            app.RegisterCLRFieldGetter(field, get_creatPath_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_creatPath_1, null);
            field = type.GetField("namePathSplit", flag);
            app.RegisterCLRFieldGetter(field, get_namePathSplit_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_namePathSplit_2, null);


        }



        static object get_needListenerAssetsRootPath_0(ref object o)
        {
            return UGame_Local.AssetsMappingConst.needListenerAssetsRootPath;
        }

        static StackObject* CopyToStack_needListenerAssetsRootPath_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UGame_Local.AssetsMappingConst.needListenerAssetsRootPath;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static object get_creatPath_1(ref object o)
        {
            return UGame_Local.AssetsMappingConst.creatPath;
        }

        static StackObject* CopyToStack_creatPath_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UGame_Local.AssetsMappingConst.creatPath;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static object get_namePathSplit_2(ref object o)
        {
            return UGame_Local.AssetsMappingConst.namePathSplit;
        }

        static StackObject* CopyToStack_namePathSplit_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UGame_Local.AssetsMappingConst.namePathSplit;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = (int)result_of_this_method;
            return __ret + 1;
        }



    }
}
