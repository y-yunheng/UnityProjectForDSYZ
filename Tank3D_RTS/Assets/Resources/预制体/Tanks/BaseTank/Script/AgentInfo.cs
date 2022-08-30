using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public Light canobs;
    public Light canattack;
    public int group = 0;
    public int id;
    public int agent_type = 0;//坦克、飞机、或者其它
    public JArray pos = new JArray();
    public float heading;
    public float hp;
    public int bullet;
    public int motion_state = 0;
    public int alive = 1;//0:false,1:true
    public int attack_id = -1;
    public int be_attacked_id = -1;
    public ArrayList enemies_in_obs = new ArrayList();
    public ArrayList enemies_can_attack = new ArrayList();
    public int attacking = 0;
    public int be_attacked = 0;
    public int is_auto_attack = 0;

    public JArray dpos= JArray.Parse("[0,0,0]");
    public bool isreach_dpos = true;
    public float dheading;
    public bool is_reach_dpos=false;

    public bool is_select=false;
    public JArray action;

    private GameObject[] Agents;
    public int reward;

    void Start()
    {
       
    }
    private void Awake()
    {
        Agents = GameObject.FindGameObjectsWithTag("Agents");
        dpos[0] = transform.position.x;
        dpos[1] = transform.position.y;
        dpos[2] = transform.position.z;
        isreach_dpos = true;
        set_pos_rot();

    }
    // Update is called once per frame
    void Update()
    {
        set_pos_rot();
        set_is_reach_dpos();
        set_canobs();
        set_attack_id();
    }

    void set_pos_rot()
    {
        heading = this.transform.rotation.y;
        float rx = transform.position.x;
        float ry = transform.position.y;
        float rz = transform.position.z;
        pos = JArray.Parse("[" + rx + "," + ry + "," + rz + "," + "]");
    }
    void set_is_reach_dpos()
    {
        if(dpos==pos)
        {
           is_reach_dpos=true;
        }else
        {
            is_reach_dpos = false;
        }
    }
    void set_canobs()
    {
        foreach (GameObject agent in Agents)
        {
            AgentInfo agentinfo = agent.GetComponent<AgentInfo>();         
            if (agentinfo.group != group)
            {
                Vector3 distance =transform.position - agent.transform.position;
                if (canobs.range> distance.magnitude)
                {
                    if (!enemies_in_obs.Contains(agentinfo.id))
                    {
                        enemies_in_obs.Add(agentinfo.id);
                    }
                }
                else
                {
                    if (enemies_in_obs.Contains(agentinfo.id))
                    {
                        enemies_in_obs.Remove(agentinfo.id);
                    }
                }




                if (canattack.range >distance.magnitude)
                {
                    if (!enemies_can_attack.Contains(agentinfo.id))
                    {
                        enemies_can_attack.Add(agentinfo.id);
                    }
                }
                else
                {
                    if (enemies_can_attack.Contains(agentinfo.id))
                    {
                        enemies_can_attack.Remove(agentinfo.id);
                    }

                }


            }

        }

    }
    void set_attack_id()
    {
        if (is_auto_attack != 0 && enemies_can_attack.Count > 0)
        {
            foreach (var id in enemies_can_attack)
            {
                int agentId = (int)id;
                attack_id = agentId;

            }

        }
    }

    void set_isreach_dpos()
    {
        Vector3 tar_position = new Vector3((float)dpos[0], (float)dpos[1], (float)dpos[2]);
        if (tar_position - transform.position == new Vector3(0, 0, 0))
        {
            isreach_dpos = true;
        }
    }
}
