using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using static UnityEngine.Mesh;

public class ConfigMgr : Singleton<ConfigMgr>
{
    public ConfigTip configTip = new ConfigTip();
    public ConfigMineral configMineral = new ConfigMineral();
    public ConfigMonster configMonster = new ConfigMonster(); 

    public ConfigMgr()
    {
        InitConfig();
    }

    private void InitConfig()
    {
        LoadTipConfig();
        LoadMineralConfig();
        LoadMonsterConfig();
    }

    private void LoadTipConfig()
    {
        TextAsset descJson = Resources.Load<TextAsset>("Configs/Tip");
        configTip = JsonUtility.FromJson<ConfigTip>(descJson.text);
    }

    public TipC GetTipById(int id)
    {
        return configTip.tipList.Find(tip => tip.id == id);
    }

    private void LoadMineralConfig()
    {
        TextAsset descJson = Resources.Load<TextAsset>("Configs/Mineral");
        configMineral = JsonUtility.FromJson<ConfigMineral>(descJson.text);
    }

    public MineralC GetMineralById(int id)
    {
        return configMineral.mineralList.Find(Mineral => Mineral.id == id);
    }

    private void LoadMonsterConfig()
    {
        TextAsset descJson = Resources.Load<TextAsset>("Configs/Monster");
        configMonster = JsonUtility.FromJson<ConfigMonster>(descJson.text);
    }

    public MonsterC GetMonsterById(int id)
    {
        return configMonster.monsterList.Find(Monster => Monster.id == id);
    }
}

[Serializable]
public class TipC
{
    public int id;
    public string title;
    public string content;
}

public class ConfigTip
{
    public List<TipC> tipList;
}

[Serializable]
public class MineralC
{
    public int id;
    public string name;
    public List<int> layer;
    public string tile;
    public List<Rate> rateList;
}

[Serializable]
public class Rate
{
    public float value;
    public int num;
}

[Serializable]
public class ConfigMineral
{
    public List<MineralC> mineralList;
}

[Serializable]
public class MonsterC
{
    public int id;
    public string name;
    public List<int> layer;
    public List<int> size;
    public List<Rate> rateList;
}

[Serializable]
public class ConfigMonster
{
    public List<MonsterC> monsterList;
}