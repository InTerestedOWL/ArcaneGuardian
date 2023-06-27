using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Combat;

public class HealthBar : MonoBehaviour
{
    public Transform target = null;
    public bool isFriendly = false;

    private GameObject HealthbarUIPrefab = null;
    private GameObject UIHealthBars = null;
    private GameObject healthBar = null;
    private HealthBarUI healthBarUIScript = null;
    private CombatTarget combatTarget = null;

    void Start()
    {
        if(HealthbarUIPrefab == null){
            if(isFriendly)
                HealthbarUIPrefab = Resources.Load<GameObject>("HealthbarFriendly");
            else
                HealthbarUIPrefab = Resources.Load<GameObject>("HealthbarEnemy");
        }

        UIHealthBars = GameObject.Find("Healthbars");

        healthBar = Instantiate(HealthbarUIPrefab, UIHealthBars.transform);
        healthBar.name = "Healthbar" + this.name;
        healthBarUIScript = healthBar.GetComponentInChildren<HealthBarUI>();
        healthBarUIScript.SetTarget(target);

        combatTarget = this.transform.GetComponent<CombatTarget>();
        combatTarget.SetHealthBar(healthBarUIScript);
    }

    public HealthBarUI GetHealthBarUI(){
        return this.healthBarUIScript;
    }
}