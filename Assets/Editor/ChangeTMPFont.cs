using UnityEditor;
using UnityEngine;
using TMPro;

public class ChangeTMPFont : EditorWindow
{
    private TMP_FontAsset selectedFont;

    [MenuItem("Tools/Change TMP Font")]
    static void Init()
    {
        ChangeTMPFont window = (ChangeTMPFont)EditorWindow.GetWindow(typeof(ChangeTMPFont));
        window.Show();
    }

    void OnGUI()
    {
        selectedFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Select Font:", selectedFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Change Font"))
        {
            ChangeFont();
        }
    }

    void ChangeFont()
    {
        if (selectedFont == null)
        {
            Debug.LogError("Please select a font.");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            TextMeshProUGUI[] tmpComponents = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmp in tmpComponents)
            {
                tmp.font = selectedFont;
            }
        }
    }
}
