using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsClient : MonoBehaviour
{
    // Start is called before the first frame update
    private UDPclient Udpclient;
    public Image plane;
    public TMP_InputField ipField;
    public TMP_InputField portField;
    public TMP_InputField idField;
    public Button button;
    private string ip;
    private int port;
    private int id;
    private GameObject[] Agents;

    void Start()
    {
        Udpclient = new UDPclient(10000);
    }
    private void Awake()
    {
        button.onClick.AddListener(startAction);

    }
    void startAction()
    {
        Debug.Log("anniu");
        ip = ipField.text;
        port = int.Parse(portField.text);
        id = int.Parse(idField.text);
        Agents = GameObject.FindGameObjectsWithTag("Tank");
        Agents[id].GetComponentInChildren<Camera>().enabled = true;
        Agents[id].GetComponent<Playercontrol>().enabled = true;
        Udpclient.ConnectStart(ip, id);
        plane.gameObject.SetActive(false);        
    }
    // Update is called once per frame
    void Update()
    {
        if (!plane.gameObject.activeSelf)
        {
            JObject action = new JObject();
            action.Add("PacketName", "AgentInputPacket");
            action.Add("AgentId", id.ToString());
            action.Add("Action", Agents[id].GetComponent<TankInfo>().action.ToString());
            Udpclient.SendQueue.Enqueue(action.ToString());
            if(Udpclient.RecviceQueue.Count>0)
            {
                JObject result = JObject.Parse(Udpclient.RecviceQueue.Dequeue());
                Debug.Log(result);
                JObject actions = (JObject)result["Actions"];
                for (int i = 0; i < actions.Count; i++)
                {
                    Agents[id].GetComponent<TankInfo>().action = (JArray)actions[i.ToString()];
                }
            }
           
           

            

        }
        
    }
    public void SetObs(JObject obs)
    {
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
            Obs.Add(i.ToString(), Jinfo);

        }
        return Obs;
    }
}
