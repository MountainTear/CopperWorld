using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMgr : MonoBehaviour
{
    //单例
    public static MainMgr instance;
    
    private GameState gameState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //初始化变量
        gameState = GameState.GameBegin;
        //加载界面前实例化单例防止堵塞
        var uiMgrInstance = UIMgr.Instance;
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //加载标题界面
        UIMgr.Instance.OpenView<TitleView>();
        //加载背景音乐
        AudioMgr.Instance.BgGameStartAudio();
    }

    public void AfterStarGame()
    {
        gameState = GameState.GameIn;
        //加载背景音乐
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
