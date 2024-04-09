using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主流程管理器
/// </summary>
public class MainMgr : MonoBehaviour
{
    //单例
    public static MainMgr instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //加载界面前实例化单例防止堵塞
        var uiMgrInstance = UIMgr.Instance;
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //加载标题界面
        UIMgr.Instance.OpenView<TitleView>("");
    }
}
