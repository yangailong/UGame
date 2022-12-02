using UnityEditor;
using UnityEngine;

namespace UGame_Local_Editor
{
    public class AssetsMapperTool
    {
        [MenuItem("Tools/UGame/重新生成资源映射表【此方法是引擎自动调用，防止出错，预留手动入口】")]
        public static void AssetsMappingRefresh()
        {
            AssetsMapperImpl.Creat();

            Debug.Log("成功生成资源映射表");
        }
    }
}
