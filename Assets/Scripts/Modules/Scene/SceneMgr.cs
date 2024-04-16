using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneMgr : Singleton<SceneMgr>
{
    public SceneType sceneType;

    public SceneMgr() {}

    #region 场景进入
    public void EnterHomeScene()
    {
        PreEnterScene();
        MainMgr.instance.StartCoroutine(LoadScene(SceneType.Home));
    }

    private void AfterEnterHomeScene()
    {
        PlayerMgr.Instance.InitPlayer();
        sceneType = SceneType.Home;
        MapMgr.Instance.InitMap();
        MapMgr.Instance.UpdateMapShow();
        PlayerMgr.Instance.ResetPos();
    }

    public void EnterMineScene()
    {
        PreEnterScene();
        MainMgr.instance.StartCoroutine(LoadScene(SceneType.Mine));
    }

    private void AfterEnterMineScene()
    {
        sceneType = SceneType.Mine;
        MapMgr.Instance.UpdateMapShow();
        PlayerMgr.Instance.ResetPos();
    }
    #endregion

    #region 场景处理
    private void PreEnterScene()
    {
        string sceneName = MapTypeToSceneName(sceneType);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    IEnumerator LoadScene(SceneType sceneType)
    {
        PlayerMgr.Instance.SetCameraEnable(false); //防止切换场景的摄像机突变
        string sceneName = MapTypeToSceneName(sceneType);
        UIUtils.Instance.SetLoading(true);
        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // 执行加载完成后的操作
        PlayerMgr.Instance.SetCameraEnable(true);
        UIUtils.Instance.SetLoading(false);
        LoadSceneCallback(sceneType);
    }

    private void LoadSceneCallback(SceneType sceneType)
    {
        if (sceneType == SceneType.Home)
        {
            AfterEnterHomeScene();
        }else if (sceneType == SceneType.Mine)
        {
            AfterEnterMineScene();
        }
    }

    private string MapTypeToSceneName(SceneType sceneType) 
    { 
        return sceneType.ToString(); 
    }
    #endregion

    public void Update()
    {
        PlayerMgr.Instance.Update();
        if (sceneType == SceneType.Home)
        {
            
        }else if (sceneType == SceneType.Mine)
        {
            PlayerMgr.Instance.UpdatePlayerLayer();
            PlayerMgr.Instance.UpdateCameraRange();
        }
    }
}