using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Skills;
using TMPro;

public class TurretController : MonoBehaviour
{
    public Skill skill;
    public float chargingTime = 3.0f;

    private bool chargingFinished = false;
    private bool chargingStarted = false;
    private float chargeTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        chargeTimer = chargingTime;
    }

    // Update is called once per frame
    void Update()
    {
        chargeTimer -= Time.deltaTime;

        if (chargeTimer <= 0.0f)
        {
            chargingFinished = true;
        }

        if(!chargingStarted) {
            //TODO: Charging animation
            chargingStarted = true;
        }
        
        if(chargingFinished){
            Attack();
            chargingFinished = false;
            chargingStarted = false;
            chargeTimer = chargingTime;
        }
    }

    void Attack() {
        skill.Use(this.gameObject);
    }
    
    void ChargeUpAttack() {

    }
}
