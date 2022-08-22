using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SyncFrameClient : MonoBehaviour
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
    public GameObject[] Agents;

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
        ip = ipField.text;
        port = int.Parse(portField.text);
        id = int.Parse(idField.text);
        Agents[id].GetComponentInChildren<Camera>().enabled = true;
        Agents[id].GetComponent<Playercontrol>().enabled = true;
        Udpclient.ConnectStart(ip, port);
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
            action.Add("Action", Agents[id].GetComponent<Playercontrol>().action);
            if(Udpclient.SendQueue.Count>5)
            {
                Udpclient.SendQueue.Dequeue();
            }
            Udpclient.SendQueue.Enqueue(action.ToString());
            
            if (Udpclient.RecviceQueue.Count > 0)
            {
                JObject result = JObject.Parse(Udpclient.RecviceQueue.Dequeue());

                JArray actions = JArray.Parse( result["Actions"].ToString());
                Debug.Log(actions.Count);
                for (int i = 0; i < actions.Count; i++)
                {
                    TankInfo AgentInfo = Agents[i].GetComponent<TankInfo>();
                    AgentInfo.action = (JArray)actions[i];
                    
                    
                }
            }





        }

    }
}
