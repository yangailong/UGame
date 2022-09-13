using UnityEditor;

namespace UGame_Local_Editor
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
