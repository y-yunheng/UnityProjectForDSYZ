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
    public bool RTSContorl = true;
    private long last_fire_time;
    private JArray action;
    private AgentInfo agentinfo;
    private Vector3 tar_position;
    private GameObject[] agents;
    private float train_num=1;
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
        if(!RTSContorl)
        {
            action = this.GetComponent<AgentInfo>().action;
            FPS_Control();
        }
        else
        {


          RTS_Control();


        }
        
    }

   void RTS_Control()
    {
        tar_position = new Vector3((float)agentinfo.dpos[0], (float)agentinfo.dpos[1], (float)agentinfo.dpos[2]);
        Debug.Log(tar_position);
        if (tar_position - transform.position != new Vector3(0, 0, 0))
        {
            moveto();
        }
        if (agentinfo.attack_id == -1)
        {
            Tutrret.transform.rotation = Quaternion.Lerp(Tutrret.transform.rotation, Quaternion.LookRotation(Body.transform.forward), turretSpeed * Time.deltaTime);
   
        }
        attack(agentinfo.attack_id);
    }
    void FPS_Control()
    {
        RibMove();
        TurretTurn();
        attack_fire();
    }

    void moveto()
    {
        tar_position.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tar_position - transform.position), turretSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, tar_position, Maxspeed * Time.deltaTime);
    }
    void attack(int id)
    {
        //本方法执行的自动攻击相关命令
        if (agentinfo.enemies_can_attack.Contains(id))
        {
            Debug.Log(id + "  " + "可攻击");
            int attack_id = id;
            GameObject attack_agent = agents[attack_id];
            Vector3 tar_attck_position = attack_agent.transform.position;
            Vector3 enemy_my_direction = tar_attck_position - Tutrret.transform.position;
            Tutrret.transform.rotation = Quaternion.Lerp(Tutrret.transform.rotation, Quaternion.LookRotation(enemy_my_direction), turretSpeed * Time.deltaTime);
            if (Vector3.Angle(Tutrret.transform.forward, enemy_my_direction) < 5)
            {
                fired();
            }
        }else
        {

        }
         
        
    }

    void attack_fire()
    {
        //本方法执行的自动攻击相关命令
      if((float)action[3]!=0)
        {
            fired();
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
                bull_rigidbody.velocity = bullposition.transform.forward * firedSpeed*train_num;
                last_fire_time = nowtime;

            }
    }
    private void RibMove()
    {
        float ab = (float)action[0]; //Input.GetAxis("Vertical1");//油门与刹车
        //WS方向控制
        float turn = (float)action[1];//Input.GetAxis("Horizontal1");
        //AD方向控制
        //

        //没有达到最大速度，继续加速
        if (this.GetComponent<Rigidbody>().velocity.sqrMagnitude < (Vector3.forward * Maxspeed).sqrMagnitude)
        {
            //计算前向油门的力
            float Force = ab * MaxForce;
            this.GetComponent<Rigidbody>().AddForce(transform.forward * Force*train_num);
            //计算旋转
            this.transform.Rotate(Vector3.up * turn * Time.deltaTime * rotateSpeed*train_num);
            /*      float a = 0.7f * turn;//转弯的角度
                  Vector3 nowforward = Quaternion.AngleAxis(a*57.3f, Vector3.up)* transform.forward;
                  Debug.Log(nowforward);
                  float NowFowardForce = Force * Mathf.Cos(a * 57.3f);
                  this.GetComponent<Rigidbody>().AddForce(nowforward * NowFowardForce);

                  Vector3 NowRight = Quaternion.AngleAxis(a * 57.3f, Vector3.up)*transform.right;
                  float NowRightForce = Force * Mathf.Sin(a * 57.3f);
                  this.GetComponent<Rigidbody>().AddForce(NowRight * NowRightForce);*/
        }




    }

    void TurretTurn()
    {
        float t = (float)action[2];
        Tutrret.transform.Rotate(Vector3.up * t, Time.deltaTime * turretSpeed*train_num);
    }
    long gettime()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        //得到精确到秒的时间戳（长度10位）
        long time = (long)mTimeSpan.TotalMilliseconds;
        return time;
    }



}
