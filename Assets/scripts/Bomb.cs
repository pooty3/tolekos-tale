﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;

    public void SetDamage(float dmg) {
        GetComponent<DamageDealer>().damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bul = other.GetComponent<Bullet>();
        if (bul) {
            bul.Deactivate();
 
        }
    }
    public void Deactivate()
    {
        Destroy(gameObject);
    }
}
