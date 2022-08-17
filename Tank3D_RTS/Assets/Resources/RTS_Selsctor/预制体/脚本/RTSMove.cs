using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMove : MonoBehaviour
{
    public Camera camera;
    private Vector3 tar_position=new Vector3();
    private AgentInfo agentinfo;


    // Start is called before the first frame update
    void Start()
    {
        agentinfo = GetComponent<AgentInfo>();
        Debug.Log(agentinfo.is_select);
        tar_position = transform.position;
    }
    void Awake()
    {
        
        
    }

    void Update()
    {
        moveto(tar_position);
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray =camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit)&& agentinfo.is_select)
            {     
               tar_position = hit.point;  
            }
            

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
        }
       /* if (hhh != null)
        {
            moveto(target);
        }*/
    }
    void moveto(Vector3 tar)
    {
        transform.position = Vector3.MoveTowards(transform.position, tar, 5 * Time.deltaTime);
        
    }
}


