using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;

public class PackagePopView : ViewBase
{
	private TextMeshProUGUI text_contentcr;
	private Button btn_closecr;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/PackagePopView";
        layer = UILayer.Tip;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
		text_contentcr = skinTrans.Find("image_bg/text_contentcr").GetComponent<TextMeshProUGUI>();
		btn_closecr = skinTrans.Find("image_bg/btn_closecr").GetComponent<Button>();

		btn_closecr.onClick.AddListener(OnCloseClick);

        UpdateView();
    }
    #endregion

	public void OnCloseClick()
	{
        Close();
    }

    private void UpdateView()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<int, int> kvp in PlayerMgr.Instance.mineralList)
        {
            var config = ConfigMgr.Instance.GetMineralById(kvp.Key);
            stringBuilder.AppendLine(config.name + ": " + kvp.Value);
        }
        text_contentcr.text = stringBuilder.ToString();
    }
}