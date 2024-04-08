using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    //Ƥ��·��
    public string skinPath;
    /// <summary>
    /// Ƥ����һ��ָ����ҳ���Prefab
    /// </summary>
    public GameObject skin;
    //�㼶
    public UILayer layer;
    //������
    public object[] args;
    //ʱͣ���ܿ���
    public bool isTimePause;

    #region ��������
    //��ʼ��
    public virtual void Init(params object[] args)
    {
        this.args = args;
    }
    //������ǰ
    public virtual void OnShowing()
    {
        //ʱͣ���ܼ��
        if (isTimePause)
        {
            Time.timeScale = 0;
        }
    }
    //�����º�
    public virtual void OnShowed() { }
    //�ر�ǰ
    public virtual void OnClosing()
    {
        //ʱͣ���ܼ��
        if (isTimePause)
        {
            Time.timeScale = 1;
        }
    }
    //�رպ�
    public virtual void OnClosed() { }
    #endregion

    #region ����
    protected virtual void Close()
    {
        string name = this.GetType().ToString();
        UIMgr.Instance.CloseView(name);
    }
    #endregion
    
}
