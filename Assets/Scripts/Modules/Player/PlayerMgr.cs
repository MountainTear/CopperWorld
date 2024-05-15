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
    public PlayerMode mode;
    public int healthCurrent;
    public int healthMax = 20;
    public int oxygenCurrent;
    public int oxygenMax = 100;
    public int damage = 1;

    public Dictionary<int, int> mineralList;    //key是矿物id，value是数量
    public Dictionary<int, int> weaponList;    //key是武器id，value是数量
    public int money = 0;
    public int weapon = 0;  //正在使用的武器id


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
        mineralList = new Dictionary<int, int>();
        weaponList = new Dictionary<int, int>();
    }

    public void InitPlayer()
    {
        if (!isPlayerInit)
        {
            Player.Instance.Init();
            mode = PlayerMode.Attack;
            UIMgr.Instance.OpenView<MainUIView>();
            RecoverState();
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

    public void TryChangeMode()
    {
        if (Player.Instance.isInUseWeapon)
        {
            return;
        }
        PlayerMode modeTarget = PlayerMode.Attack;
        if (mode == PlayerMode.Attack)
        {
            modeTarget = PlayerMode.Mine;
        }else if (mode == PlayerMode.Mine)
        {
            modeTarget = PlayerMode.Attack;
        }
        ChangeMode(modeTarget);
    }

    public void ChangeMode(PlayerMode modeTarget)
    {
        if (mode != modeTarget)
        {
            mode = modeTarget;
            UIMgr.Instance.GetView<MainUIView>().UpdateMode();
            Player.Instance.UpdateStateShow();
        }
    }

    public void RecoverState()
    {
        healthCurrent = healthMax;
        oxygenCurrent = oxygenMax;
        UIMgr.Instance.GetView<MainUIView>().UpdateHealth();
        UIMgr.Instance.GetView<MainUIView>().UpdateOxygen();
    }
    #endregion

    #region 玩家信息获取
    public int GetLayer()
    {
        if (!isPlayerInit)
        {
            return 0;
        }
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

    public CharacterState ModeToCharacterState()
    {
        if (mode == PlayerMode.Attack)
        {
            return CharacterState.Attack;
        }else if (mode == PlayerMode.Mine)
        {
            return CharacterState.Mine;
        }
        return CharacterState.None;
    }

    public int GetDamage()
    {
        return damage;
    }
    #endregion

    #region 玩家交互
    public void TakeDamage(int damage)
    {
        healthCurrent -= damage;
        UIMgr.Instance.GetView<MainUIView>().UpdateHealth();
        if (healthCurrent <= 0)
        {
            UIMgr.Instance.OpenView<GameOverPopView>();
        }
    }

    public void GetMineral(int id)
    {
        int num = 0;
        if (mineralList.ContainsKey(id))
        {
            num = mineralList[id];
        }
        else
            mineralList.Add(id, 0);
        num++;
        mineralList[id] = num;
    }
    #endregion

}
