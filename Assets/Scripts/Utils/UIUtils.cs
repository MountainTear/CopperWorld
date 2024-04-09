using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : Singleton<UIUtils>
{
    //打开界面用的变量
    private object[] args;
    private GameObject loadingView;

    public UIUtils()
    {
        args = new object[100];
        loadingView = GameObject.Find("LoadingParent");
    }
   
    //加载界面
    public void SetLoading(bool isShow)
    { 
        loadingView.SetActive(isShow); 
    }

    //弹出提示弹窗
    public void ToolTip(TipType type)
    {
        args[0] = (int)type;
        UIMgr.Instance.OpenView<TipPopView>(args);
    }
}

public enum TipType
{
    Info = 1,   //游戏介绍
}