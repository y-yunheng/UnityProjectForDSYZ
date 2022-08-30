using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetControl : MonoBehaviour
{
    // Start is called before the first frame update

    public float turn_speed=5;
    public float move_speed=5;
    private AgentInfo agentinfo;
    private Vector3 tar_position;
    void Start()
    {
        
    }
    private void Awake()
    {
        agentinfo = this.GetComponent<AgentInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        RTS_Control();
    }
    void RTS_Control()
    {
        tar_position = new Vector3((float)agentinfo.dpos[0], (float)agentinfo.dpos[1], (float)agentinfo.dpos[2]);
        if (tar_position - transform.position != new Vector3(0, 0, 0))
        {
            moveto();
        }
       
    }
    void moveto()
    {
        tar_position.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tar_position - transform.position), turn_speed * turn_speed);
        transform.position = Vector3.MoveTowards(transform.position, tar_position, move_speed * Time.deltaTime);
    }
}
