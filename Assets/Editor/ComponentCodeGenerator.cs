using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class ComponentCodeGenerator : EditorWindow
{
    [MenuItem("Assets/����Prefab���ɴ���/�����������", false, 10)]
    public static string GenerateComponentDefinitions()
    {
        string str = "";
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            StringBuilder sb = new StringBuilder();
            Transform[] allChildren = selectedGameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                Match match = Regex.Match(child.name, @"(\w+)_(\w+)cr");
                if (match.Success)
                {
                    string componentType = match.Groups[1].Value.ToLower();
                    string componentName = child.name;

                    string componentTypeName = GetComponentTypeName(componentType);
                    if (!string.IsNullOrEmpty(componentTypeName))
                    {
                        sb.AppendLine($"\tprivate {componentTypeName} {componentName};");
                    }
                }
            }

            EditorGUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log($"�����������Ѹ��Ƶ����а塣{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/����Prefab���ɴ���/���������ȡ", false, 11)]
    public static string GenerateComponentGetters()
    {
        string str = "";
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            StringBuilder sb = new StringBuilder();
            Transform[] allChildren = selectedGameObject.GetComponentsInChildren<Transform>(true);
            string rootPath = selectedGameObject.name;

            foreach (Transform child in allChildren)
            {
                Match match = Regex.Match(child.name, @"(\w+)_(\w+)cr");
                if (match.Success && child.gameObject != selectedGameObject)
                {
                    string componentType = match.Groups[1].Value.ToLower();
                    string componentName = child.name;
                    string path = GetGameObjectPath(child, rootPath);

                    string componentTypeName = GetComponentTypeName(componentType);
                    if (!string.IsNullOrEmpty(componentTypeName))
                    {
                        sb.AppendLine($"\t\t{componentName} = skinTrans.Find(\"{path}\").GetComponent<{componentTypeName}>();");
                    }
                }
            }

            EditorGUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log($"�����ȡ�����Ѹ��Ƶ����а塣{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/����Prefab���ɴ���/���ɰ�ť��Ӧע��", false, 12)]
    public static string GenerateButtonListeners()
    {
        string str = "";
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            StringBuilder sb = new StringBuilder();
            Button[] allButtons = selectedGameObject.GetComponentsInChildren<Button>(true);

            foreach (Button btn in allButtons)
            {
                string buttonName = btn.gameObject.name;
                Match match = Regex.Match(buttonName, @"btn_(\w+)cr");
                if (match.Success)
                {
                    string callbackName = match.Groups[1].Value;
                    sb.AppendLine($"\t\t{buttonName}.onClick.AddListener(On{UppercaseFirst(callbackName)}Click);");
                }
            }

            EditorGUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log($"��ť��Ӧע������Ѹ��Ƶ����а塣{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/����Prefab���ɴ���/���ɰ�ť��Ӧ����", false, 13)]
    public static string GenerateButtonResponseMethods()
    {
        string str = "";
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            StringBuilder sb = new StringBuilder();
            Button[] allButtons = selectedGameObject.GetComponentsInChildren<Button>(true);

            int i = 0;
            foreach (Button btn in allButtons)
            {
                string buttonName = btn.gameObject.name;
                Match match = Regex.Match(buttonName, @"btn_(\w+)cr");
                if (match.Success)
                {
                    i++;
                    string methodName = match.Groups[1].Value;
                    sb.AppendLine($"\tpublic void On{UppercaseFirst(methodName)}Click()");
                    sb.AppendLine("\t{");
                    sb.AppendLine("\t\t// TODO: Add your button click handling logic here");
                    sb.AppendLine("\t}");
                    if (i != allButtons.Length)
                    {
                        sb.AppendLine();
                    }
                }
            }

            EditorGUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log($"��ť��Ӧ���������Ѹ��Ƶ����а塣{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    // �������������ڻ�ȡGameObject��·��
    static string GetGameObjectPath(Transform transform, string rootPath)
    {
        string path = transform.name;
        while (transform.parent != null && transform.gameObject.name != rootPath)
        {
            transform = transform.parent;
            if (transform.gameObject.name != rootPath)
            {
                path = transform.name + "/" + path;
            }
        }
        return path;
    }

    // �������������ڸ���������ͻ�ȡ��Ӧ��C#��������
    static string GetComponentTypeName(string componentType)
    {
        switch (componentType)
        {
            case "btn": return "Button";
            case "text": return "TextMeshProUGUI";
            case "image": return "Image";
            // ������������Ӹ�����������ӳ��
            default: return null;
        }
    }

    // �������������ڽ��ַ����ĵ�һ����ĸת��Ϊ��д
    static string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}
