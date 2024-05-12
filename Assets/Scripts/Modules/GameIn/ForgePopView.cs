using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ForgePopView : ViewBase
{
	private TextMeshProUGUI text_contentcr;
	private Button btn_okcr;
	private Button btn_closecr;

	private int id;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/ForgePopView";
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
        id = Random.Range(1, ConfigMgr.Instance.configForge.forgeList.Count + 1);
		UpdateView();
    }

	public void UpdateView()
	{
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("如果有");
        foreach (var cost in ConfigMgr.Instance.GetForgeById(id).costList)
        {
            stringBuilder.Append( $"{ cost.num}个{ ConfigMgr.Instance.GetMineralById(cost.id).name}");
        }
        stringBuilder.Append($"，我可以帮你锻造一把{ConfigMgr.Instance.GetWeaponById(id).name}");
        text_contentcr.text = stringBuilder.ToString();
    }

	public void OnOkClick()
	{
        UIUtils.Instance.ToolTip(TipType.Mineral);
        Close();
    }

	public void OnCloseClick()
	{
		Close();
	}
}