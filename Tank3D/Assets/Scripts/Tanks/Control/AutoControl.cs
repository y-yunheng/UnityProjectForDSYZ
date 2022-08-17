using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoControl : MonoBehaviour
{

    public GameObject bull;
    public GameObject bullposition;
    public float movespeed = 1.0f;
    public float rotatespeed=1.0f;
    public float bullspeed=500.0f;
    public bool isfire = false;
    public TankInfo tankinfo;
    public float bodyRotetespeed;
    private long TimeInterval = 300;
    private long last_fire_time = 0;

    private TankInfo tankInfo;
    



    // Start is called before the first frame update

    void Start()
    {
        tankInfo = GetComponent<TankInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        float ws = (float)tankInfo.action[0];
        float ad = (float)tankInfo.action[1];
        float fire = (float)tankInfo.action[2];
        this.transform.Rotate(Vector3.up * -1, ad*Time.deltaTime * rotatespeed);
        this.transform.Translate(Vector3.forward * ws * Time.deltaTime * movespeed);
        //move(ws, ad);
        if(fire > 0)
        {
            fired();
        }
    }

    private void move(float ws,float ad)
    {

        this.transform.Rotate(Vector3.up * -1, Time.deltaTime * rotatespeed);
        this.transform.Translate(Vector3.forward * ws * Time.deltaTime * movespeed);
    }


    void fired()
    {
        long nowtime = gettime();
        if (nowtime - last_fire_time > TimeInterval)
        {
            GameObject insbull = Instantiate(bull, bullposition.transform.position, bullposition.transform.rotation);
            Rigidbody bull_rigidbody = insbull.GetComponent<Rigidbody>();
            BullDestory bull_destory = insbull.GetComponent<BullDestory>();
            bull_destory.id = this.GetComponent<TankInfo>().id;
            bull_rigidbody.velocity = bullposition.transform.forward * bullspeed;
            last_fire_time = gettime();
            tankInfo.bullet--;
            //Thread.Sleep(500);


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
