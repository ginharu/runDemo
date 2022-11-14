using UnityEngine;
using TMPro;
using UnityEditor.Presets;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public readonly UserGameData userGameData;
    // 血量
    //public static int Hp = 3;
    //public static int level = 1;
    // 刚体组件
    private Rigidbody2D rbody;
    // 动画组件
    private Animator ani;
    // 当前是否碰到了地面
    private int jumpCount;
    private AsyncOperation operation;
    private float precent = 10;


    public TextMeshProUGUI LifeText;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI LevelText;

    private IEnumerator loadSceneAndData()
    {
        // 加载场景
        operation = SceneManager.LoadSceneAsync("Load");
        operation.allowSceneActivation = false;
        yield return operation;
    }
    
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        ani = GetComponent<Animator>();
        StartCoroutine(loadSceneAndData());

        BgControl.Instance.UpdateSpeed(UserStorage.Instance.userGameData.Level);
        GroundCountrol.Instance.UpdateSpeed1(UserStorage.Instance.userGameData.Level);
        LifeText.text = "LIFE: X" + UserStorage.Instance.userGameData.HP.ToString();

    }


    void Update()
    {
        precent = operation == null ? 10 : operation.progress * 100;
        // renderPrecent();
        //// 水平轴 -1 0 1
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0 && UserStorage.Instance.userGameData.HP != 0)
        {
            // 移动
            ani.SetBool("IsRun", true);
            // 转身
            transform.localScale = new Vector3(horizontal < 0 ? -1f : 1f, 1f, 1f);
            // 移动
            transform.Translate(Vector2.right * horizontal * 1 * Time.deltaTime);
        }
        else
        {
            ani.SetBool("IsRun", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }



    public void Jump()
    {
        if (jumpCount<2)
        {
            jumpCount++;
            rbody.AddForce(Vector2.up * 400);
            AudioManager.Instance.Play("跳");
        }

    }
    
    // 发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 判断如果是地面
        if (collision.collider.tag == "Ground")
        {
            jumpCount = 0;
            // 结束跳跃
            ani.SetBool("IsJump", false);
        }

        // 判断如果是死亡边界
        if (collision.collider.tag == "Die")
        {
            died();
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            ani.SetBool("IsJump", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            CoinText.text = "Score: " + UserStorage.Instance.userGameData.CoinCount + "";
            LevelText.text = "LEVEL " + UserStorage.Instance.userGameData.Level + "";
            if (UserStorage.Instance.userGameData.CoinCount > (2 << (UserStorage.Instance.userGameData.Level - 1)) * 100) 
            {
                UserStorage.UpgradeLevel();
                //UserStorage.Instance.userGameData.Level++;
                BgControl.Instance.UpdateSpeed(UserStorage.Instance.userGameData.Level);
                GroundCountrol.Instance.UpdateSpeed1(UserStorage.Instance.userGameData.Level);
            }
        }

        if (collision.tag == "Enemy")
        {
            died();
        }
    }

    public void died()
    {

        // 血量减一
        UserStorage.Instance.userGameData.HP -= 1;
        UserStorage.Die();
        LifeText.text = "LIFE: X" + UserStorage.Instance.userGameData.HP.ToString();
        // 播放死亡声音
        AudioManager.Instance.Play("Boss死了");
        //  播放死亡动画
        ani.SetBool("IsDie", true);

        if (UserStorage.Instance.userGameData.HP > 0)
        {

            GroundCountrol.Instance.Clean();
            ani.SetBool("IsDie", false);
            ani.SetBool("IsRun", true);
            // 获取位置
            Vector3 pos = transform.position;
            pos.x = -2.32f;
            pos.y = 1.39f;
            // 位置赋予子物体
            transform.position = pos;
        }
        else
        {
            UserStorage.Die();
            ani.SetBool("IsRun", false);
        }
    }
}
