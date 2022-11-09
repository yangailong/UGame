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

        JITOnDemand = 1,

        JITImmediately = 2,

        NoJIT = 4,

        ForceInline = 8,
    }

}