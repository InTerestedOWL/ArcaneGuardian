using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Skills;
using TMPro;

public class TurretController : MonoBehaviour
{
    public Skill skill;

    private bool chargingStarted = false;
    private float chargeTimer = 0;
    private float chargingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        chargingTime = skill.GetMaxCooldown();
        chargeTimer = chargingTime;
    }

    // Update is called once per frame
    void Update()
    {
        chargeTimer -= Time.deltaTime;

        if (chargeTimer <= 0.0f)
        {
            Attack();
            chargingStarted = false;
            chargeTimer = chargingTime;
        }

        if(!chargingStarted) {
            //TODO: Charging animation
            chargingStarted = true;
        }
    }

    void Attack() {
        skill.Use(this.gameObject);
    }
    
    void ChargeUpAttack() {

    }
}
