using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTooltips : MonoBehaviour
{
    [SerializeField] public GameObject tooltip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showResourceTooltip(bool b){
        tooltip.SetActive(b);
    }
}
