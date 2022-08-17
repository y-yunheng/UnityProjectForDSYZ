using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualControl : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject Agents;
    public GameObject Camera;
    public float movespeed = 1.0f;
    public bool isend = false;
    private MemorySharePython mp;
    private string last_action_string = "[0,0,0,0]";
    private int InertiaNum = 0;
    private int InertiaNumMax = 5;
    private bool isaixs;
    private Camera mainCamera;
    private Transform cameraTrans;
    public int yMinLimit = -20;
    public int yMaxLimit = 80;
    //旋转速度
    public float xSpeed = 250.0f;//左右旋转速度
    public float ySpeed = 120.0f;//上下旋转速度
    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {

    }
    void Awake()
    {

        // mp = new MemorySharePython(4096);
        //ע���ڱ�������action��ά��Ϊ1

    }
    // Update is called once per frame
    void Update()
    {
        //���ϴ��ڴ��л�ȡ����
        //ע��Action��ά��Ϊ�ɰ���������0����ü�û�а��£�1����ü��Ѿ����£�ĩβ��λ�ɶ���������������
        //��ʵ�������Ҷ���ActionΪ��ά �ֱ����WASD�����Ƿ���

        if (Input.GetKey(KeyCode.K))
        {
            if (isaixs)
            {
                isaixs = false;
                
            }
            else
            {
                isaixs = true;
               
            }

        }
        float horizontalinput = Input.GetAxis("Horizontal1");
        //AD方向控制
        float Verticalinput = Input.GetAxis("Vertical1");
        //WS方向控制
        this.transform.Translate(Vector3.right * horizontalinput * Time.deltaTime * movespeed);

        this.transform.Translate(Vector3.forward * Verticalinput * Time.deltaTime * movespeed);

        if (isaixs)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            //欧拉角转化为四元数
            Quaternion rotationAgent = Quaternion.Euler(0, x, 0);
            transform.rotation = rotationAgent;
            Quaternion rotationCamera = Quaternion.Euler(y, x, 0);
            Camera.transform.rotation = rotationCamera;
        }
        






    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
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
        if (collision.gameObject.name == "end")
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
