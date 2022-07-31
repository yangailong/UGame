#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.CLRBinding;
using UGame_Local;
[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    [MenuItem("Tools/ILRuntime/通过自动分析热更DLL生成CLR绑定")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        AppDomain domain = new AppDomain();
        using (FileStream fs = new FileStream("Assets/AddressableAssets/Remote/Dll/HotFixAssembly.dll", FileMode.Open, FileAccess.Read))
        {
            domain.LoadAssembly(fs);

            //Crossbind Adapter is needed to generate the correct binding code
            InitILRuntime(domain);

            BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Scripts/ILRuntime/Generated");
        }

        AssetDatabase.Refresh();
    }

    static void InitILRuntime(AppDomain domain)
    {
        //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
        RegisterCrossBindingAdaptorImpl.Instance.Register(domain);
    }
}
#endif
