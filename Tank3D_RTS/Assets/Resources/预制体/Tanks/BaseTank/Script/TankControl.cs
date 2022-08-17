using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Tutrret;
    public GameObject Body;
    public GameObject bull;
    public GameObject bullposition;
    public float firedSpeed;
    public long TimeInterval;//炮弹装填时间


    public float Maxspeed = 5;
    public float turretSpeed = 10;
    public float MaxForce = 20000;//设置车辆最大牵引力
    public float rotateSpeed = 5;
    private long last_fire_time;
    private JArray action;
    private AgentInfo agentinfo;
    private Vector3 tar_position;
    private GameObject[] agents;
    void Start()
    {
        //  前后，左右，开火
        tar_position = new Vector3((float)agentinfo.dpos[0], (float)agentinfo.dpos[1], (float)agentinfo.dpos[2]);
    }
    private void Awake()
    {
        agentinfo=this.GetComponent<AgentInfo>();
        GameObject[] Agents = GameObject.FindGameObjectsWithTag("Tank");
        
        agents=new GameObject[Agents.Length];
        foreach (GameObject Agent in Agents)
        {
            int id = Agent.GetComponent<AgentInfo>().id;
            agents[id]=Agent;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!agentinfo.is_rts)
        {
            action = this.GetComponent<AgentInfo>().action;
        }else
        {
            tar_position=new Vector3((float)agentinfo.dpos[0], (float)agentinfo.dpos[1], (float)agentinfo.dpos[2]);
            if(tar_position-transform.position!=new Vector3(0,0,0))
            {
                moveto();
            }
            if(agentinfo.attack_id==-1)
            {
                Tutrret.transform.forward=Body.transform.forward;
            }
            attack(agentinfo.attack_id);

            
           
        }
        
    }

   
    void moveto()
    {   
        tar_position.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tar_position - transform.position), 5 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, tar_position, 5 * Time.deltaTime);
    }
    void attack(int id)
    {
        //本方法执行的自动攻击相关命令
        Debug.Log(agentinfo.enemies_can_attack.Count);
        if (agentinfo.enemies_can_attack.Contains(id))
        {
            Debug.Log(id + "  " + "可攻击");
            int attack_id = id;
            GameObject attack_agent = agents[attack_id];
            Vector3 tar_attck_position = attack_agent.transform.position;
            Vector3 enemy_my_direction = tar_attck_position - Tutrret.transform.position;
            Tutrret.transform.rotation = Quaternion.Lerp(Tutrret.transform.rotation, Quaternion.LookRotation(enemy_my_direction), 5 * Time.deltaTime);
            if (Vector3.Angle(Tutrret.transform.forward, enemy_my_direction) < 5)
            {
                fired();
            }
        }else
        {

        }
         
        
    }
    private void fired()
    {     
            long nowtime = gettime();
            if (nowtime - last_fire_time > TimeInterval)
            {
                agentinfo.bullet--;
                GameObject insbull = Instantiate(bull, bullposition.transform.position, bullposition.transform.rotation);
                Rigidbody bull_rigidbody = insbull.GetComponent<Rigidbody>();
                BullDestory bull_destory = insbull.GetComponent<BullDestory>();
                bull_destory.id = agentinfo.id;
                bull_rigidbody.velocity = bullposition.transform.forward * firedSpeed;
                last_fire_time = nowtime;

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
