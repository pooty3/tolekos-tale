﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is responsible for the death of objects and triggers the death effects
public class Death : MonoBehaviour
{
    [SerializeField] GameObject explosion = default;
    [SerializeField] SFX deathSound = default;
    [SerializeField] Health hp = default;
    public bool canDie = true;
    public int experience = 10;
    public event Action OnDeath;
    public event Action OnLifeDepleted;

    private void Awake()
    {
        hp = hp ? hp : GetComponent<Health>();
        hp.OnDeath += Die;

    }

    private void Update()
    {
        
    }
    protected void GeneralDie() {
        OnDeath?.Invoke();
        PlayerStats.GainEXP(this.experience);
        GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioManager.current.PlaySFX(deathSound);
        Destroy(exp, 1f);
    }
    public virtual void Die()
    {
        GeneralDie();

         Destroy(gameObject);
        
     

    }
}
