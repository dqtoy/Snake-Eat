using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartUIctrl : MonoBehaviour
{
    Text lastText;
    Text bestText;
    //记录toggle
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle noborder;

    private void Awake()
    {
        lastText = GameObject.Find("Last").GetComponent<Text>();
        bestText = GameObject.Find("Best").GetComponent<Text>();
        lastText.text = "上次：长度" + PlayerPrefs.GetInt("lastlength", 0) + ",分数" + PlayerPrefs.GetInt("lastscore", 0);
        bestText.text = "最好：长度" + PlayerPrefs.GetInt("bestlength", 0) + ",分数" + PlayerPrefs.GetInt("bestscore", 0);
    }
    private void Start()
    {
        //标签对上次离开游戏状态判断用于本次继续执行上次的选择模式  颜色  模式
        if (PlayerPrefs.GetString("sh","sh01")=="sh01")
        {
            blue.isOn = true;
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
        else
        {
            yellow.isOn = true;
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }

        if (PlayerPrefs.GetInt("border",1) == 1)
        {
            border.isOn = true;
            PlayerPrefs.SetInt("border", 1);
        }
        else
        {
            noborder.isOn = true;
            PlayerPrefs.SetInt("border", 0);
        }
    }
    void Update()
    {
        //退出应用
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }
    /// <summary>
    /// toggle标签  四个
    /// </summary>
    /// <param name="isOn"></param>
    //蓝色蛇
    public void BlueSelectd(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
           
        }


    }
    //黄色蛇
    public void YellowSelectd(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }


    }
    //边界模式
    public void BorderSelectd(bool isOn)
    {

        if (isOn)
        {
            PlayerPrefs.SetInt("border",1);
        }

    }
    //自由模式
    public void NoBorderSelectd(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }


    }
    //开始游戏
    public void StartGame()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");


    }



}
