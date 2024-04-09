using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using static UnityEngine.Mesh;

public class ConfigMgr : Singleton<ConfigMgr>
{
    public ConfigTip configTip = new ConfigTip();

    public ConfigMgr()
    {
        InitConfig();
    }

    private void InitConfig()
    {
        LoadTipConfig();
    }

    private void LoadTipConfig()
    {
        TextAsset descJson = Resources.Load<TextAsset>("Configs/Tip");
        configTip = JsonUtility.FromJson<ConfigTip>(descJson.text);
    }

    public Tip GetTipById(int id)
    {
        return configTip.tipList.Find(tip => tip.id == id);
    }
}

[Serializable]
public class Tip
{
    public int id;
    public string title;
    public string content;
}

public class ConfigTip
{
    public List<Tip> tipList;
}