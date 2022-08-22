using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObviousSend : MonoBehaviour
{

    public GameObject[] Agents;
    private MemorySharePython mp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        mp = new MemorySharePython( 4096);
    }
    // Update is called once per frame
    void Update()
    { 
        JObject obs=new JObject();
        obs=JsonAdd(obs, "PacketName", "UnityObs");
        for(int i=0;i<Agents.Length;i++)
        {
            
            JObject AgentObs = new JObject();
            string px = Agents[i].transform.position.x.ToString();
            string py = Agents[i].transform.position.y.ToString();
            string pz = Agents[i].transform.position.z.ToString();
            string rw = Agents[i].transform.rotation.w.ToString();
            string rx = Agents[i].transform.rotation.x.ToString();
            string ry = Agents[i].transform.rotation.y.ToString();
            string rz = Agents[i].transform.rotation.z.ToString();
            string trans = "[" + px + ", " + py + ", " + pz + ", " + rw + ", " + rx + ", " + ry + ", " + rz + "]";
            AgentObs=JsonAdd(AgentObs, "Transfom", trans);
            bool isend=Agents[i].GetComponent<AgentsControl>().isend;
            AgentObs= JsonAdd(AgentObs, "isend", isend);
            //Debug.Log(isend);
            obs =JsonAdd(obs, Agents[i].name, AgentObs);
        }
       
        mp.WriteMemory(obs.ToString(),"Unity");
        


    }
  
    public static JObject JsonAdd(object obj, string key, object value)
    {
        JObject jObj = JObject.Parse(JsonConvert.SerializeObject(obj));
        jObj.Add(new JProperty(key, value));
        return jObj;
    }

}
