using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUICtrl : MonoBehaviour {

    private static MainUICtrl _instance;//单例
    /// <summary>
    /// 单例
    /// </summary>
    public static MainUICtrl Instance
    {
        get
        {
            return _instance;

        }

    }
     void Awake()
    {
        _instance = this;//单例
        Time.timeScale = 1;//解决暂停返回主菜单再次开始游戏还处于暂停状态bug
    }


    public int score;
    public int length;

    private Text scoreText;//分数show
    private Text mesgText;//信息显示
    private Text lengthText;//长度显示
    private Image bgImage;//背景图改变
    private Color tempColor;

    public Sprite[] pausesprites;//暂停图片
    public bool isPause = false;//暂停
    private GameObject pausebutton;

    public bool hasBorder = true;//默认有边界
    void Start()
    {
        pausebutton = GameObject.Find("Pause");
        //实例化
        bgImage = GameObject.Find("Bg").GetComponent<Image>();
        mesgText = GameObject.Find("Msg").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        lengthText = GameObject.Find("Length").GetComponent<Text>();

        if (PlayerPrefs.GetInt("border",1)==0)
        {
            hasBorder = false;
            foreach (Transform t in bgImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }


    }
     void Update()
    {
        //退出应用
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
       
        switch (score/100)
        {
            case 3:
                {
                    ColorUtility.TryParseHtmlString("#CCEEFFF", out tempColor);
                    bgImage.color = tempColor;
                    mesgText.text = "阶段" + 2;

                }
            break;
            case 5:
                {
                    ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                    bgImage.color = tempColor;
                    mesgText.text = "阶段" + 3;
                }
                break;
            case 7:
                {
                    ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                    bgImage.color = tempColor;
                    mesgText.text = "阶段" + 4;
                }
                break;
            case 9:
                {
                    ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                    bgImage.color = tempColor;
                    mesgText.text = "阶段" + 5;
                }
                break;
            case 11:
                {
                    ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                    bgImage.color = tempColor;
                    mesgText.text = "你是开挂了吗";
                }
                break;
            default:
                break;
        }
    }
    //更新UI  加分
    public void UpdateUI(int s=5,int l=1 )
    {
        score += s;
        length += 1;
        scoreText.text = "得分:\n" + score;
        lengthText.text = "长度:\n" + length;
    }

    public void PauseClick()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;//暂停
            pausebutton.GetComponent<Image>().sprite = pausesprites[1];
        }
        else
        {
            Time.timeScale = 1;//播放
            pausebutton.GetComponent<Image>().sprite = pausesprites[0];
        }

    }
    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void Home()
    {
        SceneManager.LoadScene("Eatsnake");
        //记录得分
        PlayerPrefs.SetInt("lastlength", MainUICtrl.Instance.length);
        PlayerPrefs.SetInt("lastscore", MainUICtrl.Instance.score);//记录得分
        if (PlayerPrefs.GetInt("bestscore", 0) < MainUICtrl.Instance.score)
        {
            PlayerPrefs.SetInt("bestlength", MainUICtrl.Instance.length);
            PlayerPrefs.SetInt("bestscore", MainUICtrl.Instance.score);
        }
    }
}
