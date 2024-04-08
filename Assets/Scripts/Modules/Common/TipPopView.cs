using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipPopView : ViewBase
{
	private TextMeshProUGUI text_titlecr;
	private TextMeshProUGUI text_contentcr;
    private Button btn_closecr;

    private int type;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/Common/TipPopView";
        layer = UILayer.Tip;
        if (args != null)
        {
            type = args[0] is int value ? value : 0;
        }
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
		text_titlecr = skinTrans.Find("image_bg/image_titleBg/text_titlecr").GetComponent<TextMeshProUGUI>();
		text_contentcr = skinTrans.Find("image_bg/text_contentcr").GetComponent<TextMeshProUGUI>();
        btn_closecr = skinTrans.Find("image_bg/btn_closecr").GetComponent<Button>();

        btn_closecr.onClick.AddListener(OnCloseClick);

        UpdateInfo();
   }
    #endregion

    public void OnCloseClick()
    {
        Close();
    }

    private void UpdateInfo()
    {
        Tip desc = ConfigMgr.Instance.GetTipById(type);
        text_titlecr.text = desc.title;
        text_contentcr.text = desc.content;
    }
}