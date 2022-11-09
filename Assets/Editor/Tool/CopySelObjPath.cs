using UnityEditor;
using UnityEngine;

/// <summary>
///拷贝当前选择的GameObject的全路径
/// </summary>
public class CopySelObjPath
{
    [MenuItem("GameObject/Copy FllPath",false,0)]
    public static void CopyGoFllPath()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = $"transform.Find(\"{GetPath(Selection.activeTransform)}\").GetComponent<>();";
        textEditor.SelectAll();
        textEditor.Copy();
    }

  

    public static string GetPath(Transform select)
    {
        if (select == null) return string.Empty;

        if (select.parent == null) return select.name;

        return $"{GetPath(select.parent)}/{select.name}";
    }


}
