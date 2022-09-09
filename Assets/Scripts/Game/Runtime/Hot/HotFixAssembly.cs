using UnityEngine;
using System.IO;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    public class HotFixAssembly
    {
        public AppDomain appDomain = null;

        public HotFixAssembly()
        {
            appDomain = new AppDomain();
        }

        public void Run()
        {
            Load();
            InitializeILRuntime();
            OnHotFixLoaded();
        }




        public void Load()
        {
            //获取dll
            byte[] dll = DownDll.DllData();
            MemoryStream fs = new MemoryStream(dll);


            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
            byte[] pdb = DownDll.PDBData();
            MemoryStream p = new MemoryStream(pdb);

            try
            {
                appDomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch
            {
                Debug.LogError("加载热更DLL失败");
            }
        }

        private void InitializeILRuntime()
        {

#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)

            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            appDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            appDomain.DebugService.StartDebugService(56000);
#endif
            RegisterCLRMethodRedirectionImpl.Instance.Register(appDomain);
            RegisterCrossBindingAdaptorImpl.Instance.Register(appDomain);
            RegisterDelegateConvertorImpl.Instance.Register(appDomain);
            RegisterFunctionDelegateImpl.Instance.Register(appDomain);
            RegisterMethodDelegateImpl.Instance.Register(appDomain);
            RegisterValueTypeBinderImpl.Instance.Register(appDomain);
            RegisterLitJsonImpl.Instance.Register(appDomain);
        }

        private void OnHotFixLoaded()
        {
            appDomain.Invoke("UGame_Remove.RunGame", "StartUp", null, null);
        }



    }
}