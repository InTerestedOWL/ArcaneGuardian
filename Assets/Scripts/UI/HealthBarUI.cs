using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{   
    public Transform target = null;
    public Vector3 offset = new(0.0f, 1.5f, 0.0f);
    public Image foregorundImage;
    public Image backgroundImage;

    void Start(){
        
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foregorundImage.enabled = !isBehind;
        backgroundImage.enabled = !isBehind;
        
        this.transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }
    
    public void SetHealthBarPercentage(float percentage) {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregorundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public void SetTarget (Transform target){
        this.target = target;
    }
}