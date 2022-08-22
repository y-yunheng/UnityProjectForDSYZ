using Mirror;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInfoNet : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject turret;
    public GameObject cam;
    
    public int group;//组号
    public int id;//智能体id编号，从0开始
    public JArray pos = new JArray();//坐标
    public float turretRotation=0 ;//炮塔旋转度（弧度）
    public float heading=0;//航向(弧度)，水平向右为0，逆时针旋转
    public float hp=100;//智能体剩余血量
    public int bullet=100;//智能体剩余子弹
    public int motion_state=0;//智能体运动状态，锁定-1、停止0、移动1
    public int alive=1;//0:false,1:true
    public int attack_id=-1;//智能体攻击目标（敌方智能体id编号）
    public int be_attacked_id=-1;//智能体被目标攻击（敌方智能体id编号）
    public JArray  enemies_in_obs=new JArray();//智能体能看见的敌方智能体（敌方智能体id编号）
    public JArray enemies_can_attack = new JArray();//智能体能攻击到的敌方智能体（敌方智能体id编号）
    public int attacking=0;//智能体是否正在攻击（True or False）
    public int be_attacked=0;//智能体正在被攻击
    [SyncVar] public float test = 0;         
    public JArray action = new JArray();
    public Light canobs;
    public Light canattack;

    private GameObject[] Agents;

    [SyncVar] public string obsstring; 
    void Start()
    {
        action = JArray.Parse("[0,0,0,0]");
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        obsstring = get_need_syncvar();


    }
    // Update is called once per frame
    void Update()
    {
        if (isClient)
        {
            if (!isLocalPlayer)
            {
                set_need_syncvar(obsstring);
                change_gameobject();
            }
            else
            {
                set_pos_rot();
                set_obs_value();
                other_value();
                change_gameobject();
                string news = get_need_syncvar();
                CmdInfo(news);//本函数仅仅在服务端运行 


            }
        }
        
        

  

        
        

        

        
    }
    private void set_pos_rot()
    {
        heading = this.transform.rotation.y;
        turretRotation = turret.transform.rotation.y;
        float rx = transform.position.x;
        float ry = transform.position.y;
        float rz = transform.position.z;
        pos = JArray.Parse("[" + rx + "," + ry + "," + rz + "," + "]");
    }

    private void set_obs_value()
    {
        foreach (var agent in Agents)
        {
            TankInfoNet agentinfo = agent.GetComponent<TankInfoNet>();
            if (agentinfo.group != this.group)
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
    }
    private void other_value()
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
    private void change_gameobject()
    {
        if ((int)alive == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private string get_need_syncvar()
    {
        JObject Jinfo = new JObject();
        Jinfo.Add("group", group.ToString());
        Jinfo.Add("id", id.ToString());
        Jinfo.Add("pos", pos);
        Jinfo.Add("turretRotation", turretRotation.ToString());
        Jinfo.Add("heading", heading.ToString());
        Jinfo.Add("hp", hp.ToString());
        Jinfo.Add("bullet", bullet.ToString());
        Jinfo.Add("motion_state",motion_state.ToString());
        Jinfo.Add("alive", alive.ToString());
        Jinfo.Add("attacked_id", attack_id.ToString());
        Jinfo.Add("be_attacked_id", be_attacked_id.ToString());
        Jinfo.Add("enemies_in_obs", enemies_in_obs);
        Jinfo.Add("enemies_can_attack", enemies_can_attack);
        Jinfo.Add("attacking", attacking.ToString());
        Jinfo.Add("be_attacked",be_attacked.ToString());
        return Jinfo.ToString();
    }
    private void set_need_syncvar(string osbstr)
    {
        JObject Jinfo=JObject.Parse(osbstr);
        group = (int)Jinfo["group"];
        id = (int)Jinfo["id"];
        pos = (JArray)Jinfo["pos"];
        turretRotation= (float)Jinfo["turretRotation"];
        heading = (float)Jinfo["heading"];
        hp = (float)Jinfo["hp"];
        bullet = (int)Jinfo["bullet"];
        motion_state = (int)Jinfo["motion_state"];
        alive = (int)Jinfo["alive"];
        attack_id = (int)Jinfo["attacked_id"];
        be_attacked_id = (int)Jinfo["be_attacked_id"];
        enemies_in_obs = (JArray)Jinfo["enemies_in_obs"];
        enemies_can_attack = (JArray)Jinfo["enemies_can_attack"];
        attacking = (int)Jinfo["attacking"];
        be_attacked = (int)Jinfo["be_attacked"];
    }
    [Command]
    public void CmdInfo(string obs)
    {
        // 将从状态字符串同步到其它变量
        obsstring = obs;
        set_need_syncvar(obsstring);
        if((float)action[3]!=0)
        {
            test = (float)action[3];
        }
    }
}
