using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AG.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Tutorialbasis https://www.youtube.com/watch?v=RCVRJDXuUrM
public class ConfirmationWindow : MonoBehaviour

{
    [SerializeField] private TMP_Text confirmationText;
    // Start is called before the first frame update
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private ActionMapHandler actionMapHandler;

    private GameObject playerObj;
    private GameObject window;
    private Action yesAction;
    private Action noAction;

    void Start()
    {
        window = this.transform.GetChild(0).gameObject;
        window.SetActive(false);

        playerObj = GameObject.Find("Player");
        actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void closeConfirmationWindow(){
        actionMapHandler.ChangeToActionMap("Player");
        yesAction = null;
        noAction = null;
    }
    public void popupConfirmationWindow(string text, Action y, Action n){
       
        window.SetActive(true);
        yesAction = y;
        noAction = n;
        confirmationText.text = text;
        
        actionMapHandler.ChangeToActionMap("UI",true);
    }
    public void yesClicked(){
        window.SetActive(false);  
        if(yesAction != null){
            yesAction.Invoke();
        }
        closeConfirmationWindow();
    }

    public void noClicked(){
        window.SetActive(false);
        if(noAction != null){
            noAction.Invoke();
        }
        closeConfirmationWindow();
    }   
}
