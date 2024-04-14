using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    private MapType mapType;
    private Vector3 posCache;
    public SceneMgr() 
    {
        posCache = Vector3.zero;
    }

    #region 地图处理
    public void EnterHomeMap()
    {
        PreEnterMap();
        MainMgr.instance.StartCoroutine(LoadScene(MapType.Home));
    }

    private void AfterEnterHomeMap()
    {
        //加载玩家
        if (!PlayerMgr.Instance.isPlayerInit)
        {
            PlayerMgr.Instance.InitPlayer();
        }
        mapType = MapType.Home;
        ResetPlayerPos();
    }

    public void EnterMineMap()
    {
        PreEnterMap();
        MainMgr.instance.StartCoroutine(LoadScene(MapType.Mine));
    }

    private void AfterEnterMineMap()
    {
        mapType = MapType.Mine;
        ResetPlayerPos();
    }
    #endregion

    #region 场景处理
    private void PreEnterMap()
    {
        string sceneName = MapTypeToSceneName(mapType);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if ( scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    IEnumerator LoadScene(MapType mapType)
    {
        string sceneName = MapTypeToSceneName(mapType);
        UIUtils.Instance.SetLoading(true);
        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // 执行加载完成后的操作
        UIUtils.Instance.SetLoading(false);
        LoadSceneCallback(mapType);
    }

    private void LoadSceneCallback(MapType mapType)
    {
        if (mapType == MapType.Home)
        {
            AfterEnterHomeMap();
        }else if (mapType == MapType.Mine)
        {
            AfterEnterMineMap();
        }
    }

    private string MapTypeToSceneName(MapType mapType) 
    { 
        return mapType.ToString(); 
    }
    #endregion

    #region 玩家处理
    private void ResetPlayerPos()
    {
        if (mapType == MapType.Home)
        {
            posCache.x = 0;
            posCache.y = -2.45f;
        }
        else if (mapType == MapType.Mine)
        {
            posCache.x = 0;
            posCache.y = -3.45f;
        }
        PlayerMgr.Instance.SetPos(posCache);
    }
    #endregion

    public void Update()
    {
        PlayerMgr.Instance.Update();
        if (mapType == MapType.Home)
        {

        }else if (mapType == MapType.Mine)
        {

        }
    }
}

public enum MapType
{
    Home = 1,
    Mine = 2,
}

public enum DoorType
{
    Home = 1,
    Mine = 2,
}
