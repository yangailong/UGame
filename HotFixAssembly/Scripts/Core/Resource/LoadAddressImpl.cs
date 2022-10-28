using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Collections.Generic;

/// <summary>通过Addressables加载资源</summary>
namespace UGame_Remove
{
    public class LoadAddressImpl
    {
        public IEnumerator LoadAssetAsync<TObject>(string path, Action<TObject> callback)
        {
            AsyncOperationHandle<TObject> handle = Addressables.LoadAssetAsync<TObject>(path);

            if (!handle.IsDone)
            {
                yield return handle;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Addrtessable 加载资源失败：详情->{path}...{handle.OperationException}<-");
            }

            callback?.Invoke(handle.Result);
        }


        public IEnumerator LoadAssetsAsync<TObject>(IEnumerable paths, Action<TObject> callback)
        {
            AsyncOperationHandle<IList<TObject>> handle = Addressables.LoadAssetsAsync<TObject>(paths, callback, Addressables.MergeMode.Union, false);

            yield return handle;
        }


        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(string name, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            return Addressables.LoadSceneAsync(name, loadMode, activateOnLoad, priority);
        }


        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene, UnloadSceneOptions unloadOptions, bool autoReleaseHandle = true)
        {
            return Addressables.UnloadSceneAsync(scene, unloadOptions, autoReleaseHandle);
        }


        public void Release<TObject>(TObject obj)
        {
            Addressables.Release(obj);
        }


    }
}
