using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsClientPython : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tanks;
    public FileShare share;
    private GameObject[] Agents;

    void Start()
    {
        share = new FileShare("C:\\Users\\yyh\\");
    }
    private void Awake()
    {
        string msg = share.ReadFile("UnityObs");
        JObject jo = (JObject)msg;
        JObject obs = (JObject)jo["observations"];
        foreach (JProperty p in obs.Properties())
        {
            int groupid = (int)obs[p]["group"];
            JArray pos = (JArray)obs[p]["pos"];
            Vector3 position = new Vector3((int)pos[0], (int)pos[1], (int)pos[2]);
            float heading = (float)obs[p]["heading"];
            Quaternion rotation = Quaternion.Euler(0, heading, 0);
            Instantiate(tanks[groupid], position, rotation);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        string msg = share.ReadFile("UnityObs");
        JObject jo = (JObject)msg;
        JObject obs = (JObject)jo["observations"];
        foreach (JProperty p in obs.Properties())
        {
            try
            {
                int id = (int)obs[p]["id"];
                TankInfo agentinfo = Agents[id].GetComponent<TankInfo>();
                JObject Jinfo = (JObject)obs[p];
                agentinfo.group = (int)Jinfo["group"];
                agentinfo.id = (int)Jinfo["id"];
                agentinfo.pos = (JArray)obs[p]["pos"];
                agentinfo.heading = (float)Jinfo["heading"];
                agentinfo.hp = (float)Jinfo["hp"];
                agentinfo.bullet = (int)Jinfo["bullet"];
                agentinfo.motion_state = (int)Jinfo["motion_state"];
                agentinfo.alive = (int)Jinfo["alive"];
                agentinfo.attack_id = (int)Jinfo["attacked_id"];
                agentinfo.be_attacked_id = (int)Jinfo["be_attacked_id"];
                agentinfo.enemies_in_obs = (JArray)Jinfo["enemies_in_obs"];
                agentinfo.enemies_can_attack = (JArray)Jinfo["enemies_can_attack"];
                agentinfo.attacking = (int)Jinfo["attacking"];
                agentinfo.be_attacked = (int)Jinfo["be_attacked"];
            }
            catch (Exception a)
            {
                Debug.Log(a.Message);
            }



        }


    }
}
