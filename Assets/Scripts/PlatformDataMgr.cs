using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台数据管理器
/// 主要用于一次性获取数据 避免之后再多次单独获取
/// </summary>
public class PlatformDataMgr
{
    private static PlatformDataMgr instance;
    public static PlatformDataMgr Instance
    {
        get
        {
            if(instance == null)
                instance = new PlatformDataMgr();
            return instance;
        }
    }

    public List<Platform> platformlist = new List<Platform>();

    public void AddPlatform(Platform data)
    {
        platformlist.Add(data);
    }

    public void RemovePlatform(Platform data)
    {
        if (platformlist.Contains(data))
            platformlist.Remove(data);
    }

    public void ClearData()
    {
        platformlist.Clear();
    }
}