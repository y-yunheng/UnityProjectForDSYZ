using Mirror;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayercontrolNet : NetworkBehaviour
{
    // Start is called before the first frame update

    //这个是速率

    public JArray action= new JArray();
    public bool IscontrolLocal= true;
    private TankInfoNet tankinfo;
    public override void OnStartLocalPlayer()
    {
        action = JArray.Parse("[0,0,0,0]");
        tankinfo = GetComponent<TankInfoNet>();
        tankinfo.cam.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        RibMove();
        fired();
        TurretTurn();
        if(IscontrolLocal)
        {
            tankinfo.action = action;
        }
        

    }

    private void move()
    {
        action[0]= Input.GetAxis("Vertical");//油门与刹车
        //WS方向控制
        action[1] = Input.GetAxis("Horizontal");
        //AD方向控制    
    }

    private void RibMove()
    {
        action[0] = Input.GetAxis("Vertical");//油门与刹车
        //WS方向控制
        action[1] = Input.GetAxis("Horizontal");
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
        if (Input.GetKey("q"))
        {
            action[2] = -1;
        }
        else if (Input.GetKey("e"))
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
