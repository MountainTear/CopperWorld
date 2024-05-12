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
        Application.targetFrameRate = 60;
        //加载界面前实例化特定单例防止堵塞
        var configMgrInstance = ConfigMgr.Instance;
        var audioMgrInstance = AudioMgr.Instance;
        //加载标题界面
        UIMgr.Instance.OpenView<TitleView>();
        //加载背景音乐
        AudioMgr.Instance.BgGameStartAudio();
    }

    public void AfterStartGame()
    {
        gameState = GameState.GameIn;
        //加载背景音乐
        AudioMgr.Instance.BgGameInAudio();
        //进入家场景
        SceneMgr.Instance.EnterHomeScene();
    }

    private void Update()
    {
        if (gameState == GameState.GameIn)
        {
            SceneMgr.Instance.Update();
            CheckInput();
        }
    }

    private void FixedUpdate()
    {
        if (gameState == GameState.GameIn)
        {
            SceneMgr.Instance.FixedUpdate();
        }
    }

    public void CheckInput()
    {
        if (Input.GetButtonUp("ChangeMode"))
        {
            PlayerMgr.Instance.TryChangeMode();
        }
        if (Input.GetButtonUp("GoHome"))
        {
            SceneMgr.Instance.EnterHomeScene();
        }
    }
}
