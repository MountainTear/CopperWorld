using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleView : ViewBase
{
	private Button btn_startcr;
	private Button btn_infocr;
	private Button btn_endcr;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/SignIn/TitleView";
        layer = UILayer.View;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
		btn_startcr = skinTrans.Find("btn_startcr").GetComponent<Button>();
		btn_infocr = skinTrans.Find("btn_infocr").GetComponent<Button>();
		btn_endcr = skinTrans.Find("btn_endcr").GetComponent<Button>();

		btn_startcr.onClick.AddListener(OnStartClick);
		btn_infocr.onClick.AddListener(OnInfoClick);
		btn_endcr.onClick.AddListener(OnEndClick);
   }
    #endregion

	public void OnStartClick()
	{
		// TODO: Add your button click handling logic here
	}

	public void OnInfoClick()
	{
		UIUtils.Instance.ToolTip(TipType.Info);
	}

	public void OnEndClick()
	{
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}