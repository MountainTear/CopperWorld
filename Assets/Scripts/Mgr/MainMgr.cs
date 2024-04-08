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
        //Ԥ��������
        ConfigMgr.Instance.InitConfig();
        //���ر������
        UIMgr.Instance.OpenView<TitleView>("");
    }
}
