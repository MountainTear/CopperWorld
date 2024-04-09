using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����̹�����
/// </summary>
public class MainMgr : MonoBehaviour
{
    //����
    public static MainMgr instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //���ؽ���ǰʵ����������ֹ����
        var uiMgrInstance = UIMgr.Instance;
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //���ر������
        UIMgr.Instance.OpenView<TitleView>("");
    }
}
