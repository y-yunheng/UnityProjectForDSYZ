using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SyncObsClient : MonoBehaviour
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
            if (Udpclient.RecviceQueue.Count > 0)
            {
                JObject result = JObject.Parse(Udpclient.RecviceQueue.Dequeue());
                Debug.Log(result);
                JObject allobs = (JObject)result["observations"];
                SetObs(allobs);
            }





        }

    }
    public void SetObs(JObject obs)
    {
        for (int i = 0; i < obs.Count; i++)
        {
            int id = (int)obs[i.ToString()]["id"];
            TankInfo agentinfo = Agents[id].GetComponent<TankInfo>();
            JObject Jinfo = (JObject)obs[i.ToString()];
            agentinfo.group = (int)Jinfo["group"];
            agentinfo.id = (int)Jinfo["id"];
            agentinfo.pos = (JArray)Jinfo["pos"];
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
        
    }
}
