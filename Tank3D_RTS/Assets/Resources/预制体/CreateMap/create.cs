using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
public class create : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cubeObj;
    public GameObject planeObj;
    public GameObject kong_bulid;
    public GameObject kong_plane;
    public JObject obs;
    public JArray roads;
    public JArray buildings;

    void Start()
    {

    }
    private void Awake()
    {
        StreamReader sr = null;
        string path = @"D:\项目\杭州大数云智\UnityProject\UnityProjectForDSYZ\Tank3D_RTS\Assets\Resources\预制体\CreateMap\guide_shoot.json";
        sr = File.OpenText(path);
        string result = sr.ReadToEnd();
        obs = JObject.Parse(result);
        roads = (JArray)obs["roads"];

        buildings = (JArray)obs["buildings"];
        set_plane();
        set_buliding();

        
        // Update is called once per frame
       
    }

    void set_buliding()
    {
        int i=0;
        foreach (var building in buildings)
        {
            JObject buildingo = JObject.Parse(building.ToString());
            JArray pos = JArray.Parse(buildingo["pos"].ToString());
            JArray lu = JArray.Parse(buildingo["lu"].ToString());
            JArray ru = JArray.Parse(buildingo["ru"].ToString());
            JArray rd = JArray.Parse(buildingo["rd"].ToString());

            float width = float.Parse(buildingo["width"].ToString());
            float length = float.Parse(buildingo["length"].ToString());
            float height = float.Parse(buildingo["height"].ToString());
            float alpha = float.Parse(buildingo["alpha"].ToString());
            float r = (alpha / 180) * Mathf.PI;



            //Debug.Log(pos1a);

            Vector3 posx = new Vector3((float)lu[0], 0, (float)lu[1]);
            Vector3 posy = new Vector3((float)rd[0], 0, (float)rd[1]);


            Vector3 postion = (posx + posy) / 2;
            postion +=new Vector3(0,height/2,0);

            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            GameObject NewObj = Instantiate(cubeObj, postion, rotation);
            NewObj.transform.Rotate(Vector3.up * alpha);
            NewObj.name="bulid"+i;
            NewObj.transform.localScale = new Vector3(width, height, length);
            
            NewObj.transform.SetParent(kong_bulid.transform);
            i++;
        }

    }

    void set_plane()
    {
        int i=0;
        foreach (var Road in roads)
        {
              JObject road= JObject.Parse(Road.ToString());
              JArray start=(JArray)road["start"];
              JArray end=(JArray)road["end"];
              Vector3 start_position=new Vector3((float)start[0],(float)start[2],(float)start[1]);
              Vector3 end_position=new Vector3((float)end[0],(float)end[2],(float)end[1]);
              Vector3 direction=(end_position-start_position).normalized;
              Vector3 position=(end_position+start_position)/2;
              Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

              float Long=(end_position-start_position).magnitude;
              float Width=(float)road["width"];
              GameObject NewObj = Instantiate(planeObj, position,rotation);
              NewObj.transform.rotation=Quaternion.LookRotation(direction,Vector3.up);
              NewObj.transform.localScale=new Vector3(Width/10,10,Long/10);
              NewObj.transform.SetParent(kong_plane.transform);
              NewObj.name="road"+i;
              i++;

        }
        
    }
}
