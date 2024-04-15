using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtil : Singleton<CommonUtil>
{
    public string XYToKey(int x, int y)
    {
        string key = x.ToString() + "_" + y.ToString();
        return key;
    }

    public (int, int) KeyToXY(string key)
    {
        string[] parts = key.Split('_');
        if (parts.Length == 2)
        {
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            return (x, y);
        }
        else
        {
            throw new ArgumentException("Invalid key format.", nameof(key));
        }
    }
}