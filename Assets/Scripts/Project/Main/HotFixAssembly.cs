using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System.IO;

public class HotFixAssembly 
{
    private AppDomain appdomain = null;
 
    public HotFixAssembly()
    {
        appdomain = new AppDomain();
    }

    public void Start()
    {
        Load();
        InitializeILRuntime();
        OnHotFixLoaded();
    }

    MemoryStream fs = null;
    MemoryStream p=null;


    public void Load()
    {
        //获取dll
        byte[] dll = null;//www.bytes
         fs = new MemoryStream(dll);


        //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
        byte[] pdb = null; //www.bytes;
        MemoryStream p = new MemoryStream(pdb);

        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
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
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

    }

    private void OnHotFixLoaded()
    {
        appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);
    }


    public void Close()
    {
        fs?.Close();
        p?.Close();
    }
}
