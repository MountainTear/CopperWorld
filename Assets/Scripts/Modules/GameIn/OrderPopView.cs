using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class OrderPopView : ViewBase
{
	private TextMeshProUGUI text_contentcr;
	private Button btn_okcr;
	private Button btn_closecr;

    private int id;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/OrderPopView";
        layer = UILayer.Tip;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
		text_contentcr = skinTrans.Find("image_bg/text_contentcr").GetComponent<TextMeshProUGUI>();
		btn_okcr = skinTrans.Find("image_bg/btn_okcr").GetComponent<Button>();
		btn_closecr = skinTrans.Find("image_bg/btn_closecr").GetComponent<Button>();

		btn_okcr.onClick.AddListener(OnOkClick);
		btn_closecr.onClick.AddListener(OnCloseClick);

        InitInfo();
   }
    #endregion

    public void InitInfo()
    {
        id = Random.Range(1, ConfigMgr.Instance.configOrder.orderList.Count + 1);
        UpdateView();
    }

    public void UpdateView()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("如果給我");
        foreach (var cost in ConfigMgr.Instance.GetOrderById(id).costList)
        {
            stringBuilder.Append($"{cost.num}个{ConfigMgr.Instance.GetWeaponById(cost.id).name}");
        }
        stringBuilder.Append($"，我可以给你{ConfigMgr.Instance.GetOrderById(id).money}个金币");
        text_contentcr.text = stringBuilder.ToString();
    }

    public void OnOkClick()
    {
        UIUtils.Instance.ToolTip(TipType.Weapon);
        Close();
    }

    public void OnCloseClick()
    {
        Close();
    }
}