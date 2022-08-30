using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PythonControl : MonoBehaviour
{
    // Start is called before the first frame update
    MemorySharePython mp = new MemorySharePython();
    private GameObject[] Agents;
    private bool reseted=true;
    void Start()
    {
        
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Agents");
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(reseted)
        // {
        //     string sendmsg = getobs();
        //     mp.WriteMemory(sendmsg, "Cshape");
        //     reseted = false;
        // }
        string sendmsg2 = getobs();
        mp.WriteMemory(sendmsg2, "Unity");
        string msg = mp.ReadMemory("Python");
        //Debug.Log(msg);
        JObject msgobj = JObject.Parse(msg);
        string packetname = (string)msgobj["PacketName"];
        if (packetname.Equals("Reset"))
        {   int isreset= (int)msgobj["IsReset"];
            Debug.Log(isreset);
            if(isreset==1)
            {
               reseted=true;
               string SceneName = SceneManager.GetActiveScene().name;
               SceneManager.LoadScene(SceneName);
            }
            
        }else if(packetname.Equals("AgentInput"))
        {
            JArray actions = (JArray)msgobj["Actions"];
            foreach (GameObject agent in Agents)
            {
                AgentInfo agentInfo = agent.GetComponent<AgentInfo>();
                object info;
                switch (agentInfo.agent_type)
                {
                    case 0: info= agent.GetComponent<TankInfo>(); break;
                    case 1: info = agent.GetComponent<FlyInfo>(); break;
                }

                agentInfo.action = (JArray)actions[agentInfo.id];  
            } 
        }
        
    }

    string getobs()
    {
        JObject allobs=new JObject();
        JArray obsjarray=new JArray();
        JArray rewards = new JArray();
        int alive0 = 0;
        int alive1 = 0;
        int result = 0;
        foreach (GameObject agent in Agents)
        {
            JArray obs = new JArray();
            AgentInfo agentInfo = agent.GetComponent<AgentInfo>();
            obs.Add(agentInfo.id);
            obs.Add(agentInfo.group);
            obs.Add(agentInfo.pos[0]);
            obs.Add(agentInfo.pos[1]);
            obs.Add(agentInfo.pos[2]);
            obs.Add(agentInfo.alive);

            obsjarray.Add(obs);
            if (agentInfo.alive==1 && agentInfo.group==0)
            {
                alive0++;
            }else if(agentInfo.alive==1 && agentInfo.group==1)
            {
                alive1++;
            }
        }
        allobs.Add("obs",obsjarray);
        allobs.Add("rewards", obsjarray);
        if(alive0==0 && alive1!=0)
        {
            result =1;
        }else if(alive1==0 && alive0!=0)
        {
            result = 0;
        }else if (alive1==0 && alive0==0)
        {
            result = -2;
        }
        else
        {
            result = -1;
        }
        allobs.Add("result", result);
        return allobs.ToString();
    }

    void reset()
    {
        foreach(GameObject agent in Agents)
        {
            AgentInfo ageninfo=agent.GetComponent<AgentInfo>();
        }
    

    }
}
