using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    public float movespeed;
    public float bodyRotetespeed;
    public float rotatespeed;
    
    public GameObject Tutrret;
    public GameObject Body;
    private JArray action;

    // Start is called before the first frame update


    void Start()
    {
        
    }
    private void Awake()
    {
        //  前后，左右，开火
        action=this.GetComponent<TankInfo>().action;
    }
    // Update is called once per frame
    void Update()
    {
        float Verticalinput = Input.GetAxis("Vertical1");
        //WS方向控制
        float horizontalinput = Input.GetAxis("Horizontal1");
        //AD方向控制    
        this.transform.Rotate(Vector3.up * -1, horizontalinput * Time.deltaTime * rotatespeed);
        this.transform.Translate(Vector3.forward * Verticalinput * Time.deltaTime * movespeed);

        /*    if (Input.GetKey("k"))
            {
                Tutrret.transform.Rotate(Vector3.up * -1, Time.deltaTime * rotatespeed);
            }
            if (Input.GetKey("l"))
            {
                Tutrret.transform.Rotate(Vector3.up , Time.deltaTime * rotatespeed);
            }*/

    }
}
