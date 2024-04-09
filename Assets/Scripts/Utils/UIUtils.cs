using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : Singleton<UIUtils>
{
    //�򿪽����õı���
    private object[] args;
    private GameObject loadingView;

    public UIUtils()
    {
        args = new object[100];
        loadingView = GameObject.Find("LoadingParent");
    }
   
    //���ؽ���
    public void SetLoading(bool isShow)
    { 
        loadingView.SetActive(isShow); 
    }

    //������ʾ����
    public void ToolTip(TipType type)
    {
        args[0] = (int)type;
        UIMgr.Instance.OpenView<TipPopView>(args);
    }
}

public enum TipType
{
    Info = 1,   //��Ϸ����
}