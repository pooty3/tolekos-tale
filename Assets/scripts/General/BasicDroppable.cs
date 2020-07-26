﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This component encompass the drops an enemy would drop in the game
public class BasicDroppable : MonoBehaviour
{
    [SerializeField] Death death;
    public float chanceToDropCoins=0.35f;
    public int maxGold=100;
    public int minGold=10;
    [SerializeField] Coin coin;
    public List<Collectible> otherDrops = new List<Collectible>();
    private void Start()
    {
        if (death) {
            death.OnDeath += DropItems;
        }
    }
    void DropItems() {
        
        if (Random.Range(0f, 1f) <= chanceToDropCoins) {
            Coin coin = Instantiate(this.coin, transform.position, Quaternion.identity);
            coin.goldAmount = Random.Range(minGold, maxGold + 1);
        }
        if (otherDrops.Count > 0) {
            foreach (Collectible otherDrop in otherDrops) {
                Instantiate(otherDrop,transform.position,Quaternion.identity);
            }
        }

    
    }
}
