using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map
{
    public GameObject entity;
    private GameObject go_offsetcr;
    private SpriteRenderer image_bgcr;
    public Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;

    public MapIndex mapIndex;
    public MapLayerInfo layerInfo;
    private Dictionary<string, GridInfo> gridInfoDic;
    private static Vector3 posCache;
    private static Vector3Int tilePosCache;
    private static List<GenerateTarget> targetCache;
    private List<GameObject> monsterList;

    public Map(MapIndex mapIndex, MapLayerInfo layerInfo)
    {
        this.mapIndex = mapIndex;
        this.layerInfo = layerInfo;
        posCache = Vector3.zero;
        tilePosCache = Vector3Int.zero;
        gridInfoDic = new Dictionary<string, GridInfo>();
        targetCache = new List<GenerateTarget>();
        monsterList = new List<GameObject>();
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
        //变更层级
        image_bgcr.sortingOrder = (int)OrderInLayer.Map;
        tilemapRenderer.sortingOrder = (int)OrderInLayer.Tile;
        //初始化位置
        InitParentPos();
        go_offsetcr.transform.localPosition = new Vector3(0, - MapMgr.Instance.MAP_CIZE.y / 2, 0);
        //添加组件
        var collision = tilemap.gameObject.AddComponent<TilemapCollision>();
        collision.Init(this);
    }

    public void InitParentPos()
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
        gridInfoDic.Clear();
        InitGridInfoDic();
        GenerarateWall();
        GenerarateMonster();
        GenerarateMineral();
        GenerateSoil();
    }

    private void InitGridInfoDic()
    {
        for (int x = GetXMin(); x <= GetXMax(); x++)
        {
            for (int y = GetYMax(); y >= GetYMin(); y--)
            {
                gridInfoDic[CommonUtil.Instance.XYToKey(x,y)] = new GridInfo(){};
                ClearMineral(x, y);
            }
        }
        tilemap.ClearAllTiles();
        foreach(var monster in monsterList)
        {
            GameObject.Destroy(monster);
        }
    }

    private void GenerarateWall()
    {
        GenerarateWall(GetXMin());
        GenerarateWall(GetXMax());
    }

    private void GenerarateWall(int x)
    {
        for (int y = GetYMax(); y >= GetYMin(); y--)
        {
            PlaceMineral(x, y, (int)SpecialMineralId.Stone);
        }
    }

    private void GenerarateMonster()
    {
        targetCache.Clear();
        foreach (var config in ConfigMgr.Instance.configMonster.monsterList)
        {
            if (layerInfo.begin >= config.layer[0] && (layerInfo.end <= config.layer[1] || config.layer[1] <= 0))
            {
                int num = GetGenerateNumber(config.rateList);
                if (num > 0)
                {
                    targetCache.Add(new GenerateTarget { id = config.id, num = num });
                }
            }
        }
        if (targetCache.Count > 0)
        {
            foreach(var target in targetCache)
            {
                var config = ConfigMgr.Instance.GetMonsterById(target.id);
                int width = config.size[0];
                int height = config.size[1];
                int id = config.id;
                for (int i = 0; i < target.num; i++)
                {
                    if (!HasEnoughSpacePlaceMonster(width, height))
                    {
                        break;
                    }

                    bool isPlaced = false;
                    while (!isPlaced)
                    {
                        // 随机选择怪物的起始位置，以左上角为起始点放置，最边上一圈不能放置Monster
                        int startX = Random.Range(GetXMin() + 1, GetXMax() - width);
                        int startY = Random.Range(GetYMin() + height, GetYMax() - 1);

                        // 检查是否可以在这个位置放置怪物
                        if (CanPlaceMonster(startX, startY, width, height))
                        {
                            PlaceMonster(startX, startY, width, height, id);
                            isPlaced = true;
                        }
                    }
                }
            }
        }
    }

    private void GenerarateMineral()
    {
        targetCache.Clear();
        foreach (var config in ConfigMgr.Instance.configMineral.mineralList)
        {
            if (layerInfo.begin >= config.layer[0] && (layerInfo.end <= config.layer[1] || config.layer[1] <= 0))
            {
                int num = GetGenerateNumber(config.rateList);
                if (num > 0)
                {
                    targetCache.Add(new GenerateTarget { id = config.id, num = num });
                }
            }
        }
        if (targetCache.Count > 0)
        {
            foreach (var target in targetCache)
            {
                var config = ConfigMgr.Instance.GetMineralById(target.id);
                int id = config.id;
                for (int i = 0; i < target.num; i++)
                {
                    if (!HasEnoughSpacePlaceMineral())
                    {
                        break;
                    }

                    bool isPlaced = false;
                    while (!isPlaced)
                    {
                        // 随机选择矿物的位置
                        int x = Random.Range(GetXMin() + 1, GetXMax() - 1);
                        int y = Random.Range(GetYMin(), GetYMax());

                        // 检查是否可以在这个位置放置
                        if (gridInfoDic[CommonUtil.Instance.XYToKey(x, y)].type == GridType.Air)
                        {
                            PlaceMineral(x, y, id);
                            isPlaced = true;
                        }
                    }
                }
            }
        }
    }

    private void GenerateSoil()
    {
        for (int x = GetXMin() + 1; x <= GetXMax() - 1; x++)
        {
            for (int y = GetYMax(); y >= GetYMin(); y--)
            {
                if (gridInfoDic[CommonUtil.Instance.XYToKey(x, y)].type == GridType.Air)
                {
                    PlaceMineral(x, y, (int)SpecialMineralId.Soil);
                }
            }
        }
    }
    #endregion

    #region 地图生成工具函数
    private static int GetXMin()
    {
        return -MapMgr.Instance.MAP_WIDTH / 2;
    }

    private static int GetXMax()
    {
        return MapMgr.Instance.MAP_WIDTH / 2 - 1;
    }

    private static int GetYMin()
    {
        return -MapMgr.Instance.MAP_HEIGHT / 2;
    }

    private static int GetYMax()
    {
        return MapMgr.Instance.MAP_HEIGHT / 2 - 1;
    }

    private static int GetGenerateNumber(List<Rate> rates)
    {
        float roll = Random.value; // 在0到1之间随机选择一个数
        float cumulative = 0f;
        foreach (var rate in rates)
        {
            cumulative += rate.value;
            if (roll < cumulative)
            {
                return rate.num;
            }
        }
        return 0;
    }

    private bool HasEnoughSpacePlaceMonster(int width, int height)
    {
        for (int x = GetXMin(); x <= GetXMax(); x++)
        {
            for (int y = GetYMax(); y >= GetYMin(); y--)
            {
                if (CanPlaceMonster(x, y, width, height))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanPlaceMonster(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y > startY - height; y--)
            {
                if (gridInfoDic[CommonUtil.Instance.XYToKey(x,y)].type != GridType.Air)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlaceMonster(int startX, int startY, int width, int height, int id)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y > startY - height; y--)
            {
                GridInfo gridInfo = gridInfoDic[CommonUtil.Instance.XYToKey(x, y)];
                gridInfo.type = GridType.Monster;
                gridInfo.id = id;
                gridInfo.time = 0;
            }
        }
        var config = ConfigMgr.Instance.GetMonsterById(id);
        var pos = new Vector3Int(startX + width / 2, startY - height / 2, 0);
        var monster = GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/Monster/{config.prefab}"), tilemap.CellToWorld(pos), Quaternion.identity,MapMgr.Instance.monsterParent.transform);
        monsterList.Add(monster);
    }

    private bool HasEnoughSpacePlaceMineral()
    {
        for (int x = GetXMin(); x <= GetXMax(); x++)
        {
            for (int y = GetYMax(); y >= GetYMin(); y--)
            {
                if (gridInfoDic[CommonUtil.Instance.XYToKey(x, y)].type == GridType.Air)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceMineral(int x, int y, int id)
    {
        GridInfo gridInfo = gridInfoDic[CommonUtil.Instance.XYToKey(x, y)];
        gridInfo.type = GridType.Mineral;
        gridInfo.id = id;
        float time = ConfigMgr.Instance.GetMineralById(id).time;
        gridInfo.time = time > 0 ? time : float.MaxValue;
        tilePosCache.x = x;
        tilePosCache.y = y;
        tilemap.SetTile(tilePosCache, MapMgr.Instance.GetTileBaseById(id));
    }

    public void ClearMineral(int x, int y)
    {
        GridInfo gridInfo = gridInfoDic[CommonUtil.Instance.XYToKey(x, y)];
        gridInfo.type = GridType.Air;
        gridInfo.id = 0;
        gridInfo.time = 0;
        tilePosCache.x = x;
        tilePosCache.y = y;
        tilemap.SetTile(tilePosCache, null);
    }

    public void MineMineral(int x, int y)
    {
        GridInfo gridInfo = gridInfoDic[CommonUtil.Instance.XYToKey(x, y)];
        if (gridInfo.type == GridType.Mineral)
        {
            gridInfo.time -= TilemapCollision.MINE_INTERVAL;
            if (gridInfo.time < 0)
            {
                PlayerMgr.Instance.GetMineral(gridInfo.id);
                ClearMineral(x, y);
            }
        }
    }
    #endregion
}
