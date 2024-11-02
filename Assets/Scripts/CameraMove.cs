using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机跟随角移动
/// </summary>
public class CameraMove : MonoBehaviour
{
    public Transform target;    //跟随目标
    public float moveSpeed = 8; //移动速度
    private Vector3 targetPos;   //目标位置
    public float offsetY = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = target.position;
        targetPos.y += offsetY;
        targetPos.z = transform.position.z;

        //transform.position = targetPos;

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
