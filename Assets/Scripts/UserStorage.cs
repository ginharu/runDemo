using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

/**
 * <summary>用户存储</summary>
 * 
 */
public class UserStorage
{
    public readonly UserGameData userGameData;
    public static string file = "run.json";
    // 单例对象
    public readonly static UserStorage Instance = new UserStorage();

    public UserStorage()
    {
        // 加载数据
        UserGameData data = JSONUtils.Load<UserGameData>(file);
        if (data == null || data.Level == 0 || data.HP == 0)
        {
            Debug.Log("clean");
            data = new UserGameData();
            clear(data);
        }
        userGameData = data;
    }

    /**
     * <summary>获取数据</summary>
     * <returns>返回 数据</returns>
     */
    public static UserGameData Get()
    {
        return Instance.userGameData;
    }

    /**
     * <summary>升级</summary>
     * 
     */
    public static void UpgradeLevel()
    {
        Instance.userGameData.Level += 1;
        //int level = Instance.userGameData.Level;
        //isStopped = true;
        // 发送升级事件
        JSONUtils.Save<UserGameData>(Instance.userGameData, file);
        //PostNotification.Post(Const.Notification.PassedLevel, Instance);
    }

    /**
     * <summary>增加积分</summary>
     * <param name="score">添加的积分数</param>
     */
    public static void AddScore(int score)
    {
        Instance.userGameData.CoinCount += score;
            //// 升级
            //UpgradeLevel();
        JSONUtils.Save<UserGameData>(Instance.userGameData, file);
    }

    /**
     * <summary>角色死亡</summary>
     */
    public static void Die()
    {
        //Instance.userGameData.LifeNum--;
        JSONUtils.Save<UserGameData>(Instance.userGameData, file);

        //if (Instance.userGameData.LifeNum > 0)
        //{
        //    PostNotification.Post(Const.Notification.PlayerDie, Instance);
        //}
        //else
        //{
        //    SceneManager.LoadScene("GameOver");
        //    PostNotification.Post(Const.Notification.GameOver, Instance);
        //}
    }

    /**
     * <summary>游戏是否结束</summary>
     * <returns>true 已经结束，false 未结束</returns>
     */
    public static bool IsGameOver()
    {

        return Instance.userGameData.HP <= 0;
    }

    private static void clear(UserGameData userGameData)
    {
        userGameData.Level = 1;
        userGameData.CoinCount = 0;
        userGameData.HP = 3;
    }

    //public static bool IsStopped()
    //{
    //    return isStopped;
    //}

    //public static void SetContinue()
    //{
    //    isStopped = false;
    //}

    //public static void SetStop()
    //{
    //    isStopped = true;
    //}

    private void restart()
    {
        // 加载数据
        clear(userGameData);
        JSONUtils.Save<UserGameData>(userGameData);
    }

    /**
     * <summary>重新开始</summary>
     * 
     */
    public static void Restart()
    {
        Instance.restart();
    }
}

