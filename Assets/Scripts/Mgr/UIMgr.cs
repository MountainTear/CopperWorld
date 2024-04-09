using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    //����������ָ�򳡾��еĻ���
    private GameObject canvas;
    //�����ֵ䣬���Դ���Ѵ򿪵Ľ���
    public Dictionary<string, ViewBase> viewDict;
    //�㼶�ֵ䣬���Դ�Ÿ����㼶����Ӧ�ĸ�����
    private Dictionary<UILayer, Transform> layerDict;

    public UIMgr()
    {
        InitLayer();
        viewDict = new Dictionary<string, ViewBase>();
    }
   
    //��ʼ����
    private void InitLayer()
    {
        canvas = GameObject.Find("UIParent");
        if(canvas == null)
        {
            Debug.LogError("ViewMgr.InitLayer fali, canvas is null");
        }
        layerDict = new Dictionary<UILayer, Transform>();
        foreach(UILayer p in Enum.GetValues(typeof(UILayer)))
        {
            //�����е�Canvas�µĲ㼶������ʽҲҪ��UILayer
            string name = p.ToString();
            Transform transform = canvas.transform.Find(name);
            layerDict.Add(p, transform);
        }
    }

    //�򿪽���
    public void OpenView<T>(string skinPath, params object[] args) where T : ViewBase
    {
        //�Ѿ���
        string name = typeof(T).ToString();
        if (viewDict.ContainsKey(name))
        {
            return;
        }
        //����ű�
        ViewBase View = canvas.AddComponent<T>();
        View.Init(args);
        viewDict.Add(name, View);
        //����Ƥ��
        skinPath = skinPath != "" ? skinPath : View.skinPath;
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
        {
            Debug.LogError("ViewMgr.OpenView fail, skin is null, skinPath =" + skinPath);
        }
        View.skin = GameObject.Instantiate(skin);
        //����
        Transform skinTrans = View.skin.transform;
        UILayer layer = View.layer;
        Transform parent = layerDict[layer];
        skinTrans.SetParent(parent, false);
        //View����������
        View.OnShowing();
        View.OnShowed();
    }
    
    //�رս���
    public void CloseView(string name)
    {
        ViewBase View = (ViewBase)viewDict[name];
        if(View == null)
        {
            return;
        }
        View.OnClosing();
        viewDict.Remove(name);
        View.OnClosed();
        //����Ƥ���ͽ���
        GameObject.Destroy(View.skin);
        GameObject.Destroy(View);
    }

    //��ȡ����
    public T GetView<T>() where T : ViewBase
    {
        // ʹ�����͵�ȫ����Ϊ�ֵ�ļ�
        string key = typeof(T).FullName;

        if (key != null && viewDict.TryGetValue(key, out ViewBase view))
        {
            return view as T;
        }

        // ���û���ҵ�����ת��ʧ�ܣ�����Ĭ��ֵ��null��
        return default(T);
    }
}

/// <summary>
/// �ֲ�����
/// </summary>
public enum UILayer
{
    //����
    View,
    //����
    Tip
}