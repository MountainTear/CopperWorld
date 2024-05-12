using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class WeaponPopView : ViewBase
{
	private TextMeshProUGUI text_contentcr;
	private Button btn_closecr;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/WeaponPopView";
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
        foreach (KeyValuePair<int, int> kvp in PlayerMgr.Instance.weaponList)
        {
            var config = ConfigMgr.Instance.GetWeaponById(kvp.Key);
            stringBuilder.AppendLine(config.name + ": " + kvp.Value);
        }
        text_contentcr.text = stringBuilder.ToString();
    }
}