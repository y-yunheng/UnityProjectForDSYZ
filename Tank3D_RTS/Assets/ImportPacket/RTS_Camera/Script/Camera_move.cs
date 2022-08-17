using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    public float moveSpeed = 5;
    private Transform Inittransform;
    void Start()
    {
        Inittransform = camera.gameObject.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float ws = Input.GetAxis("Vertical");
        float ad = Input.GetAxis("Horizontal");
        this.transform.Translate(this.transform.forward * ws* moveSpeed * Time.deltaTime);
        this.transform.Translate(this.transform.right *ad * moveSpeed * Time.deltaTime);
    }

}
