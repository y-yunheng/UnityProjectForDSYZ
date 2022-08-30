using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
public class TankInfo : MonoBehaviour
{
// Start is called before the first frame update

   

    /*这是Agent的状态数据*/
    public float turretRotation ;//炮塔旋转度（弧度）
    /*强化学习相关*/
    public JArray action = JArray.Parse("[0,0,0,0]");//在FPS模式下操作Agent的动作空间
    private GameObject[] Agents;
    public GameObject turret;

    void Start()
    {
       
    }
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    void set_pos_rot()
    {
        turretRotation = turret.transform.rotation.y;
    }
}
