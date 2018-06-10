using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//using System.Linq;//  Last使用

/// <summary>
/// 蛇控制
/// </summary>
public class Snakehead : MonoBehaviour
{


    public List<Transform>  bodyList= new List<Transform>();
    public float velocity = 0.5f;//加速
    private int step=30;//一步长度
    private int x;
    private int y;
    private Vector3 headpos;
    private Transform canvas;
    public bool isDie = false;//是否死亡

    private GameObject bodyPrefab;
    private Sprite[] bodySprite=new Sprite[2];//舍身的图片2张

    private static Snakehead _instance;//单例

    public AudioClip eatclip;
    public AudioClip dieclip;
    // private GameObject dieEffect;//死亡特效
    /// <summary>
    /// 单例
    /// </summary>
    public static Snakehead Instance
    {
        get
        {
            return _instance;

        }

    }



    void Awake()
    {
        canvas=GameObject.Find("Canvas").transform;
        _instance = this;//单例
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprite[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprite[1]= Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
        //Resources.Load();
    }
    void Start()
    {
        InvokeRepeating("Move",0,velocity);//调用 间隔固定时间调用可实现update效果
        x = 0;y = step;//刚开始向上走
        bodyPrefab = Resources.Load<GameObject>("Prefabs/SnakeBody");//获取舍身预设物
       // dieEffect= Resources.Load<GameObject>("Prefabs/effect");//获取特效预设物

    }
    void Update()
    {
        ControlButtonClick();//调用控制键
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {

        headpos = gameObject.transform.localPosition;//保存舌头移动前的位置
        gameObject.transform.localPosition = new Vector3(headpos.x+x,headpos.y+y,headpos.z);//舌头向希望位置移动
       //舍身移动
        if (bodyList.Count > 0)
        {
            /**********蛇身移动方法一：蛇尾移动到蛇头移动前的位置 弃用了***********/
            //    此让舍身跟随的方式有两个缺点 舍身颜色无顺序  舍身生成会在原点闪一下再出现在舌头位置
            //    bodyList.Last().localPosition = headpos;//将最后一个舍身位置移到舌头
            //    bodyList.Insert(0, bodyList.Last());//蛇尾插入序列
            //    bodyList.RemoveAt(bodyList.Count - 1);//移除末尾
            /************方法二：从后往前开始移动舍身*********/
            for (int i = bodyList.Count-2; i >=0; i--)
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;//每一个舍身都移动到他前面一个节点的位置
            }
            bodyList[0].localPosition = headpos;//第一个舍身移动到舌头原位置

        }

    }


    /// <summary>
    /// 蛇生长
    /// </summary>
    void Grow()
    {
        AudioSource.PlayClipAtPoint(eatclip,Vector3.zero);//播放音效
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;//三目运算
        GameObject body = (GameObject)Instantiate(bodyPrefab,new Vector3 (1000,1000,0),Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprite[index];
        body.transform.SetParent(canvas, false);
        bodyList.Add(body.transform);
    }
    
    
    
    /// <summary>
    /// 死亡
    /// </summary>
    void Die()
    {
        AudioSource.PlayClipAtPoint(dieclip, Vector3.zero);//播放音效
        CancelInvoke();
        isDie = true;
        // Instantiate(dieEffect);
        //记录得分
        PlayerPrefs.SetInt("lastlength",MainUICtrl.Instance.length);
        PlayerPrefs.SetInt("lastscore", MainUICtrl.Instance.score);//记录得分
        if (PlayerPrefs.GetInt("bestscore",0)< MainUICtrl.Instance.score)
        {
            PlayerPrefs.SetInt("bestlength", MainUICtrl.Instance.length);
            PlayerPrefs.SetInt("bestscore", MainUICtrl.Instance.score);
        }
        StartCoroutine(Gameover(1.5f));

    }
    IEnumerator Gameover(float t)
    {

        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 碰撞检测 食物  蛇身
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Food"))
        {
            //检测到食物  吃了它
            Destroy(collision.gameObject);//吃掉食物
            MainUICtrl.Instance.UpdateUI(5, 1);//加分  计算长度
            Grow();
            FoodMaker.Instance.MakeFood(Random.Range(0, 100) < 20 ? true : false);
            //if (Random.Range(0,100)<20)
            //{
            //    FoodMaker.Instance.MakeFood(true);
            //}
            //else
            //{
            //    FoodMaker.Instance.MakeFood(false);
            //}
           
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            //奖励
            Destroy(collision.gameObject);
            MainUICtrl.Instance.UpdateUI(Random.Range(1,10)*5,1);
            Grow();

        }
        else if(collision.gameObject.CompareTag("Body"))
        {
            //碰到自己
            Die();

        }
        else
        {
            //撞到边界
            if (MainUICtrl.Instance.hasBorder)
            {
                //边界模式
                Die();
            }
            else
            {

                // 自由模式
                switch (collision.gameObject.name)
                {
                    //自由模式
                    case "up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 13 * 30, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 14.5f * 30, transform.localPosition.y, transform.localPosition.z);
                        break;
                    default:
                        break;
                }
            }
          
            

        }
    }
    /// <summary>
    /// 控制转向 加速
    /// </summary>
    private void ControlButtonClick()
    {
        if (Input.GetKeyDown(KeyCode.W) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
            //W键
            if (y != -step)
            {
                //点击W  Up
                x = 0; y = step;
                //gameObject.transform.localRotation = Quaternion.Euler(0,0,0);//四元数 转头
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.A) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
            if (x != step)
            {
                //点击A  Left
                x = -step; y = 0;
                //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);//四元数转头
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
            if (y != step)
            {
                //点击S Down
                x = 0; y = -step;
                //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);//四元数转头
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

            }
        }
        if (Input.GetKeyDown(KeyCode.D) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
            if (x != -step)
            {
                //点击D   Right
                x = step; y = 0;
                //gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);//四元数转头
                gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
            }
        }
        //加速按钮空格
        if (Input.GetKeyDown(KeyCode.Space) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
          
            CancelInvoke();
            InvokeRepeating("Move", 0, (velocity - 0.2f));

        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUICtrl.Instance.isPause == false && isDie == false)
        {
            //调用控制速度  实现加速
           CancelInvoke();
           InvokeRepeating("Move", 0, velocity);
        }

    }

    //摇杆按钮  W A S D 控制上下左右
    public void yaoganBtnClick(int index)
    {
        if (MainUICtrl.Instance.isPause==false && Snakehead.Instance.isDie == false)
        {
            switch (index)
            {
                case 0:
                    {
                        if (y != -step)
                        {
                            //点击W  Up
                            x = 0; y = step;
                            //gameObject.transform.localRotation = Quaternion.Euler(0,0,0);//四元数 转头
                            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }

                    }
                    break;
                case 1:
                    {
                        if (y != step)
                        {
                            //点击S Down
                            x = 0; y = -step;
                            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);//四元数转头
                            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

                        }
                    }
                    break;
                case 2:
                    {
                        if (x != -step)
                        {
                            //点击D   Right
                            x = step; y = 0;
                            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);//四元数转头
                            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
                        }
                    }
                    break;
                case 3:
                    {
                        if (x != step)
                        {
                            //点击A  Left
                            x = -step; y = 0;
                            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);//四元数转头
                            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
       

    }



}
