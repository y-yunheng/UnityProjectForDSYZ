using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPython : MonoBehaviour
{
    // Start is called before the first frame update
    FileShare fl;
    public GameObject[] tanks;
    public JObject Obs;
    private bool isinit=false;

    public GameObject[] Agents;
    private MemorySharePython sm;

    void Start()
    {
        fl = new FileShare("C:\\ShareFile");
    }
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        
        try
        {
            //接受动作
            string msg = fl.ReadFile("AgentAction");
            JObject jo = JObject.Parse(msg);
            if (jo["PacketName"].ToString().Equals("AgentInputPacket"))
            {

                for (int i = 0; i < Agents.Length; i++)
                {
                    TankInfo agentinfo = Agents[i].GetComponent<TankInfo>();
                    JArray action = (JArray)jo[i.ToString()];                     
                    agentinfo.action = action;
                }
            }

        }
        catch(Exception e)
        {
            //Debug.Log(e.ToString());
        }


        //发送状态{"PacketName":xxx ,0:{xx},1:{xxx},}
        for (int i = 0; i < Agents.Length; i++)
        {
            try
            {
                TankInfo agentinfo = Agents[i].GetComponent<TankInfo>();                    
                JObject Jinfo = (JObject)Obs[i.ToString()];
                Jinfo.Add("group", agentinfo.group.ToString());
                Jinfo.Add("id", agentinfo.id.ToString());
                Jinfo.Add("pos", agentinfo.pos.ToString());
                Jinfo.Add("turretRotation",agentinfo.turretRotation.ToString());
                Jinfo.Add("heading",agentinfo.heading.ToString());
                Jinfo.Add("hp", agentinfo.hp.ToString());
                Jinfo.Add("bullet", agentinfo.bullet.ToString());
                Jinfo.Add("motion_state", agentinfo.motion_state.ToString());
                Jinfo.Add("alive", agentinfo.alive.ToString());
                Jinfo.Add("attacked_id", agentinfo.attack_id.ToString());
                Jinfo.Add("be_attacked_id", agentinfo.be_attacked_id.ToString());
                Jinfo.Add("enemies_in_obs", agentinfo.enemies_in_obs.ToString());
                Jinfo.Add("enemies_can_attack",agentinfo.enemies_can_attack.ToString());
                Jinfo.Add("attacking", agentinfo.attacking.ToString());
                Jinfo.Add("be_attacked", agentinfo.be_attacked.ToString());

            }
            catch(Exception e)
            {
                //表示该智能体没有存活
                Debug.LogError(e.Message);
            }
            
        }
        fl.CreateOrOPenFile(Obs.ToString(),"UnityObs");   
 
    }



    

   
}
