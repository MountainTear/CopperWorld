using UnityEngine;
using UnityEngine.EventSystems; // 引入事件系统

public class Npc : MonoBehaviour, IPointerClickHandler
{
    public NpcType type;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (type == NpcType.Forge)
        {
            UIMgr.Instance.OpenView<ForgePopView>();
        }else if (type == NpcType.Order)
        {
            UIMgr.Instance.OpenView<OrderPopView>();
        }
    }
}
