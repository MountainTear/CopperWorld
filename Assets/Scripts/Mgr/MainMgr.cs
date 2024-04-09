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
        //���ؽ���ǰʵ����������ֹ����
        var uiMgrInstance = UIMgr.Instance;
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //���ر������
        UIMgr.Instance.OpenView<TitleView>();
        //���ر�������
        AudioMgr.Instance.BgGameStartAudio();
    }

    public void AfterStarGame()
    {
        gameState = GameState.GameIn;
        //���ر�������
        AudioMgr.Instance.BgGameInAudio();
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
