using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class AssetsMappingImpl
{
    public static bool IsExistAssetsRootPath()
    {
        return Directory.Exists($"{Application.dataPath}/{AssetsMappingConst.needListenerAssetsRootPath}");
    }

    public static bool InListenerAssetsRootPath(string path)
    {
        return path.Contains(AssetsMappingConst.needListenerAssetsRootPath);
    }

    
    public static void Creat()
    {
        Dictionary<string, string> mapping = SearchAssets();

        string content = ParseMappingData(mapping);

        string path = $"{Application.dataPath}/{AssetsMappingConst.needListenerAssetsRootPath}/{AssetsMappingConst.creatPath}";

        WriteStringByFile(path, content);

        AssetDatabase.Refresh();
       
    }

    private static string ParseMappingData(Dictionary<string, string> mapping)
    {
        string content = string.Empty;
        foreach (var item in mapping)
        {
            content += $"{item.Key}{AssetsMappingConst.namePathSplit}{item.Value}\n";
        }

        content.TrimEnd('\n');

        return content;
    }

    private static void WriteStringByFile(string path, string content)
    {
        byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(content);

        try
        {
            string pathDir = Path.GetDirectoryName(path);
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            File.WriteAllBytes(path, bytes);
        }
        catch (Exception e)
        {
            Debug.LogError($"File Create Fail! \n{e.Message}");
        }
    }


    private static int direIndex = 0;

    private static Dictionary<string, string> SearchAssets()
    {
        string searchPath = $"{Application.dataPath}/{AssetsMappingConst.needListenerAssetsRootPath}";

        direIndex = searchPath.LastIndexOf(AssetsMappingConst.needListenerAssetsRootPath);

        direIndex += AssetsMappingConst.needListenerAssetsRootPath.Length + 1;

        Dictionary<string, string> mapping = new Dictionary<string, string>();

        RecursionSearchAssets(searchPath, mapping);

        return mapping;
    }

    private static void RecursionSearchAssets(string path, Dictionary<string, string> mapping)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string[] dires = Directory.GetDirectories(path);

        for (int i = 0; i < dires.Length; i++)
        {
            RecursionSearchAssets(dires[i], mapping);
        }

        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            string fileName = RemoveExpandName(new FileInfo(files[i]).Name);
            string relativePath = files[i].Substring(direIndex);


            if (relativePath.EndsWith(".meta") || relativePath.EndsWith(".DS_Store")) continue;

            relativePath = relativePath.Replace("\\", "/");

            if (fileName.EndsWith(" "))
            {
                Debug.LogError($"文件名尾部中有空格！ ->{fileName}<-");
                continue;
            }

            if (!mapping.ContainsKey(fileName))
            {
                mapping.Add(fileName, relativePath);
            }
            else
            {
                Debug.LogError($"存在重名文件！文件名:   {fileName}             path:   {relativePath}");
            }

        }
    }

    private static string RemoveExpandName(string name)
    {
        if (Path.HasExtension(name))
        {
            name = Path.ChangeExtension(name, null);
        }
        return name;
    }


}


