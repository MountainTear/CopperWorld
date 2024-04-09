using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : Singleton<UIUtils>
{
    //�򿪽����õı���
    private object[] args;

    public UIUtils()
    {
        args = new object[100];
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