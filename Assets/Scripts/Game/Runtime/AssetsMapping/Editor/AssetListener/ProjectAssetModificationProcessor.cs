using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProjectAssetModificationProcessor : AssetModificationProcessor
{

    [InitializeOnLoadMethod]
    private static void Add()
    {
        EditorApplication.update += ProjectWindowChanged;
    }

    private static object[] onCreateAsset = null;
    private static object[] onSaveAssets = null;
    private static object[] onMoveAsset = null;
    private static object[] onDeleteAsset = null;

    public static Action<string> OnCreateAssetCallback = null;
    public static Action<string[]> OnSaveAssetsCallback = null;
    public static Action<AssetMoveResult, string, string> OnMoveAssetCallback = null;
    public static Action<AssetDeleteResult, string, RemoveAssetOptions> OnDeleteAssetCallback = null;

    private static void ProjectWindowChanged()
    {
        if (onCreateAsset != null)
        {
            OnCreateAssetCallback?.Invoke(onCreateAsset[0] as string);
            onCreateAsset = null;
        }

        if (onSaveAssets != null)
        {
            OnSaveAssetsCallback?.Invoke(onSaveAssets[0] as string[]);
            onSaveAssets = null;
        }

        if (onMoveAsset != null)
        {
            OnMoveAssetCallback?.Invoke((AssetMoveResult)onMoveAsset[0], onMoveAsset[1] as string, onMoveAsset[2] as string);
            onMoveAsset = null;
        }

        if (onDeleteAsset != null)
        {
            OnDeleteAssetCallback?.Invoke((AssetDeleteResult)onDeleteAsset[0], onDeleteAsset[1] as string, (RemoveAssetOptions)onDeleteAsset[2]);
            onDeleteAsset = null;
        }
    }

    /// <summary>有新资源</summary>
    private static Action<string> OnWillCreateAssetCallback = null;
    private static void OnWillCreateAsset(string path)
    {
        OnWillCreateAssetCallback?.Invoke(path);
        onCreateAsset = new object[] { path };
    }


    /// <summary>保存资源</summary>
    public static Func<string[], string[]> OnWillSaveAssetsCallback = null;
    private static string[] OnWillSaveAssets(string[] paths)
    {
        OnWillSaveAssetsCallback?.Invoke(paths);
        List<string> result = new List<string>();
        foreach (string path in paths)
        {
            if (IsUnlocked(path))
            {
                result.Add(path);
            }
            else
            {
                Debug.LogError($"{path} is read-only");
            }
        }
        onSaveAssets = new object[] { result.ToArray() };

        // Debug.Log($"OnWillSaveAssets :{EditorApplication.timeSinceStartup}    {paths.Length}");
        return result.ToArray();
    }


    /// <summary>移动资源</summary>
    public static Func<string, string, AssetMoveResult> OnWillMoveAssetCallback = null;
    private static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    {
        OnWillMoveAssetCallback?.Invoke(oldPath, newPath);

        AssetMoveResult result = AssetMoveResult.DidNotMove;

        if (IsLocked(oldPath))
        {
            Debug.LogError($"Could not move {oldPath} to {newPath} because {oldPath} is locked!");
            return AssetMoveResult.FailedMove;
        }
        else if (IsLocked(newPath))
        {
            Debug.LogError($"Could not move {oldPath} to {newPath} because {newPath} is locked!");
            return AssetMoveResult.FailedMove;
        }

        onMoveAsset = new object[] { result, oldPath, newPath };


        return result;
    }


    /// <summary>删除资源</summary>
    public static Func<string, RemoveAssetOptions, AssetDeleteResult> OnWillDeleteAssetCallback = null;
    private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    {
        OnWillDeleteAssetCallback?.Invoke(assetPath, option);
        AssetDeleteResult res = AssetDeleteResult.DidDelete;
        if (IsLocked(assetPath))
        {
            Debug.LogError($"Could not delete {assetPath} because it is locked!");
            res = AssetDeleteResult.FailedDelete;
        }

        onDeleteAsset = new object[] { res, assetPath, option };

        return AssetDeleteResult.DidNotDelete;
    }


    /// <summary>文件没有死锁</summary>
    static bool IsUnlocked(string path)
    {
        return !IsLocked(path);
    }


    /// <summary>文件死锁</summary>
    static bool IsLocked(string path)
    {
        if (!File.Exists(path)) return false;

        return new FileInfo(path).IsReadOnly;
    }
}
