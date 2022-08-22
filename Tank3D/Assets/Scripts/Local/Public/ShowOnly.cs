using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnly : MonoBehaviour
{
    private FileShare mp;
    public GameObject[] Agents;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        mp = new FileShare("C:\\Users\\yyh\\");


    }

    // Update is called once per frame
    void Update()
    {
        try
        {

            string msg = mp.ReadFile("UnityObs");
           // Debug.Log(msg);
            for (int i = 0; i < Agents.Length; i++)
            {
                JObject jot = Readjson(msg, "Observations");
                JArray postionArray = (JArray)jot[i.ToString()]["selfInfo"]["pos"];
                //Debug.Log(i);
                //Debug.Log(postionArray);
                //string velocity = jot["Velocity"].ToString();
                //JArray velocityArray = JArray.Parse(velocity);

               // int speed  = int.Parse(jot["Speed"].ToString());

               // Debug.Log(speed);
                /*JArray rotationArray = JArray.Parse(postion);*/
                Agents[i].transform.position = new Vector3((float)postionArray[0], 0, (float)postionArray[1]);
                //���������صĻ������ݰ�
                float heading = (float)jot[i.ToString()]["selfInfo"]["heading"];
                float headding_du= 360 / (heading * 2);
                Debug.Log(heading);
                Agents[i].transform.rotation.Set(Agents[i].transform.rotation.x, Agents[i].transform.rotation.y+ headding_du, Agents[i].transform.rotation.x, Agents[i].transform.rotation.w);
                //all[i].transform.rotation.Set((float)rotationArray[0], (float)rotationArray[1], (float)rotationArray[2], (float)rotationArray[3]);
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);

        }
    }

    private JObject Readjson(string JsonString,string ObjName)
    {
        //json {"obj1":{"postion":[x,x,x],"rotation":[y,y,y]},"obj2":{"postion":[x,x,x],"rotation":[y,y,y,y]},"obj3":{"postion":[x,x,x],"rotation":[y,y,y]}}
        string jsontext = JsonString;
        JObject jo = JObject.Parse(jsontext);
        string ObjTransform = jo[ObjName].ToString();
        JObject jot = JObject.Parse(ObjTransform);
        return jot;

    }
    private JObject Readjson(string JsonString, int ObjName)
    {
        //json {"obj1":{"postion":[x,x,x],"rotation":[y,y,y]},"obj2":{"postion":[x,x,x],"rotation":[y,y,y,y]},"obj3":{"postion":[x,x,x],"rotation":[y,y,y]}}
        string jsontext = JsonString;
        JObject jo = JObject.Parse(jsontext);
        string ObjTransform = jo[ObjName].ToString();
        JObject jot = JObject.Parse(ObjTransform);
        return jot;

    }





}
