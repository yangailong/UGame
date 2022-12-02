using UnityEngine;
namespace UGame_Local
{
    [CreateAssetMenu(fileName = "UGame", menuName = "UGame/CfgUGame")]
    public class CfgUGame : ScriptableObject
    {

        /// <summary>
        /// 加密密钥.加密数据和dll
        /// </summary>
        public string key = null;


        /// <summary>
        /// ILRuntime寄存器模式
        /// </summary>
        public ILRuntimeJITFlags jITFlags = ILRuntimeJITFlags.None;


        /// <summary>
        ///   PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉
        /// </summary>
        public bool usePdb = true;

      
    }


    public enum ILRuntimeJITFlags
    {
        None = 0,

        /// <summary>
        /// 按需JIT模式，使用该模式在默认的情况下会按照原始的方式运行，当该方法被反复执行时，会被标记为需要被JIT，并在后台线程完成JIT编译后切换到寄存器模式运行
        /// </summary>
        JITOnDemand = 1,

        /// <summary>
        /// 立即JIT模式，使用该模式时，当方法被调用的瞬间即会被执行JIT编译，在第一次执行时即使用寄存器模式运行。
        /// </summary>
        JITImmediately = 2,

        /// <summary>
        /// 禁用JIT模式，该方法在执行时会始终以传统方式执行
        /// </summary>
        NoJIT = 4,

        /// <summary>
        /// 强制内联模式，该模式只对方法的Attribute生效，标注该模式的方法在被调用时将会无视方法体内容大小，强制被内联
        /// </summary>
        ForceInline = 8,



/*        使用建议
ILRuntime推荐的使用模式有2种：

AppDomain构造函数时不指定JIT模式，即默认使用传统模式执行，在遇到就要优化的密集计算型方法时，对该方法指定JITImmediately模式
直接在AppDomain构造函数处指定JITOnDemand模式
第一种用法对现有实现影响最小，仅在需要优化处开启，可以比较精准的控制执行效果。如果并不知道在什么时候应该使用何种模式，也可以直接使用JITOnDemand模式，让ILRuntime自行决定运行模式，在大多数情况下是能达到不错的性能平衡的。

*/


    }

}