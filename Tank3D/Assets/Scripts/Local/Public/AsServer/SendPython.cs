using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPython : MonoBehaviour
{
    FileShare fl;
    private MemorySharePython memory;
    public JObject Obs;
    private bool isinit = false;

    public GameObject[] Agents;
    private MemorySharePython sm;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        fl = new FileShare("C:\\Users\\yyh\\");
        memory = new MemorySharePython();
        Agents = GameObject.FindGameObjectsWithTag("Tank");
    }

    // Update is called once per frame
    void Update()
    {

        //发送状态{"PacketName":xxx ,0:{xx},1:{xxx},}
        
        //fl.CreateOrOPenFile(Obs.ToString(), "UnityObs");
        try
        {
            Obs = getobs();
            memory.WriteMemory(Obs.ToString(), "UnityObs");
        }catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }

    public JObject getobs()
    {
        JObject Obs = new JObject();
        for (int i = 0; i < Agents.Length; i++)
        {
            TankInfo agentinfo = Agents[i].GetComponent<TankInfo>();
            float px = Agents[i].transform.position.x;
            float py = Agents[i].transform.position.y;
            float pz = Agents[i].transform.position.z;
            JObject Jinfo = new JObject();
            Jinfo["group"] = agentinfo.group.ToString();
            Jinfo["id"] = agentinfo.id.ToString();
            Jinfo["pos"] = agentinfo.pos;
            Jinfo["turretRotation"] = agentinfo.turretRotation.ToString();
            Jinfo["heading"] = agentinfo.heading.ToString();
            Jinfo["hp"] = agentinfo.hp.ToString();
            Jinfo["bullet"] = agentinfo.bullet.ToString();
            Jinfo["motion_state"] = agentinfo.motion_state.ToString();
            Jinfo["alive"] = agentinfo.alive.ToString();
            Jinfo["attacked_id"] = agentinfo.attack_id.ToString();
            Jinfo["be_attacked_id"] = agentinfo.be_attacked_id.ToString();
            Jinfo["enemies_in_obs"] = agentinfo.enemies_in_obs;
            Jinfo["enemies_can_attack"] = agentinfo.enemies_can_attack;
            Jinfo["attacking"] = agentinfo.attacking.ToString();
            Jinfo["be_attacked"] = agentinfo.be_attacked.ToString();
            Jinfo["action"] = "["+agentinfo.action[0]+"," + agentinfo.action[1] + "," + agentinfo.action[2] + "," + agentinfo.action[3] + "]";
            if(i==1)
            {
                Debug.Log(Jinfo["action"]);
            }
            Obs.Add(i.ToString(), Jinfo);

            
        }
       
            return Obs;
    }
}
