using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : Singleton<MapMgr>
{
    public Vector2 MAP_CIZE;    //单个地图的大小
    public int ORIGIN_POS_Y;    //初始位置
    public int GRID_WIDTH;  //格子宽度
    public int LAYER_PER_MAP;   //单个地图的宽度层数
    public GameObject parent;
    private Dictionary<MapIndex, Map> mapDic;
    private int playerLayerCache = -1;
    public Vector3 posCache;

    public MapMgr() 
    {
        MAP_CIZE = new Vector2(30, 10);
        ORIGIN_POS_Y = -4;
        GRID_WIDTH = 1;
        LAYER_PER_MAP = (int)MAP_CIZE.y / GRID_WIDTH;
        parent = GameObject.Find("Map");
        posCache = Vector3.zero;
    }

    public void InitMap()
    {
        if (mapDic == null)
        {
            mapDic = new Dictionary<MapIndex, Map>()
            {
                { MapIndex.Up, new Map(MapIndex.Up, new MapLayerInfo{ begin = 0, end = LAYER_PER_MAP})},
                { MapIndex.Middle, new Map(MapIndex.Middle, new MapLayerInfo{ begin = LAYER_PER_MAP + 1, end = LAYER_PER_MAP * 2})},
                { MapIndex.Down, new Map(MapIndex.Down, new MapLayerInfo{ begin = LAYER_PER_MAP * 2 + 1, end = LAYER_PER_MAP * 3})},
            };
            parent.transform.position = new Vector3(0, ORIGIN_POS_Y, 0);
        }
    }

    public void UpdateMapShow()
    {
        bool isShowMap = SceneMgr.Instance.sceneType == SceneType.Mine;
        if (isShowMap != parent.gameObject.activeSelf)
        {
            parent.gameObject.SetActive(isShowMap);
        }
    }

    public void OnPlayerLayerChange()
    {
        int layer = PlayerMgr.Instance.GetLayer();
        if (layer != playerLayerCache)
        {
            MapLayerInfo middleLayerInfo = mapDic[MapIndex.Middle].layerInfo;
            if (layer > middleLayerInfo.end && playerLayerCache <= middleLayerInfo.end)
            {
                //更换索引
                var cache = mapDic[MapIndex.Up];
                mapDic[MapIndex.Up] = mapDic[MapIndex.Middle];
                mapDic[MapIndex.Up].mapIndex = MapIndex.Up;
                mapDic[MapIndex.Middle] = mapDic[MapIndex.Down];
                mapDic[MapIndex.Middle].mapIndex = MapIndex.Middle;
                mapDic[MapIndex.Down] = cache;
                mapDic[MapIndex.Down].mapIndex = MapIndex.Down;
                //更新位置及数据
                posCache.y = mapDic[MapIndex.Middle].entity.transform.localPosition.y - MapMgr.Instance.MAP_CIZE.y;
                mapDic[MapIndex.Down].entity.transform.localPosition = posCache;
                mapDic[MapIndex.Down].layerInfo.begin = mapDic[MapIndex.Middle].layerInfo.end + 1;
                mapDic[MapIndex.Down].layerInfo.end = mapDic[MapIndex.Middle].layerInfo.end + LAYER_PER_MAP;
            }
            else if(middleLayerInfo.begin > LAYER_PER_MAP + 1 && layer < middleLayerInfo.begin && playerLayerCache >= middleLayerInfo.begin)
            {
                //更换索引
                var cache = mapDic[MapIndex.Down];
                mapDic[MapIndex.Down] = mapDic[MapIndex.Middle];
                mapDic[MapIndex.Down].mapIndex = MapIndex.Down;
                mapDic[MapIndex.Middle] = mapDic[MapIndex.Up];
                mapDic[MapIndex.Middle].mapIndex = MapIndex.Middle;
                mapDic[MapIndex.Up] = cache;
                mapDic[MapIndex.Up].mapIndex = MapIndex.Up;
                //更新位置及数据
                posCache.y = mapDic[MapIndex.Middle].entity.transform.localPosition.y + MapMgr.Instance.MAP_CIZE.y;
                mapDic[MapIndex.Up].entity.transform.localPosition = posCache;
                mapDic[MapIndex.Up].layerInfo.begin = mapDic[MapIndex.Middle].layerInfo.begin - LAYER_PER_MAP;
                mapDic[MapIndex.Up].layerInfo.end = mapDic[MapIndex.Middle].layerInfo.begin - 1;
            }
        }
        playerLayerCache = layer;
    }
}

public struct MapLayerInfo
{
    public int begin;
    public int end;
}
