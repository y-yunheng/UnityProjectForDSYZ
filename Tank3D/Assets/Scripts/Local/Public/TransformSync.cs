using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : MonoBehaviour
{
    private MemorySharePython mp;
    public GameObject[] Agents;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        mp = new MemorySharePython(4096);
        

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            for (int i = 0; i < Agents.Length; i++)
            {

                string msg = mp.ReadMemory(Agents[i].name);

                JObject jot = Readjson(msg, Agents[i].name);

                string postion = jot["Transfom"].ToString();


                JArray postionArray = JArray.Parse(postion);

                

                /*JArray rotationArray = JArray.Parse(postion);*/
                Agents[i].transform.position = new Vector3((float)postionArray[0], (float)postionArray[1], (float)postionArray[2]);
                //���������صĻ������ݰ�
                //string jsonstr = { "PacketName":"Observe","AgentId":0,"Observe:"[x,x,x,y,y,y,y]}
                /*string px = Agents[i].transform.position.x.ToString();
                string py = Agents[i].transform.position.y.ToString(); 
                string pz = Agents[i].transform.position.z.ToString(); 
                string rw = Agents[i].transform.rotation.w.ToString(); 
                string rx = Agents[i].transform.rotation.x.ToString(); 
                string ry = Agents[i].transform.rotation.y.ToString(); 
                string rz = Agents[i].transform.rotation.z.ToString();
                string obs = "[" + px + ", " + py + ", " + pz + ", " + rw + ", " + rx + ", " + ry + ", " + rz + "]";
                string jsonstr = "{ \"PacketName\":\"Observe\",\"AgentId\":" + Agents[i].name + ",\"Observe:\"+obs"*/;
                //mp.WriteMemory(jsonstr);
                //Debug.Log(postionArray);
                //all[i].transform.rotation.Set((float)rotationArray[0], (float)rotationArray[1], (float)rotationArray[2], (float)rotationArray[3]);
            }
        }catch(Exception e)
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
}
