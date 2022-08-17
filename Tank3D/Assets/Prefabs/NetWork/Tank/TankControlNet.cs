using Mirror;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControlNet : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject Tutrret;
    public GameObject Body;
    public GameObject bull;
    public GameObject bullposition;
    public float firedSpeed;
    public long TimeInterval=1000;//炮弹装填时间


    public float Maxspeed = 5;
    public float turretSpeed = 10;
    public float MaxForce = 20000;//设置车辆最大牵引力
    public float rotateSpeed = 5;
    public long last_fire_time;
    private JArray action;
    private TankInfoNet tankinfo;
    void Start()
    {
        //  前后，左右，开火
        
    }
    private void Awake()
    {
        tankinfo = this.GetComponent<TankInfoNet>();
    }
    // Update is called once per frame
    void Update()
    {   
        action = this.GetComponent<TankInfoNet>().action;
        RibMove();
        fired();
        TurretTurn();
    }

    private void move()
    {
        float ws = (float)action[0];                              //Input.GetAxis("Vertical1");//油门与刹车
        //WS方向控制
        float turn = (float)action[1];
        //AD方向控制    
        this.transform.Translate(Vector3.forward * ws * Time.deltaTime * Maxspeed);
        rotateSpeed = Mathf.Abs(ws * Maxspeed);
        this.transform.Rotate(Vector3.up * turn * Time.deltaTime * rotateSpeed);
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
            this.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
            //计算旋转
            this.transform.Rotate(Vector3.up * turn * Time.deltaTime * rotateSpeed);
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



    private void fired()
    {
        if ((int)action[3] != 0 ||tankinfo.attacking==1)
        {
            long nowtime = gettime();
            if (nowtime - last_fire_time > TimeInterval)
            {
                tankinfo.bullet--;
                GameObject insbull = Instantiate(bull, bullposition.transform.position, bullposition.transform.rotation);
                Rigidbody bull_rigidbody = insbull.GetComponent<Rigidbody>();
                BullDestoryNet bull_destory = insbull.GetComponent<BullDestoryNet>();
                bull_destory.id = tankinfo.id;
                bull_rigidbody.velocity = bullposition.transform.forward * firedSpeed;
                last_fire_time = nowtime;

            }
        }

    }

    void TurretTurn()
    {
        float t = (float)action[2];
        Tutrret.transform.Rotate(Vector3.up * t, Time.deltaTime * turretSpeed);
    }

    long gettime()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        //得到精确到秒的时间戳（长度10位）
        long time = (long)mTimeSpan.TotalMilliseconds;
        return time;
    }



}
