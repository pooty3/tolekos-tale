﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This class is responsible for the health of any unit
public class Health : MonoBehaviour
{
    public float maxHP = 1000;
    protected float currentHP;
    public event Action OnDeath;
    public bool canDie = true;
    public event Action<float> OnHeal;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    public void TakeDamage(float dmg) {
        currentHP -= dmg;
    }

    public virtual void CheckDeath() {
        if (ZeroHP()&&canDie) {
            InvokeDeath();
        }
    }

    public virtual void InvokeDeath() {

        OnDeath?.Invoke();
    }

    public virtual void Heal(float amount){
        float oldHP = currentHP;
        float newHP = amount + currentHP;
        currentHP = Mathf.Clamp(newHP, 0, maxHP);
        float actualHeal = currentHP - oldHP;
        OnHeal?.Invoke(actualHeal);

    }
    public virtual float GetCurrentHP()
    {
        return currentHP;
    }

   
    public virtual void IncreaseMaxHP(float hp) {
        maxHP += hp;
        currentHP += hp;
    }
    public virtual void ResetHP() {
        currentHP = maxHP;
    }

    public virtual bool ZeroHP() {
        return currentHP <= 0;
    }

    private void Update()
    {
        CheckDeath();
    }

}
