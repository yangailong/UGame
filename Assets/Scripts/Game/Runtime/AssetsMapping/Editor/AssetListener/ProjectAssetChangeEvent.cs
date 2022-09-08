using System.Collections.Generic;
using UnityEditor;
public class ProjectAssetChangeEvent
{
    [InitializeOnLoadMethod]
    private static void EventFileChange()
    {
        ProjectAssetModificationProcessor.OnCreateAssetCallback += OnCreateAsset;
        ProjectAssetModificationProcessor.OnSaveAssetsCallback += OnSaveAsset;
        ProjectAssetModificationProcessor.OnMoveAssetCallback += OnMoveAsset;
        ProjectAssetModificationProcessor.OnDeleteAssetCallback += OnDeleteAsse;
        EditorApplication.projectChanged += OnProjectWindowChanged;
    }

    private static void OnProjectWindowChanged()
    {
        UpdateAsset(null);
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

    public static void UpdateAsset(List<string> pathArr)
    {
        if (pathArr == null || pathArr.Count == 0) return;

        bool update = false;

        foreach (var path in pathArr)
        {
            if (AssetsMappingImpl.InListenerAssetsRootPath(path))
            {
                update = true;
                break;
            }
        }

        if (!update) return;

        if (AssetsMappingImpl.IsExistAssetsRootPath())
        {
            AssetsMappingImpl.Creat();
        }

    }

}

