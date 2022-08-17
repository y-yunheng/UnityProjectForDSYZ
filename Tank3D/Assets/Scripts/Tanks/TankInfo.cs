using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject turret;
    public GameObject cam;
    public int group;//组号
    public int id;//智能体id编号，从0开始
    public JArray pos = new JArray();//坐标
    public float turretRotation ;//炮塔旋转度（弧度）
    public float heading;//航向(弧度)，水平向右为0，逆时针旋转
    public float hp;//智能体剩余血量
    public int bullet;//智能体剩余子弹
    public int motion_state=0;//智能体运动状态，锁定-1、停止0、移动1
    public int alive=1;//0:false,1:true
    public int attack_id=-1;//智能体攻击目标（敌方智能体id编号）
    public int be_attacked_id=-1;//智能体被目标攻击（敌方智能体id编号）
    public JArray  enemies_in_obs=new JArray();//智能体能看见的敌方智能体（敌方智能体id编号）
    public JArray enemies_can_attack = new JArray();//智能体能攻击到的敌方智能体（敌方智能体id编号）
    public int attacking=0;//智能体是否正在攻击（True or False）
    public int be_attacked=0;//智能体正在被攻击

    public JArray action;
    public Light canobs;
    public Light canattack;

    private GameObject[] Agents;
    private GameObject[] TeamAgents;
    void Start()
    {
       
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");

        action = JArray.Parse("[0,0,0,0]");

    }
    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        heading = this.transform.rotation.y;
        turretRotation = turret.transform.rotation.y;
        float rx= transform.position.x;
        float ry = transform.position.y;
        float rz = transform.position.z;
        pos = JArray.Parse("[" + rx + "," + ry + "," + rz + "," + "]");

        foreach (var agent in Agents)
        {
            TankInfo agentinfo=agent.GetComponent<TankInfo>();
            if(agentinfo.group!=this.group)
            {
                Vector3 distance = this.transform.position - agent.transform.position;
                if (Math.Sqrt(canobs.range) < distance.sqrMagnitude)
                {
                    if (Vector3.Angle(this.transform.forward, distance) < canobs.spotAngle / 2)
                    {
                        if (enemies_in_obs.Contains(agentinfo.id))
                        {
                            enemies_in_obs.Add(agentinfo.id);
                        }
                    }
                    else
                    {
                        if (enemies_in_obs.Contains(agentinfo.id))
                        {
                            enemies_in_obs.Remove(agentinfo.id);
                        }

                    }



                }
                if (Math.Sqrt(canattack.range) < distance.sqrMagnitude)
                {
                    if (Vector3.Angle(this.transform.forward, distance) < canattack.spotAngle / 2)
                    {
                        if (enemies_can_attack.Contains(agentinfo.id))
                        {
                            enemies_can_attack.Add(agentinfo.id);
                        }
                    }
                    else
                    {
                        if (enemies_can_attack.Contains(agentinfo.id))
                        {
                            enemies_can_attack.Remove(agentinfo.id);
                        }

                    }
                }
            }
            
        }

        if((int)action[3]!=0)
        {
            attacking = 1;
        }
        else
        {
            attacking = 0;
        }
     
        
    }
}
