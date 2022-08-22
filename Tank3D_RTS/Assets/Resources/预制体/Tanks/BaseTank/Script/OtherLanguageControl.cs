using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherLanguageControl : MonoBehaviour
{
    // Start is called before the first frame update
    MemorySharePython mp = new MemorySharePython();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string sendmsg=GetMsg();
        mp.WriteMemory(sendmsg,"Cshape");
        string readstring=mp.ReadMemory("Python");
        /*拿到string后转为JObject对象，即转为json
          将数据写入AgentInfo
         */
    }
    string GetMsg()
    {
        return "";
    }
}
