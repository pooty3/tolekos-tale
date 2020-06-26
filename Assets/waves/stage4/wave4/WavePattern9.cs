﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct WavePattern9Subwave
{
    public float spawnDelay;
    public float xPos;
    public float yPos;
    public float angularVelocity;
}
public class WavePattern9 : EnemyWave
{
    [SerializeField] EnemyStats stats;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shotRate = 0.1f;
    [SerializeField] float bulletDamage = 200f;
    [SerializeField] int numberOfLines = 3;
    [SerializeField] List<WavePattern9Subwave> subwaveList;
    [SerializeField] float activeTime = 15f;
    protected Enemy enemy;
    protected Bullet bullet;

    public virtual void SetUp()
    {

    }
    
    public override void SpawnWave()
    {
        SetUp();
        foreach (WavePattern9Subwave subwave in subwaveList) 
        {
            StartCoroutine(SpawnSubwave(subwave.spawnDelay, subwave.xPos, subwave.yPos, subwave.angularVelocity));
        }
    }

    IEnumerator SpawnSubwave(float spawnDelay, float xPos, float yPos, float angularVelocity) 
    {
        yield return new WaitForSeconds(spawnDelay);
        this.StartCoroutine(SpawnEnemy(xPos, yPos, angularVelocity));
    }
    
    IEnumerator SpawnEnemy(float xPos, float yPos, float angularVelocity)
    {
        //TODO: Make spawning particle effect
        Vector2 position = new Vector2(xPos, yPos);
        Enemy enemy = Instantiate(this.enemy, position, Quaternion.identity);
        enemy.SetEnemy(stats, false);
        enemy.shooting.StartShooting(EnemyPatterns.ConstantSpinningStraightBullets(bullet, bulletDamage, enemy.transform, bulletSpeed, angularVelocity, 0, numberOfLines, shotRate));
        yield return new WaitForSeconds(activeTime);
        if (enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}