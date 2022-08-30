using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRTSMove : MonoBehaviour
{
    public Camera camera;
    public GameObject SelectingBox;
    public List<GameObject> selectedGameObject;
    GameObject[] gms; 


    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
       gms = GameObject.FindGameObjectsWithTag("Agents");
    }

    void Update()
    {
        GetSelectedGameObject();
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("运行");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();        
            if (Physics.Raycast(ray, out hit, 100f) && selectedGameObject.Count>0)
            {
                
                if (hit.transform.gameObject.GetComponent<AgentInfo>() != null)
                {
                    foreach (GameObject go in selectedGameObject)
                    {
                        AgentInfo agentinfo = go.GetComponent<AgentInfo>();
                        if (agentinfo.group != hit.transform.gameObject.GetComponent<AgentInfo>().group)
                        {
                            agentinfo.attack_id = hit.transform.gameObject.GetComponent<AgentInfo>().id;
                        }
                    }
                   
                }
                else
                {   foreach (GameObject go in selectedGameObject)
                    {
                        AgentInfo agentinfo = go.GetComponent<AgentInfo>();
                        agentinfo.dpos[0] = hit.point[0];
                        agentinfo.dpos[1] = hit.point[1];
                        agentinfo.dpos[2] = hit.point[2];
                        agentinfo.attack_id = -1;
                    }
                    

                }
            }
        }
        if(Input.GetKey("l"))
        {
            foreach (GameObject go in selectedGameObject)
            {
                AgentInfo agentinfo = go.GetComponent<AgentInfo>();
                if (agentinfo.is_select)
                {
                    if (agentinfo.is_auto_attack == 1)
                    {
                        agentinfo.is_auto_attack = 0;

                    }
                    else if (agentinfo.is_auto_attack == 0)
                    {
                        agentinfo.is_auto_attack = 1;
                    }
                    else
                    {
                        agentinfo.is_auto_attack = 0;
                    }

                }
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

    void GetSelectedGameObject()
    {
        
        foreach (GameObject go in gms)
        {
            if (go.GetComponent<AgentInfo>().is_select && !selectedGameObject.Contains(go))
            {
                selectedGameObject.Add(go); 
            }else if (!go.GetComponent<AgentInfo>().is_select && selectedGameObject.Contains(go))
            {
                selectedGameObject.Remove(go);
            }
        }
    }
    
}


