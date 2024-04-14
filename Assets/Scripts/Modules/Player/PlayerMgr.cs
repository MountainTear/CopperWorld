using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public bool isPlayerInit = false;

    public void InitPlayer()
    {
        Player.Instance.Init();
        isPlayerInit = true;
    }

    public void Update()
    {
        Player.Instance.Update();
    }

    public void SetPos(Vector3 pos)
    {
        Player.Instance.entity.transform.position = pos;
    }
}
