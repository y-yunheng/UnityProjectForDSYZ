using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BullDestoryNet :NetworkBehaviour
{
    // Start is called before the first frame update
    public int id;
    public GameObject particle;
    private long starttime = 0;
    void Start()
    {
        starttime=gettime();
    }

    // Update is called once per frame
    void Update()
    {
        if(gettime()-starttime>5000)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        particle.GetComponent<ParticleSystem>().Play();
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag=="Tank")
        {
            
            GameObject agent=collision.gameObject;
            TankInfoNet agentinfo= agent.GetComponent<TankInfoNet>();
            agentinfo.hp-=1;
            agentinfo.be_attacked = 1;
            if (agentinfo.hp>0)
            {
                agentinfo.alive = 1;
            }else
            {
                agentinfo.alive = 0;
            }
            


            
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
