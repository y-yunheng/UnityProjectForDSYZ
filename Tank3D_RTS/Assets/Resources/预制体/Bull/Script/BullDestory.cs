using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BullDestory : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public  ParticleSystem[] particles;
    private long starttime = 0;
    private GameObject[] Agents;
    private AgentInfo myagentInfo;
    void Start()
    {
        starttime=gettime();
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        foreach (GameObject agent in Agents)
        {
            if(agent.GetComponent<AgentInfo>().id ==id)
            {
                myagentInfo = agent.GetComponent<AgentInfo>();
            }
        }

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
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
        
        Debug.Log(collision.gameObject.name);
        //如果打到了别人坦克
        if (collision.gameObject.tag=="Tank")
        {
            
            GameObject agent=collision.gameObject;
            AgentInfo agentinfo= agent.GetComponent<AgentInfo>();
            agentinfo.hp=agentinfo.hp-1;
            agentinfo.be_attacked = 1;
            agentinfo.reward=-1;
            if (agentinfo.hp>0)
            {
                agentinfo.alive = 1;
            }else
            {
                agentinfo.alive = 0;
                collision.gameObject.SetActive(false);
                myagentInfo.reward+=10;
            }
            myagentInfo.reward=1;
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
