using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgControl : MonoBehaviour
{

    public float Speed = 0.2f;
    public static BgControl Instance;

    void Start()
    {
        Instance = this;

    }
    // Update is called once per frame
    void Update()
    {
        if (UserStorage.Instance.userGameData.HP <= 0)
        {
            return;
        }
        foreach (Transform tran in transform)
        {
            // 获取位置
            Vector3 pos = tran.position;
            // 按照速度向左移动
            pos.x -= Speed * Time.deltaTime;
            // 判断是否出了屏幕
            if (pos.x < -7.2f)
            {
                //把图片移动到右边
                pos.x += 7.2f * 2;
            }
            // 位置赋予子物体
            tran.position = pos;
        }
    }

    public void UpdateSpeed(int level)
    {
        Speed = Speed * level;
    }
}
