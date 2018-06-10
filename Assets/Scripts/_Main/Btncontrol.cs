using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 蛇速度控制按钮脚本 点击可以加速
/// </summary>
public class Btncontrol : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 延迟时间  
    private float delay = 3f;

    // 按钮是否是按下状态  
    private bool isDown = false;

    // 按钮最后一次是被按住状态时候的时间  
    private float lastIsDownTime;



    void Update()
    {
        // 如果按钮是被按下状态  
        if (isDown)
        {
            // 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒  
            if (Time.time - lastIsDownTime > delay)
            {
                // 触发长按方法  
              //  Debug.Log("长按");
 
                // 记录按钮最后一次被按下的时间  
                lastIsDownTime = Time.time;

            }
           
        }

    }

    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown(PointerEventData eventData)
    {

        if (MainUICtrl.Instance.isPause==false&&Snakehead.Instance.isDie==false)
        {
            isDown = true;
            GameObject.Find("SnakeHead").GetComponent<Snakehead>().CancelInvoke();
            GameObject.Find("SnakeHead").GetComponent<Snakehead>().InvokeRepeating("Move", 0, GameObject.Find("SnakeHead").GetComponent<Snakehead>().velocity - 0.2f);
            lastIsDownTime = Time.time;
        }
       
     
    }

    // 当按钮抬起的时候自动调用此方法  
    public void OnPointerUp(PointerEventData eventData)
    {
        if (MainUICtrl.Instance.isPause == false && Snakehead.Instance.isDie == false)
        {
            isDown = false;
            //调用控制速度  实现加速
            GameObject.Find("SnakeHead").GetComponent<Snakehead>().CancelInvoke();
            GameObject.Find("SnakeHead").GetComponent<Snakehead>().InvokeRepeating("Move", 0, GameObject.Find("SnakeHead").GetComponent<Snakehead>().velocity);
            //Debug.Log("抬起");
        }
    }

    // 当鼠标从按钮上离开的时候自动调用此方法  
    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
       // Debug.Log("离开");
    }
}
