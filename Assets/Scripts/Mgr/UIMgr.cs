using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    //����������ָ�򳡾��еĻ���
    private GameObject canvas;
    //�����ֵ䣬���Դ���Ѵ򿪵Ľ���
    public Dictionary<string, ViewBase> dict;
    //�㼶�ֵ䣬���Դ�Ÿ����㼶����Ӧ�ĸ�����
    private Dictionary<UILayer, Transform> layerDict;

    public UIMgr()
    {
        InitLayer();
        dict = new Dictionary<string, ViewBase>();
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
        if (dict.ContainsKey(name))
        {
            return;
        }
        //����ű�
        ViewBase View = canvas.AddComponent<T>();
        View.Init(args);
        dict.Add(name, View);
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
        ViewBase View = (ViewBase)dict[name];
        if(View == null)
        {
            return;
        }
        View.OnClosing();
        dict.Remove(name);
        View.OnClosed();
        //����Ƥ���ͽ���
        GameObject.Destroy(View.skin);
        GameObject.Destroy(View);
    }

    //��ȡ����
    public ViewBase GetView(string name)
    {
        ViewBase View = (ViewBase)dict[name];
        return View;
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