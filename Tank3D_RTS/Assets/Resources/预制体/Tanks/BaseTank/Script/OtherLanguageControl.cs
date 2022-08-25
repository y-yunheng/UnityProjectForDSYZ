using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherLanguageControl : MonoBehaviour
{
    // Start is called before the first frame update
    MemorySharePython mp = new MemorySharePython();
    public AgentInfo agentInfo;
    void Start()
    {
        
    }
    private void Awake()
    {
        agentInfo = GetComponent<AgentInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        //string sendmsg = GetMsg();
       // mp.WriteMemory(sendmsg, "Cshape");
        string msg = mp.ReadMemory("Python");
        Debug.Log(msg);
        JObject msgobj=JObject.Parse(msg);
        /*JArray action = (JArray)msgobj["Action"];
        int agentid = (int)msgobj["AgentId"];
        if(agentid == agentInfo.id)
        {
            agentInfo.action = action;
        }*/
        JArray pos = (JArray)msgobj["Pos"];
        int agentid = (int)msgobj["AgentId"];
        if (agentid == agentInfo.id)
        {
            agentInfo.dpos = pos;
        }
    }

    void loaclOP(JArray dpos)
    {
        Vector3 Dposition = new Vector3((float)dpos[0], (float)dpos[1], (float)dpos[2]);
        
        if (Dposition - transform.position != new Vector3(0, 0, 0))
        {
            Debug.Log(Dposition);
            Debug.Log(transform.position);
            Vector3 dis = Dposition - transform.position;
            float distance = dis.magnitude;

            /*if (distance>0)
            {
                agentInfo.action[0] = -1;
            }else if (distance<0)
            {
                agentInfo.action[0] = 1;
            }else
            {
                agentInfo.action[0] = 0;
            }*/


            float angel = AngleSigned(transform.forward, dis, transform.up);

            if (angel > 0)
            {
                agentInfo.action[1] = 1;
            }
            else if (angel < 0)
            {
                agentInfo.action[1] = -1;
            }
            else
            {
                agentInfo.action[1] = 0;
            }
        }
        
    }
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)

    {

        return Mathf.Atan2(

        Vector3.Dot(n, Vector3.Cross(v1, v2)),

        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;

    }

    string GetMsg()
    {
        return "";
    }
}
