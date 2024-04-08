using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    //皮肤路径
    public string skinPath;
    /// <summary>
    /// 皮肤，一般指的是页面的Prefab
    /// </summary>
    public GameObject skin;
    //层级
    public UILayer layer;
    //面板参数
    public object[] args;
    //时停功能开关
    public bool isTimePause;

    #region 生命周期
    //初始化
    public virtual void Init(params object[] args)
    {
        this.args = args;
    }
    //面板更新前
    public virtual void OnShowing()
    {
        //时停功能检测
        if (isTimePause)
        {
            Time.timeScale = 0;
        }
    }
    //面板更新后
    public virtual void OnShowed() { }
    //关闭前
    public virtual void OnClosing()
    {
        //时停功能检测
        if (isTimePause)
        {
            Time.timeScale = 1;
        }
    }
    //关闭后
    public virtual void OnClosed() { }
    #endregion

    #region 操作
    protected virtual void Close()
    {
        string name = this.GetType().ToString();
        UIMgr.Instance.CloseView(name);
    }
    #endregion
    
}
