using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMgr : Singleton<MapMgr>
{
    public Vector2 MAP_CIZE;    //单个地图的大小，实际大小
    public int ORIGIN_POS_Y;    //初始位置
    public int GRID_WIDTH;  //格子宽度，更改需同步修改Tilemap和Tile
    public int MAP_HEIGHT;   //单个地图的高度（格子数）
    public int MAP_WIDTH;   //单个地图的宽度（格子数）

    public GameObject parent;

    private Dictionary<MapIndex, Map> mapDic;
    private Dictionary<int, TileBase> tileBaseDic;
    private int playerLayerCache = -1;
    public Vector3 posCache;

    public MapMgr() 
    {
        MAP_CIZE = new Vector2(30, 10);
        ORIGIN_POS_Y = -4;
        GRID_WIDTH = 1;
        MAP_HEIGHT = (int)MAP_CIZE.y / GRID_WIDTH;
        MAP_WIDTH = (int)MAP_CIZE.x / GRID_WIDTH;
        parent = GameObject.Find("Map");
        posCache = Vector3.zero;
    }

    public void InitMap()
    {
        if (mapDic == null)
        {
            mapDic = new Dictionary<MapIndex, Map>()
            {
                { MapIndex.Up, new Map(MapIndex.Up, new MapLayerInfo{ begin = 1, end = MAP_HEIGHT})},
                { MapIndex.Middle, new Map(MapIndex.Middle, new MapLayerInfo{ begin = MAP_HEIGHT + 1, end = MAP_HEIGHT * 2})},
                { MapIndex.Down, new Map(MapIndex.Down, new MapLayerInfo{ begin = MAP_HEIGHT * 2 + 1, end = MAP_HEIGHT * 3})},
            };
            parent.transform.position = new Vector3(0, ORIGIN_POS_Y, 0);
            foreach (Map map in mapDic.Values)
            {
                map.UpdateMap();
            }
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
                posCache.y = mapDic[MapIndex.Middle].entity.transform.localPosition.y - MapMgr.Instance.MAP_CIZE.y * GRID_WIDTH;
                mapDic[MapIndex.Down].entity.transform.localPosition = posCache;
                mapDic[MapIndex.Down].layerInfo.begin = mapDic[MapIndex.Middle].layerInfo.end + 1;
                mapDic[MapIndex.Down].layerInfo.end = mapDic[MapIndex.Middle].layerInfo.end + MAP_HEIGHT;
                mapDic[MapIndex.Down].UpdateMap();
            }
            else if(middleLayerInfo.begin > MAP_HEIGHT + 1 && layer < middleLayerInfo.begin && playerLayerCache >= middleLayerInfo.begin)
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
                posCache.y = mapDic[MapIndex.Middle].entity.transform.localPosition.y + MapMgr.Instance.MAP_CIZE.y * GRID_WIDTH;
                mapDic[MapIndex.Up].entity.transform.localPosition = posCache;
                mapDic[MapIndex.Up].layerInfo.begin = mapDic[MapIndex.Middle].layerInfo.begin - MAP_HEIGHT;
                mapDic[MapIndex.Up].layerInfo.end = mapDic[MapIndex.Middle].layerInfo.begin - 1;
                mapDic[MapIndex.Up].UpdateMap();
            }
            UIMgr.Instance.GetView<MainUIView>().UpdateLayer();
        }
        playerLayerCache = layer;
    }

    public TileBase GetTileBaseById(int id)
    {
        if (tileBaseDic == null)
        {
            tileBaseDic = new Dictionary<int, TileBase> ();
            foreach (var config in ConfigMgr.Instance.configMineral.mineralList)
            {
                TileBase tileBase = Resources.Load<TileBase>($"Tiles/tile_{config.id}");
                tileBaseDic.Add(config.id, tileBase);
            }
        }
        return tileBaseDic[id];
    }
}

public class MapLayerInfo
{
    public int begin;
    public int end;
}

public class GridInfo
{
    public GridType type;
    public int id;
}

public class GenerateTarget
{
    public int id;
    public int num;
}