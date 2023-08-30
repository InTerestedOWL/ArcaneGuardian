using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text scoreText;

    // Start is called before the first frame update
    public void setGold(int g){
        goldText.text = g.ToString();
    }
    public void setScore(int s){
        scoreText.text = s.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
