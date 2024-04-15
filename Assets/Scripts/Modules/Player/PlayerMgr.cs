using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public Dictionary<SceneType, Vector3> ORIGIN_POS;  //在场景的初始位置
    public bool isPlayerInit = false;

    public PlayerMgr()
    {
        ORIGIN_POS = new Dictionary<SceneType, Vector3>
        {
            { SceneType.Home, new Vector3(0, -2.95f, 0)},
            { SceneType.Mine, new Vector3(0, -2.95f, 0)},
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
    public void ResetOriginPos()
    {
        SceneType sceneType = SceneMgr.Instance.sceneType;
        Player.Instance.SetPos(ORIGIN_POS[sceneType]);
        Player.Instance.ReseState();
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
