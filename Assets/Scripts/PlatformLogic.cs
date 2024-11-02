using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台逻辑处理类
/// </summary>
public class PlatformLogic 
{
    private Role role;
    private Platform curPlatform;
    private List<Platform> platformList;
    
    public PlatformLogic(Role role)
    {
        this.role = role;
        curPlatform = null;
        platformList = PlatformDataMgr.Instance.platformlist;
    }

    /// <summary>
    /// 检测玩家平台变化的函数
    /// </summary>
    public void UpdateCheck()
    {
        //当玩家跳跃、下落时，判断是否能够切换平台
        if (role.isJump||role.isFall)
        {
            curPlatform = null; //让每次寻找到的 是这一帧最高的平台 而不是整个跳跃过程中的最高平台
            for (int i = 0; i < platformList.Count; i++)
            {
                //【玩家是否处于落在某个平台的条件下
                if (platformList[i].CheckObjectFallOnSelf(role.transform.position) && (curPlatform == null || curPlatform.Y < platformList[i].Y))
                {
                    curPlatform = platformList[i];
                    role.ChangePlatformData(curPlatform.Y, curPlatform.canShowShadow, curPlatform.canFall);
                }
            }
        }


        if (!role.isJump && !role.isFall && curPlatform != null && !curPlatform.CheckObjectFallOnSelf(role.transform.position))
        {
            role.Fall();
        }
    }

}
