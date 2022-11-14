using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCountrol : MonoBehaviour
{

    public float Speed = 2f;

    public GameObject[] GroundPrefabs;
    public GameObject[] EnemyPrefabs;
    public GameObject[] HiderPrefabs;
    // Update is called once per frame
    private int rand;
    private int groundIdx;
    public static GroundCountrol Instance;

    void Start()
    {
        Instance = this;

    }


    void Update()
    {
        if(UserStorage.Instance.userGameData.HP <= 0)
        {
            return;
        }

        foreach (Transform tran in transform)
        {

            // 获取位置
            Vector3 pos = tran.position;
           
            Vector2 local = tran.localScale;
            // 按照速度向左移动
            pos.x -= Speed * Time.deltaTime;


            if (pos.x  < -(tran.GetComponent<Renderer>().bounds.size.x +1))
            {
                if (tran.tag == "Enemy" || tran.tag == "Hinder")
                {
                    Destroy(tran.gameObject);
                    continue;
                }
                int index = tran.GetSiblingIndex();

                groundIdx = Random.Range(0, GroundPrefabs.Length);
                // 创建新的地面
                Transform newTrans = Instantiate(GroundPrefabs[groundIdx], transform).transform;

                Vector2 newScale = newTrans.localScale;
                newScale.x = Random.Range(0.5f, 1f);
                //newPos.x = pos.x + 7.2f * 2;
                newTrans.localScale = newScale;

                // 获取新地面的位置
                Vector2 newPos = newTrans.position;
                // 设置新地面的位置(当前位置+下一个地面的宽度+间距)
                Transform nextTrans = getNextGround(index);
                Debug.Log(pos.x);
                Debug.Log(tran.GetComponent<Renderer>().bounds.size.x);
                Debug.Log(nextTrans.GetComponent<Renderer>().bounds.size.x);
                Debug.Log(newTrans.GetComponent<Renderer>().bounds.size.x * newScale.x/2);

                //newPos.x = pos.x + tran.GetComponent<Renderer>().bounds.size.x -1f + 0.44f+ nextTrans.GetComponent<Renderer>().bounds.size.x  + newTrans.GetComponent<Renderer>().bounds.size.x * newScale.x /2;
                newPos.x =  1f + nextTrans.position.x + nextTrans.GetComponent<Renderer>().bounds.size.x/2 + newTrans.GetComponent<Renderer>().bounds.size.x * newScale.x /2;

                newPos.y = Random.Range(-1.5f, 0.2f);





                // 位置设置回去
                newTrans.position = newPos;
                // 销毁旧地面
                Destroy(tran.gameObject);

                rand = Random.Range(0, 10);
                if (rand <3)
                {
                    continue;
                }else if (rand < 6 && groundIdx !=0)
                {
                    createEnemy(newPos, newScale);
                }
                else if (rand < 11 && groundIdx != 0)
                {
                    createHinder(newPos, newScale);
                }
            }
            // 位置赋予子物体
            tran.position = pos;
            //tran.localScale = 
        }
    }

    private Transform getNextGround(int index)
    {
        int idx = index +1;
        for (; ; ) {
            Transform nextTrans = transform.GetChild(idx);
            if (nextTrans.tag == "Enemy" || nextTrans.tag == "Hinder")
            {
                idx++;
                continue;
            }
            return nextTrans;
        }
    }

    private void createEnemy(Vector2 newPos, Vector2 newScale)
    {
        // 创建敌人
        Transform newEnemy = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], transform).transform;
        Vector2 newEnemyPos = newEnemy.position;
        // 设置新敌人的位置

        newEnemyPos.x = Random.Range(newPos.x - newScale.x / 2, newPos.x + newScale.x / 2);
        newEnemyPos.y = newPos.y + 1;
        // 位置设置回去
        newEnemy.position = newEnemyPos;
    }

    private void createHinder(Vector2 newPos, Vector2 newScale)
    {
        // 创建敌人
        Transform newHinder = Instantiate(HiderPrefabs[Random.Range(0, HiderPrefabs.Length)], transform).transform;
        Vector2 newHinderPos = newHinder.position;
        // 设置新敌人的位置

        newHinderPos.x = Random.Range(newPos.x - newScale.x / 2, newPos.x + newScale.x / 2);
        newHinderPos.y = newPos.y + 1;
        // 位置设置回去
        newHinder.position = newHinderPos;
    }

    public void Clean()
    {
        foreach (Transform tran in transform)
        {
            Destroy(tran.gameObject);
        }
        Instantiate(GroundPrefabs[0], transform);
        Instantiate(GroundPrefabs[1], transform);
    }

    public void UpdateSpeed1(int level)
    {
        Speed = Speed * level;
    }
}