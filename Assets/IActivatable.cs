using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IActivatable : MonoBehaviour
{
    public float coolDown = 5;
    float coolDownTimer = 0;

    void Update()
    {
        if (coolDownTimer > 0) {
            coolDownTimer -= Time.deltaTime;
            return;
        }
    }

    protected bool isInCooldown()
    {
        return coolDownTimer > 0;
    }

    protected void startCooldown()
    {
        coolDownTimer = coolDown;
    }

    public void Activate() 
    {
        if (isInCooldown()) {
            return;
        }
        PerformAction();
        startCooldown();
    }

    public virtual void PerformAction() {
        
    }
}
