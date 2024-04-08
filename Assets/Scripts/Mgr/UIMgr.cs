using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    //画布，用于指向场景中的画布
    private GameObject canvas;
    //界面字典，用以存放已打开的界面
    public Dictionary<string, ViewBase> dict;
    //层级字典，用以存放各个层级所对应的父物体
    private Dictionary<UILayer, Transform> layerDict;

    public UIMgr()
    {
        InitLayer();
        dict = new Dictionary<string, ViewBase>();
    }
   
    //初始化层
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
            //场景中的Canvas下的层级命名方式也要是UILayer
            string name = p.ToString();
            Transform transform = canvas.transform.Find(name);
            layerDict.Add(p, transform);
        }
    }

    //打开界面
    public void OpenView<T>(string skinPath, params object[] args) where T : ViewBase
    {
        //已经打开
        string name = typeof(T).ToString();
        if (dict.ContainsKey(name))
        {
            return;
        }
        //界面脚本
        ViewBase View = canvas.AddComponent<T>();
        View.Init(args);
        dict.Add(name, View);
        //加载皮肤
        skinPath = skinPath != "" ? skinPath : View.skinPath;
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
        {
            Debug.LogError("ViewMgr.OpenView fail, skin is null, skinPath =" + skinPath);
        }
        View.skin = GameObject.Instantiate(skin);
        //坐标
        Transform skinTrans = View.skin.transform;
        UILayer layer = View.layer;
        Transform parent = layerDict[layer];
        skinTrans.SetParent(parent, false);
        //View的生命周期
        View.OnShowing();
        View.OnShowed();
    }
    
    //关闭界面
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
        //销毁皮肤和界面
        GameObject.Destroy(View.skin);
        GameObject.Destroy(View);
    }

    //获取界面
    public ViewBase GetView(string name)
    {
        ViewBase View = (ViewBase)dict[name];
        return View;
    }
}

/// <summary>
/// 分层类型
/// </summary>
public enum UILayer
{
    //界面
    View,
    //弹窗
    Tip
}