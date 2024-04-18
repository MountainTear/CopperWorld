using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class ComponentCodeGenerator : EditorWindow
{
    [MenuItem("Assets/根据Prefab生成代码/生成组件定义", false, 10)]
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
            Debug.Log($"组件定义代码已复制到剪切板。{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/根据Prefab生成代码/生成组件获取", false, 11)]
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
            Debug.Log($"组件获取代码已复制到剪切板。{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/根据Prefab生成代码/生成按钮响应注册", false, 12)]
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
            Debug.Log($"按钮响应注册代码已复制到剪切板。{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    [MenuItem("Assets/根据Prefab生成代码/生成按钮响应函数", false, 13)]
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
            Debug.Log($"按钮响应函数代码已复制到剪切板。{selectedGameObject.name}");
            str += sb;
        }
        return str;
    }

    // 辅助方法，用于获取GameObject的路径
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

    // 辅助方法，用于根据组件类型获取对应的C#类型名称
    static string GetComponentTypeName(string componentType)
    {
        switch (componentType)
        {
            case "btn": return "Button";
            case "text": return "TextMeshProUGUI";
            case "image": return "Image";
            case "slider": return "Slider";
            // 可以在这里添加更多的组件类型映射
            default: return null;
        }
    }

    // 辅助方法，用于将字符串的第一个字母转换为大写
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
