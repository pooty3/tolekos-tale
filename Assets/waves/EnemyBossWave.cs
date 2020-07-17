﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossWave : EnemyWave
{
    protected static float endPhaseTransition = 0.2f;
    protected static float spellCardTransition = 1.7f;
    public Boss boss;

    [HideInInspector] public GameObject bossImage;
    [Header("Audio")]
    public Sprite bossPortrait;
    protected SpellCardUI currentUI;
    protected Boss currentBoss;
    [SerializeField] protected List<string> namesOfSpellCards;




    [Header("Next Scene")]
    [SerializeField] protected LevelDescription nextLevel;

    public Action OnDefeat;



    



   
    public virtual void Collect()
    {
        GameManager.CollectEverything();
    }
    public virtual void NextStage()
    {
        GameManager.sceneLoader.LoadShopScene(nextLevel);


    }

    

    protected virtual void PlayLifeDepletedSound()
    {
        AudioManager.current.PlaySFX(GameManager.gameData.lifeDepletedSFX);

    }

    public virtual void EndPhase()
    {
        try
        {
            PlayLifeDepletedSound();
            currentBoss.shooting.StopAllCoroutines();
            currentBoss.movement.StopMoving();
            currentBoss.enemyAudio.StopAllCoroutines();
            StartCoroutine(DestroyNonBoss());
            if (currentUI)
            { Destroy(currentUI.gameObject); }
            SwitchToImage();
            }catch (Exception ex)
        {
            Debug.Log(ex);
        }
        
    }
    protected IEnumerator DestroyNonBoss() {
        yield return null;
        bool bl = GameManager.DestroyAllNonBossEnemy(true);
        if (bl)
        { yield return null; }
        GameManager.DestoryAllEnemyBullets();
    }

    public virtual void SpellCardUI(string name)
    {
        currentUI = Instantiate(GameManager.gameData.spellcardUI);
        if (!bossPortrait) {
            bossPortrait = bossImage.GetComponent<SpriteRenderer>().sprite;
        }
        currentUI.SetImage(bossPortrait);
        currentUI.PlaySFX();
        currentUI.SetText(name.Replace("\\n", "\n"));

    }

    protected virtual void SwitchToImage()
    {
        bossImage.GetComponent<SpriteRenderer>().enabled = true;
        try
        {
            bossImage.transform.position = currentBoss.transform.position;
            currentBoss.gameObject.SetActive(false);
        }
        catch (Exception ex) 
        {
            Debug.Log(ex);
        }

      
    }

    protected virtual void SwitchToBoss()
    {
         try
         {
             currentBoss.gameObject.SetActive(true);
             currentBoss.transform.position = bossImage.transform.position;
             bossImage.GetComponent<SpriteRenderer>().enabled = false;
         }
         catch (Exception ex)
         {
             Debug.Log(ex);
         }
      
    }

}
