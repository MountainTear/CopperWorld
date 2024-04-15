using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map
{
    public GameObject entity;
    private GameObject go_offsetcr;
    private SpriteRenderer image_bgcr;
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;

    public MapIndex mapIndex;
    public MapLayerInfo layerInfo;
    public Vector3 posCache;
    public Vector3Int tilePosCache;
    public TileBase stone;
    public TileBase soil;

    public Map(MapIndex mapIndex, MapLayerInfo layerInfo)
    {
        this.mapIndex = mapIndex;
        this.layerInfo = layerInfo;
        posCache = Vector3.zero;
        tilePosCache = Vector3Int.zero;
        Init();
    }

    public void Init()
    {
        //实例化实体
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Map/Map"), MapMgr.Instance.parent.transform);
        //初始化组件
        go_offsetcr = entity.transform.Find("go_offsetcr").gameObject;
        image_bgcr = entity.transform.Find("go_offsetcr/image_bgcr").GetComponent<SpriteRenderer>();
        tilemap = entity.transform.Find("go_offsetcr/Grid/Tilemap").GetComponent<Tilemap>();
        tilemapRenderer = entity.transform.Find("go_offsetcr/Grid/Tilemap").GetComponent<TilemapRenderer>();
        stone = MapMgr.Instance.GetTileBaseById((int)SpecialMineralId.Stone);
        soil = MapMgr.Instance.GetTileBaseById((int)SpecialMineralId.Soil);
        //变更层级
        image_bgcr.sortingOrder = (int)OrderInLayer.Map;
        tilemapRenderer.sortingOrder = (int)OrderInLayer.Tile;
        //初始化位置
        InitPos();
        go_offsetcr.transform.localPosition = new Vector3(0, - MapMgr.Instance.MAP_CIZE.y / 2, 0);
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

    #region 地图生成
    public void UpdateMap()
    {
        GenerarateWall();
    }

    private void GenerarateWall()
    {
        GenerarateWall(-MapMgr.Instance.MAP_LENGTH/2);
        GenerarateWall(MapMgr.Instance.MAP_LENGTH/2 - 1);
    }

    private void GenerarateWall(int x)
    {
        tilePosCache.x = x;
        for (int i = MapMgr.Instance.MAP_WIDTH/2 - 1; i >= -MapMgr.Instance.MAP_WIDTH/2; i--)
        {
            tilePosCache.y = i;
            tilemap.SetTile(tilePosCache, stone);
        }
    }

    private void GenerarateMonster()
    {

    }
    #endregion
}
