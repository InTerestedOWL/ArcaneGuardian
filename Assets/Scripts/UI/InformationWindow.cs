using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//Tutorialvorlage von https://www.youtube.com/watch?v=1J7suYYf5bw
public class InformationWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    private GameObject window;
    private Animator iwAnimator;



    private Coroutine queueChecker;

    private Queue<string> popupQueue;
    public void popupInformationWindow(string text){
        if(popupQueue.Count <= 3){
            popupQueue.Enqueue(text);
        }       
        if(queueChecker==null){
            queueChecker = StartCoroutine(CheckQueue());
        }
    }
    private void showInformationWindow(string text){
 
        window.SetActive(true);
        infoText.text = text;
        iwAnimator.Play("InformationWindowAnimation");
    }
    private IEnumerator CheckQueue(){
        do{
            showInformationWindow(popupQueue.Dequeue());
            do{
                yield return null;
            }while(!iwAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
            
        }while(popupQueue.Count > 0);

        window.SetActive(false);
        queueChecker = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        window = this.transform.GetChild(0).gameObject;
        iwAnimator = this.GetComponent<Animator>();
        window.SetActive(false);

        popupQueue = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
