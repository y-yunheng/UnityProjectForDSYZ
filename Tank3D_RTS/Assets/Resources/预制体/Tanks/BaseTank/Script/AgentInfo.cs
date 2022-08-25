using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject turret;
    public GameObject cam;
    public Light canobs;
    public Light canattack;
    public Light radar;
    public bool is_select=false;//物体被选中
    public bool is_rts=false;

    /*这是坦克的状态数据*/
    public int group=0;//组号
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
    public ArrayList  enemies_in_obs=new ArrayList();//智能体能看见的敌方智能体（敌方智能体id编号）,以及能探测到的智能体。
    public ArrayList enemies_can_attack = new ArrayList();//智能体能攻击到的敌方智能体（敌方智能体id编号）
    public int attacking=0;//智能体是否正在攻击（True or False）
    public int be_attacked=0;//智能体正在被攻击（True or False）
    public int is_auto_attack=0;//智能体是否自动攻击（True or False）


    /*这是坦克动作数据*/
    public JArray dpos= JArray.Parse("[0,0,0]");//坦克要去往的地点
    //public JArray attck_dpos = JArray.Parse("[0,0,0]");//坦克要去往的攻击的地点 如果有则会执行攻击
    public float dturretRotation;//坦克炮塔需要旋转度（弧度）
    public float dheading;//坦克将要转向(弧度)，水平向右为0，逆时针旋转
    public bool is_reach_dpos=false;//坦克是否到达的目的地
    
    public JArray action = JArray.Parse("[0,0,0,0]");//在FPS模式下操作坦克的动作空间
    private GameObject[] Agents;
    void Start()
    {
       
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        dpos[0] = transform.position.x;
        dpos[1] = transform.position.y;
        dpos[2] = transform.position.z;
       

    }
    // Update is called once per frame
    void Update()
    {

        set_pos_rot();
        set_is_reach_dpos();
        set_attacking();
        set_canobs();
        set_attack_id();




    }

    void set_pos_rot()
    {
        heading = this.transform.rotation.y;
        turretRotation = turret.transform.rotation.y;
        float rx = transform.position.x;
        float ry = transform.position.y;
        float rz = transform.position.z;
        pos = JArray.Parse("[" + rx + "," + ry + "," + rz + "," + "]");
    }
    void set_is_reach_dpos()
    {
        if(dpos==pos)
        {
           is_reach_dpos=true;
        }else
        {
            is_reach_dpos = false;
        }
    }
    void set_attacking()
    {
        if ((int)action[3] != 0)
        {
            attacking = 1;
        }
        else
        {
            attacking = 0;
        }
    }
    void set_canobs()
    {
        foreach (GameObject agent in Agents)
        {
            AgentInfo agentinfo = agent.GetComponent<AgentInfo>();         
            if (agentinfo.group != group)
            {
                Vector3 distance =transform.position - agent.transform.position;
                if (canobs.range> distance.magnitude)
                {
                    if (!enemies_in_obs.Contains(agentinfo.id))
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




                if (canattack.range >distance.magnitude)
                {
                    if (!enemies_can_attack.Contains(agentinfo.id))
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
    void set_attack_id()
    {
        if (is_auto_attack != 0 && enemies_can_attack.Count > 0)
        {
            foreach (var id in enemies_can_attack)
            {
                int agentId = (int)id;
                attack_id = agentId;

            }

        }
    }
    //本函数用于测试
    private void OnCollisionEnter(Collision collision)
    {

    }

}
