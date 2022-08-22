using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    // Start is called before the first frame update

    //这个是速率
    


    public JArray action= JArray.Parse("[0,0,0,0]");
    private TankInfo tankinfo;
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        RibMove();
        fired();
        TurretTurn();

    }

    private void move()
    {
        action[0]= Input.GetAxis("Vertical1");//油门与刹车
        //WS方向控制
        action[1] = Input.GetAxis("Horizontal1");
        Vector3 transSpeed = default;
        //AD方向控制    
    }

    private void RibMove()
    {
        action[0] = Input.GetAxis("Vertical1");//油门与刹车
        //WS方向控制
        action[1] = Input.GetAxis("Horizontal1");
        //AD方向控制
        //


    }



    private void fired()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            action[3] = 1;
        }
        else
        {
            action[3] = 0;
        }
        
    }

    void TurretTurn()
    {
        if (Input.GetKey("k"))
        {
            action[2] = -1;
        }
        else if (Input.GetKey("l"))
        {
            action[2] = 1;
        }
        else
        {
            action[2]= 0;  
        }

    }

    long gettime()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        //得到精确到秒的时间戳（长度10位）
        long time = (long)mTimeSpan.TotalMilliseconds;
        return time;
    }

    


}
