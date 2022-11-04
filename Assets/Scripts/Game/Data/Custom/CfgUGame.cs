using UnityEngine;
namespace UGame_Local
{
    [CreateAssetMenu(fileName ="UGame",menuName ="UGame/CfgUGame")]
    public class CfgUGame : ScriptableObject
    {
        public string m_Key = "UGame.Prefs.Keys";

        public ILRuntimeJITFlags jITFlags = ILRuntimeJITFlags.None;

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