using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public GameObject entity;
    private SpriteRenderer image_bg;
    public MapIndex mapIndex;
    public MapLayerInfo layerInfo;
    public Vector3 posCache;

    public Map(MapIndex mapIndex, MapLayerInfo layerInfo)
    {
        this.mapIndex = mapIndex;
        this.layerInfo = layerInfo;
        posCache = Vector3.zero;
        Init();
    }

    public void Init()
    {
        //实例化实体
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Map/Map"), MapMgr.Instance.parent.transform);
        //初始化组件
        image_bg = entity.transform.Find("image_bg").GetComponent<SpriteRenderer>();
        //变更层级
        image_bg.sortingOrder = (int)OrderInLayer.Map;
        //初始化位置
        InitPos();
        image_bg.transform.localPosition = new Vector3(0, - MapMgr.Instance.MAP_CIZE.y / 2, 0);
    }

    public void InitPos()
    {
        if (mapIndex == MapIndex.Up)
        {
            posCache.y = 0;
        }else if (mapIndex == MapIndex.Middle)
        {
            posCache.y = -MapMgr.Instance.MAP_CIZE.y;
        }else if (mapIndex == MapIndex.Down)
        {
            posCache.y = -MapMgr.Instance.MAP_CIZE.y * 2;
        }
        entity.transform.localPosition = posCache;
    }
}
