using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour {

    private static FoodMaker _instance;//单例
    /// <summary>
    /// 单例
    /// </summary>
    public static FoodMaker Instance
    {
        get
        {
            return _instance;

        }

    }

    private int xlimit = 31;
    private int ylimit = 17;
    private int xoffset = 14;
    private GameObject foodprefab;//食物预设物
    private GameObject rewardPrefab;//奖励预设物
    public Sprite[] foodsprites;
     
    private Transform foodHolder;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {

        foodHolder = GameObject.Find("FoodMaker").transform;
        foodprefab = Resources.Load<GameObject>("Prefabs/Food");//获取 食物预设物
        rewardPrefab= Resources.Load<GameObject>("Prefabs/Reward");//获取 奖励预设物
        MakeFood(false);
    }
    /// <summary>
    /// 生成食物
    /// </summary>
    public  void  MakeFood(bool isReward)
    {
        int index = Random.Range(0, foodsprites.Length);//随机图片
        GameObject food = Instantiate(foodprefab);//生成
        food.transform.SetParent(foodHolder, false);//设置副物体 false意思是不设置为世界坐标
        food.GetComponent<Image>().sprite = foodsprites[index];
        int x = Random.Range(-xlimit + xoffset, xlimit);
        int y = Random.Range(-ylimit, ylimit);
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        if (isReward)
        {
           
            GameObject reward = Instantiate(rewardPrefab);//生成
            reward.transform.SetParent(foodHolder, false);//设置副物体 false意思是不设置为世界坐标
           
             x = Random.Range(-xlimit + xoffset, xlimit);
             y = Random.Range(-ylimit, ylimit);
            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }



}
