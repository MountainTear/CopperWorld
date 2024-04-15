using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public Vector3 ORIGIN_POS;  //初始位置
    public Dictionary<SceneType, Vector3> SCENE_CHANGE_POS;  //切换场景的位置
    public bool isPlayerInit = false;
    public bool isSetOriginPos = false;

    public PlayerMgr()
    {
        ORIGIN_POS = new Vector3(0, -2.95f, 0);
        SCENE_CHANGE_POS = new Dictionary<SceneType, Vector3>
        {
            { SceneType.Home, new Vector3(11.5f, -2.95f, 0)},
            { SceneType.Mine, new Vector3(-11.5f, -2.95f, 0)},
        };
    }

    public void InitPlayer()
    {
        if (!isPlayerInit)
        {
            Player.Instance.Init();
            isPlayerInit = true;
        }
    }

    public void Update()
    {
        Player.Instance.Update();
    }

    #region 玩家设置
    public void ResetPos()
    {
        SceneType sceneType = SceneMgr.Instance.sceneType;
        if (!isSetOriginPos)
        {
            Player.Instance.SetPos(ORIGIN_POS);
            isSetOriginPos = true;
        }
        else
        {
            Player.Instance.SetPos(SCENE_CHANGE_POS[sceneType]);
        }
        Player.Instance.ReseState();
    }

    public void SetCameraEnable(bool enable)
    {
        if (!isPlayerInit)
        {
            return;
        }
        if (Player.Instance.camera.enabled != enable)
        {
            Player.Instance.camera.enabled = enable;
        }
    }
    #endregion

    #region 玩家信息获取
    public int GetLayer()
    {
        int layer = 0;
        float posY = Player.Instance.entity.transform.position.y;
        int originY = MapMgr.Instance.ORIGIN_POS_Y;
        if (isPlayerInit && posY <= originY)
        {
            layer = (int)(originY - posY) / MapMgr.Instance.GRID_WIDTH;
        }
        return layer;
    }

    public void UpdatePlayerLayer()
    {
        MapMgr.Instance.OnPlayerLayerChange();
    }
    #endregion
}
