using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletsFiredMy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bull;
    public GameObject bullposition;
    public float movespeed = 500.0f;
    public bool isfire=false;
    public TankInfo tankinfo;
    private long TimeInterval = 300;
    private long last_fire_time = 0;


    void Start()
    {
        
    }
    private void Awake()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //手动开炮
        if (Input.GetKey(KeyCode.Space))
        {
            fired();
        }
    }

    long gettime()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        //得到精确到秒的时间戳（长度10位）
        long time = (long)mTimeSpan.TotalMilliseconds;
        return time;
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
            bull_rigidbody.velocity = bullposition.transform.forward * movespeed;
            last_fire_time = gettime();
            tankinfo.bullet--;
            //Thread.Sleep(500);


        }
    }
}
