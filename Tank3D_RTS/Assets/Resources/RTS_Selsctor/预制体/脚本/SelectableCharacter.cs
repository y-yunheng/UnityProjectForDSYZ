using UnityEngine;

public class SelectableCharacter : MonoBehaviour {

    public SpriteRenderer selectImage;
    public AgentInfo agentInfo;
    private void Awake() {
        selectImage.enabled = false;
        agentInfo = GetComponentInParent<AgentInfo>();
    }

    //Turns off the sprite renderer
    public void TurnOffSelector()
    {
        selectImage.enabled = false;
        agentInfo.is_select=false;
    }

    //Turns on the sprite renderer
    public void TurnOnSelector()
    {
        selectImage.enabled = true;
        agentInfo.is_select=true;   
    }

}
