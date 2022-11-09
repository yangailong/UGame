using UnityEngine;
namespace UGame_Local
{
    [CreateAssetMenu(fileName = "UGame", menuName = "UGame/CfgUGame")]
    public class CfgUGame : ScriptableObject
    {

        /// <summary>
        /// ������Կ.�������ݺ�dll
        /// </summary>
        public string key = null;


        /// <summary>
        /// ILRuntime�Ĵ���ģʽ
        /// </summary>
        public ILRuntimeJITFlags jITFlags = ILRuntimeJITFlags.None;


        /// <summary>
        ///   PDB�ļ��ǵ������ݿ⣬����Ҫ����־����ʾ������кţ�������ṩPDB�ļ����������ڻ��������ڴ棬��ʽ����ʱ�뽫PDBȥ��
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