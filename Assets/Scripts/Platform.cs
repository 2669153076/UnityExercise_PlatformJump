using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台数据类
/// </summary>
public class Platform : MonoBehaviour
{
    public float width = 5; //平台宽
    public float Y => transform.position.y; //平台对应Y坐标
    public float Left => transform.position.x - width / 2;  //平台左边界
    public float Right => transform.position.x + width / 2; //平台右边界

    public bool canShowShadow = true;   //是否显示影子
    public bool canFall = false;    //是否能够下落，穿过平台

    /// <summary>
    /// 检测玩家是否可以落在平台上
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckObjectFallOnSelf(Vector3 pos)
    {
        if (pos.y >= Y && pos.x < Right && pos.x > Left)
        {
            return true;
        }
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlatformDataMgr.Instance.AddPlatform(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - Vector3.right * width / 2,transform.position+Vector3.right*width/2);

    }
}
