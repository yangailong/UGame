﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UGame_Local_Editor
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ILRuntimeMenu
    {

        [MenuItem("Tools/ILRuntime/打开ILRuntime中文文档")]
        static void OpenDocumentation()
        {
            Application.OpenURL("https://ourpalm.github.io/ILRuntime/");
        }

        [MenuItem("Tools/ILRuntime/打开ILRuntime Github项目")]
        static void OpenGithub()
        {
            Application.OpenURL("https://github.com/Ourpalm/ILRuntime");
        }

        [MenuItem("Tools/ILRuntime/打开ILRuntime视频教程")]
        static void OpenTutorial()
        {
            Application.OpenURL("https://learn.u3d.cn/tutorial/ilruntime");
        }
    }
}
#endif
