using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIView : ViewBase
{
    private Slider slider_healthcr;
    private TextMeshProUGUI text_healthcr;
    private Slider slider_oxygencr;
    private TextMeshProUGUI text_oxygencr;
    private TextMeshProUGUI text_modecr;
    private TextMeshProUGUI text_modeDesccr;
    private Button btn_packagecr;
    private Button btn_weaponcr;
    private TextMeshProUGUI text_layercr;


    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/UI/GameIn/MainUIView";
        layer = UILayer.View;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        slider_healthcr = skinTrans.Find("go_state/slider_healthcr").GetComponent<Slider>();
        text_healthcr = skinTrans.Find("go_state/slider_healthcr/text_healthcr").GetComponent<TextMeshProUGUI>();
        slider_oxygencr = skinTrans.Find("go_state/slider_oxygencr").GetComponent<Slider>();
        text_oxygencr = skinTrans.Find("go_state/slider_oxygencr/text_oxygencr").GetComponent<TextMeshProUGUI>();
        text_modecr = skinTrans.Find("go_mode/text_modecr").GetComponent<TextMeshProUGUI>();
        text_modeDesccr = skinTrans.Find("go_mode/text_modeDesccr").GetComponent<TextMeshProUGUI>();
        btn_packagecr = skinTrans.Find("btn_packagecr").GetComponent<Button>();
        btn_weaponcr = skinTrans.Find("btn_weaponcr").GetComponent<Button>();
        text_layercr = skinTrans.Find("text_layercr").GetComponent<TextMeshProUGUI>();

        btn_packagecr.onClick.AddListener(OnPackageClick);
        btn_weaponcr.onClick.AddListener(OnWeaponClick);

        UpdateView();
   }
    #endregion

	public void OnPackageClick()
	{
        UIMgr.Instance.OpenView<PackagePopView>();
	}

    public void OnWeaponClick()
    {
        UIMgr.Instance.OpenView<WeaponPopView>();
    }

    public void UpdateView()
    {
        UpdateMode();
        UpdateHealth();
        UpdateOxygen();
        UpdateLayer();
    }

    public void UpdateHealth()
    {
        slider_healthcr.value = (float)PlayerMgr.Instance.healthCurrent / PlayerMgr.Instance.healthMax;
        text_healthcr.text = $"{PlayerMgr.Instance.healthCurrent}/{PlayerMgr.Instance.healthMax}";
    }

    public void UpdateOxygen()
    {
        slider_oxygencr.value = (float)PlayerMgr.Instance.oxygenCurrent / PlayerMgr.Instance.oxygenMax;
        text_oxygencr.text = $"{PlayerMgr.Instance.oxygenCurrent}/{PlayerMgr.Instance.oxygenMax}";
    }

    public void UpdateLayer()
    {
        text_layercr.text = $"（B）回城\n当前层数：{PlayerMgr.Instance.GetLayer()}";
    }

    public void UpdateMode()
    {
        if (PlayerMgr.Instance.mode == PlayerMode.Attack)
        {
            text_modecr.text = "攻击\n模式";
            text_modeDesccr.text = string.Format("攻击力：");
        }
        else if (PlayerMgr.Instance.mode == PlayerMode.Mine)
        {
            text_modecr.text = "挖矿\n模式";
            text_modeDesccr.text = string.Format("耐久度：");
        }
    }
}