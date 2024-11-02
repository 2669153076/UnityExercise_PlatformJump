using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色控制相关
/// </summary>
public class Role : MonoBehaviour
{
    public float moveSpeed = 5;

    public float initYSpeed = 10;   //初始跳跃速度
    private float curYSpeed;    //当前速度
    public float G = 9.8f;  //重力加速度
    private float curPlatformY = 0; //当前平台的 Y 值（平台高度）
    private int jumpIndex;  //二段跳计数

    public GameObject shadow;   //影子
    private Vector3 shadowPos;  //影子位置
    public float jumpMaxH = 3;  //角色跳跃最大位置 当到达这个位置时，影子缩放为0

    private PlatformLogic platformLogic;    //处理上、下平台的逻辑对象
    public bool isJump => animator.GetBool("IsJump");   //是否处于跳跃状态
    public bool canFall;    //是否可以主动下落
    public bool isFall=>animator.GetBool("IsFall"); //是否在下落

    private float horizontal;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shadowPos = shadow.transform.position;
        platformLogic = new PlatformLogic(this);
    }

    // Update is called once per frame
    void Update()
    {
        #region 移动
        Move();

        #endregion

        #region 跳跃
        Jump();
        #endregion

        #region 影子
        Shadow();
        #endregion

        #region 平台
        SwitchPlatform();
        #endregion


    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (horizontal != 0)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        animator.SetBool("IsRun", horizontal != 0);
    }

    private void Jump()
    {
        
        if (!isJump && !isFall && canFall  && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            //未处于跳跃、下落
            //平台可以下落
            //按下按键
            //下落
            Fall();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpIndex < 2)
        {
            animator.SetBool("IsJump", true);

            curYSpeed = initYSpeed;

            ++jumpIndex;
        }
        if (isJump||isFall) //跳跃、下落状态时，y坐标变化
        {
            //当跳跃或下落时 第一帧位移之前，就让当前Y速度产生变化

            //受重力影响 每一帧降低Y轴速度
            curYSpeed -= G * Time.deltaTime;

            transform.Translate(Vector3.up * curYSpeed * Time.deltaTime);
            
            animator.SetBool("IsFall", curYSpeed < 0);


            if (transform.position.y <= curPlatformY)
            {
                animator.SetBool("IsJump", false);
                animator.SetBool("IsFall", false);
                Vector3 pos = transform.position;
                pos.y = curPlatformY;
                transform.position = pos;

                jumpIndex = 0;
            }
        }
    }
    
    private void Shadow()
    {
        shadowPos.x = this.transform.position.x;
        shadowPos.y = curPlatformY; //影子y的位置和平台位置一致
        shadow.transform.position = shadowPos;

        shadow.transform.localScale = 1.5f * Vector3.one * Mathf.Max(0, jumpMaxH - (transform.position.y - curPlatformY)) / jumpMaxH;
    }

    public void SwitchPlatform()
    {
        platformLogic.UpdateCheck();

    }

    /// <summary>
    /// 改变平台相关信息
    /// </summary>
    /// <param name="y"></param>
    /// <param name="isShowShadow"></param>
    /// <param name="canFall"></param>
    public void ChangePlatformData(float y, bool isShowShadow, bool canFall)
    {
        curPlatformY = y;
        shadow.SetActive(isShowShadow);
        this.canFall = canFall;
    }

    /// <summary>
    /// 玩家下落方法
    /// </summary>
    public void Fall()
    {
        animator.SetBool("IsFall", true);
        //相当于将平台设置为空
        //因为角色脚本里没有平台相关数据，所以需要将它改为很小的数
        curPlatformY = -9999;
        //因为是自由落体 所以Y上的速度是0
        curYSpeed = 0;
        //由于在下落时只允许跳跃一次，将计数置为1，就不会出现下落时还会二段跳
        jumpIndex = 1;

    }
}
