#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using ILRuntime.Runtime.CLRBinding;
using UGame_Local;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local_Editor
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ILRuntimeCLRBinding
    {
        [MenuItem("Tools/ILRuntime/通过自动分析热更DLL生成CLR绑定")]
        static void GenerateCLRBindingByAnalysis()
        {
            //用新的分析热更dll调用引用来生成绑定代码
            AppDomain domain = new AppDomain();
            using (FileStream fs = new FileStream("Assets/AddressableAssets/Remote_UnMapper/Dll/Dll~/HotFixAssembly.dll", FileMode.Open, FileAccess.Read))
            {
                domain.LoadAssembly(fs);

                //Crossbind Adapter is needed to generate the correct binding code
                InitILRuntime(domain);

                BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Scripts/Helper/ILRuntime/Generated");
            }

            AssetDatabase.Refresh();
        }



        [MenuItem("Tools/ILRuntime/删除所有CLR绑定")]
        static void DeleteCLRBindins()
        {
            Directory.Delete("Assets/Scripts/Helper/ILRuntime/Generated", true);
            AssetDatabase.Refresh();
        }


        static void InitILRuntime(AppDomain domain)
        {
            //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
            RegisterCrossBindingAdaptorImpl.Instance.Register(domain);
            RegisterCLRMethodRedirectionImpl.Instance.Register(domain);
            RegisterDelegateConvertorImpl.Instance.Register(domain);
            RegisterFunctionDelegateImpl.Instance.Register(domain);
            RegisterMethodDelegateImpl.Instance.Register(domain);
            RegisterValueTypeBinderImpl.Instance.Register(domain);
            RegisterLitJsonImpl.Instance.Register(domain);
        }
    }
}
#endif
