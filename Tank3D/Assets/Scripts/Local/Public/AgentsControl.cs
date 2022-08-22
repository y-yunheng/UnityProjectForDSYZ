using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsControl : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject Agents;
    public float movespeed=10.0f;
    public bool isend = false;
    private MemorySharePython mp;
    private string last_action_string="[0,0,0,0]";
    private int InertiaNum = 0;
    private int InertiaNumMax = 5;
    void Start()
    {

    }
    void Awake()
    {
        mp = new MemorySharePython( 4096);
        //ע���ڱ�������action��ά��Ϊ1

    }
    // Update is called once per frame
    void Update()
    {
        //���ϴ��ڴ��л�ȡ����
        //ע��Action��ά��Ϊ�ɰ���������0����ü�û�а��£�1����ü��Ѿ����£�ĩβ��λ�ɶ���������������
        //��ʵ�������Ҷ���ActionΪ��ά �ֱ����WASD�����Ƿ���
        try
        {
            string jsonstring = mp.ReadMemory(this.name);
            if(jsonstring == null)
            {
               /* JArray last_action = JArray.Parse(last_action_string);
                int Forward = (int)last_action[0] - (int)last_action[1];
                int Left = (int)last_action[2] - (int)last_action[3];
                Debug.Log(last_action);
                transform.position += -1 * transform.forward * Forward * movespeed * Time.deltaTime; //new Vector3(Forward * movespeed * Time.deltaTime,0, 0);
                last_action_string = last_action.ToString();*/
            }
            else
            {
                JObject jo = ReadJsonJO(jsonstring);
                string PacketName = jo["PacketName"].ToString();
                string AgentName = jo["AgentId"].ToString();
                if (PacketName.Equals("AgentInputPacket") && AgentName.Equals(this.name))
                {
                    JArray ActionList = ReadJsonArray(jo, "Action");


                    //���������н�����ǰ��Ϊ1������Ϊ-1������Ϊ1������Ϊ-1
                    int Forward = (int)ActionList[0] - (int)ActionList[1];
                    int Left = (int)ActionList[2] - (int)ActionList[3];
                    Debug.Log(ActionList);
                    transform.position += -1*transform.forward * Forward * movespeed * Time.deltaTime; //new Vector3(Forward * movespeed * Time.deltaTime,0, 0);
                    last_action_string = ActionList.ToString();
                }
            }
              

        }
        catch (Exception e)
        {

            Debug.Log(e);
        }



    }


    private JObject ReadJsonJO(string JsonString)
    {
       
        string jsontext = JsonString;
        JObject jo = JObject.Parse(jsontext);
        return jo;

    }
    private JArray ReadJsonArray(JObject jo, string keys)
    {

        string action = jo[keys].ToString();
        JArray ja = JArray.Parse(action);
        return ja;
    }
    private void SendJson(int action)
    {
        long times = DateTimeToTimestamp(DateTime.Now);

        string jsonstring = "{\"PacketName\":" + "AgentOp" + ",\"AgentId\":" + 0 + "," + "Action" + ":" + action + "," + "Time" + ":" + times + "}";

    }

    public static long DateTimeToTimestamp(DateTime datetime)
    {
        DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime timeUTC = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);//����ʱ��ת��UTCʱ��
        TimeSpan ts = (timeUTC - dd);
        return (Int64)ts.TotalMilliseconds;//��ȷ������
    }




    // ������ʼ�ĺ��� ����һ�νӴ�ʱ ���Զ�����
    // ��ײ�����Ӵ�ʱ�� �Զ�ִ���������
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="end")
        {
            isend = true;
        }
        
    }

    // ��ײ��������ʱ  ���Զ�ִ�еĺ���
    //private void OnCollisionExit(Collision collision)
    //{
    //    //Debug.Log("zsd");
    //     isend = true;
    //
    //}
    //
    //// ���������໥�Ӵ�Ħ��ʱ �᲻ͣ�ĵ��øú���
    //private void OnCollisionStay(Collision collision)
    //{
    //    //Debug.Log("zsd");
    //    //thiscollision = collision;
    //    isend = true;
    //}
    //

}
