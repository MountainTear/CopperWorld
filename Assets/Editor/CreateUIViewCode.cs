using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;

public class CreateUIViewCode : EditorWindow
{
    [MenuItem("Assets/根据Prefab生成代码/创建界面代码", false, 9)]
    static void CreateUIViewScript()
    {
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            string prefabName = selectedGameObject.name;
            string scriptName = $"{prefabName}";
            string scriptPath = AssetDatabase.GetAssetPath(selectedGameObject);
            string skinPath = scriptPath.Replace(".prefab", "");
            skinPath = skinPath.Replace("Assets/Resources/", "");
            string fullPath = Path.GetDirectoryName(scriptPath);    //会把原路径中的/自动换成\
            fullPath = fullPath.Replace(@"\","/");
            fullPath = fullPath.Replace("Resources/Prefabs/UI", "Scripts/Modules");
            fullPath = $"{fullPath}/{scriptName}.cs";
            if (File.Exists(fullPath))
            {
                Debug.LogError($"文件已存在: {fullPath}");
                continue;
            }

            // 生成各部分代码
            string componentDefinitions = GenerateComponentDefinitions(selectedGameObject);
            string componentGetters = GenerateComponentGetters(selectedGameObject);
            string buttonListeners = GenerateButtonListeners(selectedGameObject);
            string buttonResponseMethods = GenerateButtonResponseMethods(selectedGameObject);

            string scriptContent = GetScriptContent(scriptName, skinPath, componentDefinitions, componentGetters, buttonListeners, buttonResponseMethods);
            File.WriteAllText(fullPath, scriptContent);

            // 刷新编辑器，以便新文件出现在Project视图中
            AssetDatabase.Refresh();

            Debug.Log($"界面代码已创建于：{fullPath}");
        }
    }

    // 这里是整合所有生成的代码到一个模板中
    static string GetScriptContent(string className, string skinPath, string componentDefinitions, string componentGetters, string buttonListeners, string buttonResponseMethods)
    {
        return
$@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class {className} : ViewBase
{{
{componentDefinitions}
    #region 生命周期
    public override void Init(params object[] args)
    {{
        base.Init(args);
        skinPath = ""{skinPath}"";
        layer = UILayer.View;
    }}

    public override void OnShowing()
    {{
        base.OnShowing();
        Transform skinTrans = skin.transform;
{componentGetters}
{buttonListeners}   }}
    #endregion

{buttonResponseMethods}}}";
    }

    // 下面的方法需要从之前的功能中提取，以生成相应的代码段
    static string GenerateComponentDefinitions(GameObject selectedGameObject)
    {
        return ComponentCodeGenerator.GenerateComponentDefinitions();
    }

    static string GenerateComponentGetters(GameObject selectedGameObject)
    {
        return ComponentCodeGenerator.GenerateComponentGetters();
    }

    static string GenerateButtonListeners(GameObject selectedGameObject)
    {
        return ComponentCodeGenerator.GenerateButtonListeners();
    }

    static string GenerateButtonResponseMethods(GameObject selectedGameObject)
    {
        return ComponentCodeGenerator.GenerateButtonResponseMethods();
    }
}
