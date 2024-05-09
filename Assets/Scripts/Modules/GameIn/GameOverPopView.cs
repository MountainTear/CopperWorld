using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPopView : ViewBase
{
	private Button btn_closecr;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/GameOverPopView";
        layer = UILayer.Tip;

        isTimePause = true;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
		btn_closecr = skinTrans.Find("image_bg/btn_closecr").GetComponent<Button>();

		btn_closecr.onClick.AddListener(OnCloseClick);
   }
    #endregion

	public void OnCloseClick()
	{
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}