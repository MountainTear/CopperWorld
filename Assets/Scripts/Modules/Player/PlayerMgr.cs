using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public void InitPlayer()
    {
        Player.Instance.Init();
    }

    public void Update()
    {
        Player.Instance.Update();
    }
}
