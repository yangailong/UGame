using System.Collections.Generic;
using UnityEditor;

namespace UGame_Local_Editor
{
    public class ProjectAssetChangeEvent
    {
        [InitializeOnLoadMethod]
        private static void EventFileChange()
        {
            ProjectAssetModificationProcessor.OnCreateAssetCallback += OnCreateAsset;
            ProjectAssetModificationProcessor.OnSaveAssetsCallback += OnSaveAsset;
            ProjectAssetModificationProcessor.OnMoveAssetCallback += OnMoveAsset;
            ProjectAssetModificationProcessor.OnDeleteAssetCallback += OnDeleteAsse;
        }

        private static void OnCreateAsset(string path)
        {
            List<string> pathArr = new List<string>();
            pathArr.Add(path);
            UpdateAsset(pathArr);
        }

        private static void OnSaveAsset(string[] paths)
        {
            List<string> pathArr = new List<string>();
            pathArr.AddRange(paths);
            UpdateAsset(pathArr);
        }

        private static void OnMoveAsset(AssetMoveResult ar, string path1, string path2)
        {
            List<string> pathArr = new List<string>();
            pathArr.Add(path1);
            pathArr.Add(path2);
            UpdateAsset(pathArr);
        }

        private static void OnDeleteAsse(AssetDeleteResult ar, string path1, RemoveAssetOptions ra)
        {
            List<string> pathArr = new List<string>();
            pathArr.Add(path1);
            UpdateAsset(pathArr);
        }

        public static void UpdateAsset(List<string> paths)
        {
            UnityEngine.Debug.LogError("-----------");

            if (paths == null || paths.Count == 0) return;

            bool update = false;

            foreach (var path in paths)
            {
                if (AssetsMapperImpl.InListenerAssetsRootPath(path))
                {
                    update = true;
                    break;
                }
            }

            if (!update) return;

            if (AssetsMapperImpl.IsExistAssetsRootPath())
            {
                AssetsMapperImpl.Creat();
            }

        }


    }
}

