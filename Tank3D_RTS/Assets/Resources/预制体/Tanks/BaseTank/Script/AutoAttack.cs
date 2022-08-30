using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    //本脚本致力于使得Agent可以自主选择攻击对象
    // Start is called before the first frame update
    public AgentInfo agentInfo;
    private GameObject[] agents;
    void Start()
    {
        
    }
    private void Awake()
    {
        agents = GameObject.FindGameObjectsWithTag("Tank");
        agentInfo = GetComponent<AgentInfo>();
    }

    // Update is called once per frame
    void Update()
    {   
    

        if (agentInfo.is_auto_attack != 0 && agentInfo.enemies_can_attack.Count>0)
        {
            foreach (var id in agentInfo.enemies_can_attack)
            {
                int agentId = (int)id;
                agentInfo.attack_id = agentId;

            }

        }
    }
            
}
