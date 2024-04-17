using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public Vector3 ORIGIN_POS;  //初始位置
    public Dictionary<SceneType, Vector3> SCENE_CHANGE_POS;  //切换场景的位置

    public GameObject cameraRange;

    public bool isPlayerInit = false;
    public bool isSetOriginPos = false;
    public Vector3 posCache;
    public PlayerState state;

    public PlayerMgr()
    {
        ORIGIN_POS = new Vector3(0, -4f, 0);
        SCENE_CHANGE_POS = new Dictionary<SceneType, Vector3>
        {
            { SceneType.Home, new Vector3(11.5f, -4f, 0)},
            { SceneType.Mine, new Vector3(-11.5f, -4f, 0)},
        };
        cameraRange = GameObject.Find("CameraRange");
        posCache = Vector3.zero;
    }

    public void InitPlayer()
    {
        if (!isPlayerInit)
        {
            Player.Instance.Init();
            state = PlayerState.Normal;
            isPlayerInit = true;
        }
    }

    public void FixedUpdate()
    {
        if (isPlayerInit)
        {
            Player.Instance.FixedUpdate();
        }
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

    public void UpdateCameraRange()
    {
        float posY = Player.Instance.entity.transform.position.y;
        float originY = ORIGIN_POS.y;
        posCache.y = posY < originY ? posY - originY : 0;
        cameraRange.transform.position = posCache;
    }

    public void ChangeState(PlayerState stateTarget)
    {
        if (state != stateTarget)
        {
            //Player.Instance.UpdateStateShow(stateTarget);
            state = stateTarget;
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
            layer = (int)math.ceil((originY - posY) / MapMgr.Instance.GRID_WIDTH);
        }
        return layer;
    }

    public void UpdatePlayerLayer()
    {
        MapMgr.Instance.OnPlayerLayerChange();
    }
    #endregion
}
