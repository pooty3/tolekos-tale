﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this class is responsible for putting enemy bullets into and out of the game manager for future interactions
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] ParticleSystem spawnParticles = default;
    [SerializeField] float size = 0.7f;
    [SerializeField] bool spawnAnimation = true;
    // Start is called before the first frame update
    private void OnEnable()
    {

        GameManager.enemyBullets.Add(gameObject.GetInstanceID(), gameObject);
     
    }




    private void OnDisable()
    {

        GameManager.enemyBullets.Remove(gameObject.GetInstanceID());
    }

}
