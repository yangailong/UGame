using UnityEditor;
using UnityEngine;

namespace UGame_Local
{
    public class AssetsMappingTool
    {
        [MenuItem("Tools/Assets/AssetsMapping/资源重命名检测")]
        [MenuItem("Assets/Refresh AssetsMapping  %M")]
        public static void AssetsMappingRefresh()
        {
            AssetsMappingImpl.Creat();
        }
    }
}
