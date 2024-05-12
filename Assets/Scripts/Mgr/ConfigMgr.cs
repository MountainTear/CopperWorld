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
    public ConfigWeapon configWeapon = new ConfigWeapon();
    public ConfigForge configForge = new ConfigForge();
    public ConfigOrder configOrder = new ConfigOrder();

    public ConfigMgr()
    {
        InitConfig();
    }

    private void InitConfig()
    {
        LoadTipConfig();
        LoadMineralConfig();
        LoadMonsterConfig();
        LoadWeaponConfig();
        LoadForgeConfig();
        LoadOrderConfig();
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

    private void LoadWeaponConfig()
    {
        TextAsset weaponJson = Resources.Load<TextAsset>("Configs/Weapon");
        configWeapon = JsonUtility.FromJson<ConfigWeapon>(weaponJson.text);
    }

    public WeaponC GetWeaponById(int id)
    {
        return configWeapon.weaponList.Find(Weapon => Weapon.id == id);
    }

    private void LoadForgeConfig()
    {
        TextAsset forgeJson = Resources.Load<TextAsset>("Configs/Forge");
        configForge = JsonUtility.FromJson<ConfigForge>(forgeJson.text);
    }

    public ForgeC GetForgeById(int id)
    {
        return configForge.forgeList.Find(Forge => Forge.id == id);
    }

    private void LoadOrderConfig()
    {
        TextAsset orderJson = Resources.Load<TextAsset>("Configs/Order");
        configOrder = JsonUtility.FromJson<ConfigOrder>(orderJson.text);
    }

    public OrderC GetOrderById(int id)
    {
        return configOrder.orderList.Find(Order => Order.id == id);
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
    public float time;
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
    public string prefab;
    public List<Rate> rateList;
}

[Serializable]
public class ConfigMonster
{
    public List<MonsterC> monsterList;
}

[Serializable]
public class WeaponC
{
    public int id;
    public string name;
    public int attack;
}

[Serializable]
public class ConfigWeapon
{
    public List<WeaponC> weaponList;
}

[Serializable]
public class CostItem
{
    public int id;
    public int num;
}

[Serializable]
public class ForgeC
{
    public int id;
    public int weapon;
    public List<CostItem> costList;
}

[Serializable]
public class ConfigForge
{
    public List<ForgeC> forgeList;
}

[Serializable]
public class OrderC
{
    public int id;
    public int money;
    public List<CostItem> costList;
}

[Serializable]
public class ConfigOrder
{
    public List<OrderC> orderList;
}