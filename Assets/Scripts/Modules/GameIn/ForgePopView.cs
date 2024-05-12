using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgePopView : ViewBase
{
	private TextMeshProUGUI text_contentcr;
	private Button btn_okcr;
	private Button btn_closecr;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/ForgePopView";
        layer = UILayer.View;
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
   }
    #endregion

	public void OnOkClick()
	{
		// TODO: Add your button click handling logic here
	}

	public void OnCloseClick()
	{
		Close();
	}
}