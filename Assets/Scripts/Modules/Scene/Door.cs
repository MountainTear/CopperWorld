using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            if (type == DoorType.Home)
            {
                SceneMgr.Instance.EnterHomeScene();
            }
            else if (type == DoorType.Mine)
            {
                SceneMgr.Instance.EnterMineScene();
            }
        }
    }
}
