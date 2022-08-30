using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
public class FlyInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public float forward_force;
    public JArray pos = new JArray();//坐标
    public float heading;//航向(弧度)，水平向右为0，逆时针旋转
    public JArray dpos;
    private float up_force;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void set_pos_rot()
    {
        heading = this.transform.rotation.y;
        float rx = transform.position.x;
        float ry = transform.position.y;
        float rz = transform.position.z;
        pos = JArray.Parse("[" + rx + "," + ry + "," + rz + "," + "]");
    }
}
