using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRTSMove : MonoBehaviour
{
    public Camera camera;
    public List<SelectableCharacter> selectableChars;
    private AgentInfo agentinfo;


    // Start is called before the first frame update
    void Start()
    {
        agentinfo = GetComponent<AgentInfo>();
    }
    void Awake()
    {
        
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("运行");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();        
            if (Physics.Raycast(ray, out hit, 100f)&&agentinfo.is_select)
            {
                
                if (hit.transform.gameObject.GetComponent<AgentInfo>() != null)
                {
                    if(agentinfo.group != hit.transform.gameObject.GetComponent<AgentInfo>().group)
                    {           
                        agentinfo.attack_id = hit.transform.gameObject.GetComponent<AgentInfo>().id;
                    }
                   
                }
                else
                {
                    agentinfo.dpos[0] = hit.point[0];
                    agentinfo.dpos[1] = hit.point[1];
                    agentinfo.dpos[2] = hit.point[2];
                    agentinfo.attack_id = -1;

                }
            }
        }
        if(Input.GetKey("l") && agentinfo.is_select)
        {
            if(agentinfo.is_auto_attack ==1)
            {
                agentinfo.is_auto_attack = 0;

            }else if(agentinfo.is_auto_attack ==0)
            {
                agentinfo.is_auto_attack=1;
            }else
            {
                agentinfo.is_auto_attack = 0;
            }
            
        }


        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit) && agentinfo.is_select)
            {
                Debug.Log(hit.point);
                agentinfo.attck_dpos[0] = hit.point[0];
                agentinfo.attck_dpos[1] = hit.point[1];
                agentinfo.attck_dpos[2] = hit.point[2];
            }
        }*/


            /* if (Physics.Raycast(ray, out hit))
             {
                 if (hit.collider.tag == "player")
                 {
                     hhh = hit.collider.transform;
                     isse = true;
                 }
                 if (hit.collider.name == "Environment")
                 {
                     if (hhh != null)
                     {
                         target = hit.point;
                         target.y = hhh.position.y;
                         isse = false;
                     }
                 }
             }*/

            /* if (hhh != null)
             {
                 moveto(target);
             }*/
    }
    
}


