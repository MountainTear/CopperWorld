using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMgr : MonoBehaviour
{
    //����
    public static MainMgr instance;
    
    private GameState gameState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //��ʼ������
        gameState = GameState.GameBegin;
        Application.targetFrameRate = 60;
        //���ؽ���ǰʵ�����ض�������ֹ����
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //���ر������
        UIMgr.Instance.OpenView<TitleView>();
        //���ر�������
        AudioMgr.Instance.BgGameStartAudio();
    }

    public void AfterStartGame()
    {
        gameState = GameState.GameIn;
        //���ر�������
        AudioMgr.Instance.BgGameInAudio();
        //����ҵ�ͼ
        SceneMgr.Instance.EnterHomeMap();
    }

    private void Update()
    {
        if (gameState == GameState.GameIn)
        {
            SceneMgr.Instance.Update();
        }
    }
}

public enum GameState
{
    GameBegin = 1,
    GameIn = 2, 
    GameOver = 3,
}
