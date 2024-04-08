using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;

public class CreateUIViewCode : EditorWindow
{
    [MenuItem("Assets/����Prefab���ɴ���/�����������", false, 9)]
    static void CreateUIViewScript()
    {
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            string prefabName = selectedGameObject.name;
            string scriptName = $"{prefabName}";
            string scriptPath = AssetDatabase.GetAssetPath(selectedGameObject);
            string skinPath = scriptPath.Replace(".prefab", "");
            skinPath = skinPath.Replace("Assets/Resources/", "");
            string fullPath = Path.GetDirectoryName(scriptPath);    //���ԭ·���е�/�Զ�����\
            fullPath = fullPath.Replace(@"\","/");
            fullPath = fullPath.Replace("Resources/Prefabs/UI", "Scripts/Modules");
            fullPath = $"{fullPath}/{scriptName}.cs";
            if (File.Exists(fullPath))
            {
                Debug.LogError($"�ļ��Ѵ���: {fullPath}");
                continue;
            }

            // ���ɸ����ִ���
            string componentDefinitions = GenerateComponentDefinitions(selectedGameObject);
            string componentGetters = GenerateComponentGetters(selectedGameObject);
            string buttonListeners = GenerateButtonListeners(selectedGameObject);
            string buttonResponseMethods = GenerateButtonResponseMethods(selectedGameObject);

            string scriptContent = GetScriptContent(scriptName, skinPath, componentDefinitions, componentGetters, buttonListeners, buttonResponseMethods);
            File.WriteAllText(fullPath, scriptContent);

            // ˢ�±༭�����Ա����ļ�������Project��ͼ��
            AssetDatabase.Refresh();

            Debug.Log($"��������Ѵ����ڣ�{fullPath}");
        }
    }

    // �����������������ɵĴ��뵽һ��ģ����
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
    #region ��������
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

    // ����ķ�����Ҫ��֮ǰ�Ĺ�������ȡ����������Ӧ�Ĵ����
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
