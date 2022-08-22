using Mirror;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPythonControlNet : NetworkBehaviour
{
    // Start is called before the first frame update
    private MemorySharePython msp=new MemorySharePython();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JObject actionpacket=new JObject();
        string msg=msp.ReadMemory("Action");
        actionpacket=JObject.Parse(msg);
        if(actionpacket["PacketName"].ToString().Equals( "AgentInputPacket"))
        {
            this.GetComponent<TankInfoNet>().action = (JArray)actionpacket["action"];
        }
        
        msp.WriteMemory(this.GetComponent<TankInfoNet>().obsstring, "Obs");

    }
}
