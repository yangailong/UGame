using UnityEditor;

namespace UGame_Local_Editor
{
    public class AssetsMapperTool
    {
        [MenuItem("Tools/Assets/AssetsMapper/资源重命名检测")]
        [MenuItem("Assets/Refresh AssetsMapper  %M")]
        public static void AssetsMappingRefresh()
        {
            AssetsMapperImpl.Creat();
        }
    }
}
