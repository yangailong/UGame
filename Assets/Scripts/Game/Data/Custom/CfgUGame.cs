using UnityEngine;
namespace UGame_Local
{
    [CreateAssetMenu(fileName = "UGame", menuName = "UGame/CfgUGame")]
    public class CfgUGame : ScriptableObject
    {
        public string key { set; get; }


        public ILRuntimeJITFlags jITFlags { set; get; }= ILRuntimeJITFlags.None;


        public bool usePdb { set; get; }= true;


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