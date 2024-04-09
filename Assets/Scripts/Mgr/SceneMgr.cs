using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    private MapType mapType;
    public SceneMgr() { }

    public void EnterHomeMap()
    {
        MainMgr.instance.StartCoroutine(LoadScene(MapType.Home));
    }

    private void AfterEnterHomeMap()
    {
        //�������
        PlayerMgr.Instance.InitPlayer();
        mapType = MapType.Home;
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
        }
    }

    private string MapTypeToSceneName(MapType mapType) 
    { 
        return mapType.ToString(); 
    }

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
