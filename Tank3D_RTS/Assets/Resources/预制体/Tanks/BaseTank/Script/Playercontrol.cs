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
    public JArray dpos = JArray.Parse("[0,0,0]");
    public Vector3 dposV=new Vector3();
    private AgentInfo AgentInfo;
    void Start()
    {

    }
    private void Awake()
    {
        dposV = new Vector3((float)dpos[0], (float)dpos[1], (float)dpos[2]);
    }

    // Update is called once per frame
    void Update()
    {
        RibMove();
        //fired();
       // TurretTurn();

    }

    private void move()
    {
        //如果当前位置与目标点不一致
        if(transform.position!= dposV)
        {
            Vector3 nowpos_dpos = dposV - transform.position;
            float angle=Vector3.Angle(transform.forward, nowpos_dpos);
            if(angle >0)
            {
                action[1] = 1; //ad方向控制
            }
            else
            {
                action[1] = -1; //ad方向控制
            }
            
            if (Vector3.Angle(transform.forward, nowpos_dpos)<5)
            {
                action[0] =1;//油门与刹车
            }

        }
        
        Vector3 transSpeed = default;
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
