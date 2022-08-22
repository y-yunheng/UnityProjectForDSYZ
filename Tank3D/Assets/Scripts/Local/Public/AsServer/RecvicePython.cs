using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecvicePython : MonoBehaviour
{
    // Start is called before the first frame update
    FileShare fl;
    private MemorySharePython  memory;
    public JObject Obs;
    private bool isinit = false;

    public GameObject[] Agents;
    private Thread ReceiveListern;

    void Start()
    {
        memory = new MemorySharePython();
        //fl = new FileShare("C:\\Users\\yyh\\");
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Tank");
    }

    // Update is called once per frame
    void Update()
    {

        /*try
        {
            //string msg = fl.ReadFile("Reset");
            string msg = memory.ReadMemory("Reset");
            JObject jo = JObject.Parse(msg);
            if ((jo["PacketName"].ToString()).Equals("Reset"))
            {
                //如果是重置包则重置当前场景
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
                jo["PacketName"] = "NoReset";
                //fl.CreateOrOPenFile(jo.ToString(), "Reset");
                memory.WriteMemory(jo.ToString(), "Reset");
            }
        }
        catch
        {
        }*/


        try
        {
            string msg2 = memory.ReadMemory("PythonPacket");
            JObject jo2 = JObject.Parse(msg2);
            if ((jo2["PacketName"].ToString()).Equals("PythonInputPacket"))
            {
                //如果的动作包则改变动作
                JArray actions = JArray.Parse(jo2["Actions"].ToString());
                for (int i = 0; i < Agents.Length; i++)
                {
                    TankInfo agentinfo = Agents[i].GetComponent<TankInfo>();
                    JArray action = (JArray)actions[i];
                    agentinfo.action = action;
                }
            }
        }
        catch
        {
        }


    }
}
