using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public Map map;
    public static float MINE_INTERVAL = 0.1f;    //判断矿物破坏间隔
    private float lastCollisionTime;

    public void Init(Map map)
    {
        this.map = map;
        tilemap = map.tilemap;
    }

    private bool IsCanMine(Collider2D collision)
    {
        if (collision.CompareTag(SpecialTag.Weapon.ToString()) && PlayerMgr.Instance.mode == PlayerMode.Mine)
            return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCanMine(collision))
        {
            lastCollisionTime = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsCanMine(collision))
        {
            BoxCollider2D boxCollision = (BoxCollider2D)collision;
            if (Time.time - lastCollisionTime >= MINE_INTERVAL)
            {
                // 将世界坐标矩形转换为tilemap坐标矩形
                Vector3 weaponPos = boxCollision.transform.position + Vector3.Scale((Vector3)boxCollision.offset, boxCollision.transform.localScale);
                Vector3 size = boxCollision.size;
                Vector3Int minCell = tilemap.WorldToCell(weaponPos - size / 2);
                Vector3Int maxCell = tilemap.WorldToCell(weaponPos + size / 2);

                // 遍历所有的tile，判断每个tile是否与矩形相交
                for (int x = minCell.x; x <= maxCell.x; x++)
                {
                    for (int y = minCell.y; y <= maxCell.y; y++)
                    {
                        Vector3Int cellPos = new Vector3Int(x, y, 0);
                        if (tilemap.HasTile(cellPos))
                        {
                            map.MineMineral(x, y);
                        }
                    }
                }
                lastCollisionTime = Time.time;
            }
        }
    }
}
