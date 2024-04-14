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

    #region ��ͼ����
    public void EnterHomeMap()
    {
        PreEnterMap();
        MainMgr.instance.StartCoroutine(LoadScene(MapType.Home));
    }

    private void AfterEnterHomeMap()
    {
        //�������
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

    #region ��������
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
        // �첽���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // �ȴ������������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // ִ�м�����ɺ�Ĳ���
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

    #region ��Ҵ���
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
