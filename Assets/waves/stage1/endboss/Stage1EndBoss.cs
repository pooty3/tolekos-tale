﻿using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;


public class Stage1EndBoss : EnemyWave
{
    [SerializeField] Movement background;
    public GameObject bossImage;
    [SerializeField] Boss theBoss;
    [Header("Dialogues")]
    [SerializeField] Dialogue preBossFight;
    [SerializeField] Dialogue midFightDialogue;
    [SerializeField] Dialogue endFightDialogue;

    [Header("Audio")]
    [SerializeField] SFX spellCardSFX;
    [SerializeField] SFX pattern1SFX;
    [SerializeField] SFX pattern2SFX;
    [SerializeField] SFX pattern3smashSFX;
    [SerializeField] SFX pattern3tpSFX;
    [SerializeField] SFX lifeDepletedSFX;
   
    [Header("Pattern1")]
    [SerializeField]
    BulletPack pattern1ConePack;
    [SerializeField] AudioClip pattern1S2FX;
    [SerializeField] float pattern1volume;
    [SerializeField]
    int pattern1Number = 5, numberOfCone = 5;
    [SerializeField] float pattern1PulseRate = 1f, pattern1SpawnRate = 0.05f, pattern1Spacing = 0.05f, pattern1Speed = 3f;

    [SerializeField] SpellCardUI spellCardUI;
    SpellCardUI currentUI;
    Boss currentBoss;
    [SerializeField] List<string> namesOfSpellCards;
    [Header("Pattern2")]
    [SerializeField] AudioClip pattern2IntroSound;
    [SerializeField] float pattern2IntroSoundVolume;
    [SerializeField] float arrowSpawnTimeMax = 0.05f, arrowSpawnTimeMin = 0.01f;
    [SerializeField] float maxSpeed = 5f, minSpeed = 2f;
    [SerializeField] Bullet arrow;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxY = 3.5f, minY = 0.5f;
    [Header("Pattern2 Homing Behavior")]
    [SerializeField] float arrowSpeed = 1.5f;
    [SerializeField] float shotRate = 1f;

    [Header("Pattern3 Bullet Behavior")]
    [SerializeField] Bullet explodingBullet;
    [SerializeField] int numberOfBulletsPattern3 = 30;
    [SerializeField] float initialSpeedPattern3 = 10f, maxRadiusPattern3 = 1f, shortDelayPattern3 = 0.2f, finalSpeedPattern3 = 2f;
    [SerializeField] float pausetime = 2f;
    [SerializeField] Bullet explosion, punch;
    [SerializeField] float punchSpeed = 2f;
    [SerializeField] float explosionTime;
   [Header("Sound Effects")]
    [SerializeField] AudioClip tpSound, smashSound, beforeSmashSound;
    [SerializeField] float tpSoundVolume = 0.1f, smashSoundVolume = 0.1f, beforeSmashSoundVolume = 0.1f;


    public event Action OnDefeat;
    private void Start()
    {
      
    }
    public void Defeated() {
        OnDefeat?.Invoke();
        GameManager.DestoryAllEnemyBullets();
        DestroyAfter(5);
    }

    public override void SpawnWave() {

        StartCoroutine(PreFight());
    }

    IEnumerator PreFight() {
        Movement bg = Instantiate(background, new Vector3(0, 8, 0.9f), Quaternion.identity);
        bossImage = bg.transform.Find("bossImage").gameObject;
        bg.destroyBoundary = 10f;
        bg.SetSpeed(new Vector2(0, -0.8f));
        bg.StopMovingAfter(10);
        yield return new WaitForSeconds(12);
        StartCoroutine(DialogueManager.StartDialogue(preBossFight, StartBossFight));
    }
   

   
    void StartBossFight() {
        Vector2 initialPosition = new Vector2(bossImage.transform.position.x, bossImage.transform.position.y);

        currentBoss = Instantiate(theBoss, initialPosition, Quaternion.identity);
        bossImage.SetActive(false);
        currentBoss.shooting.StartShooting(Pattern1(currentBoss));
        currentBoss.bosshealth.OnLifeDepleted += EndPhase1;
        
    }

    void EndPhase1() {
      
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase1;
        EndPhase();
        
        Invoke("StartPhase2", 1f);
    
    }

    void StartPhase2() {
        SpellCardUI(namesOfSpellCards[0]);
        spellCardSFX.PlayClip();
        Invoke("StartPattern2", 3f);
    }
    void StartPattern2() {
        
        SwitchToBoss();
        currentBoss.bosshealth.OnLifeDepleted += EndPhase2;
        currentBoss.shooting.StartShootingAfter(RainOfArrows(), 0.3f);
        currentBoss.shooting.StartShootingAfter(MoveLeftAndRight(), 0.3f);
        currentBoss.shooting.StartShootingAfter(EnemyPatterns.ShootAtPlayer(arrow, currentBoss.transform, arrowSpeed, shotRate), 0.3f);
        currentBoss.enemyAudio.PlayAudio(pattern2SFX, shotRate, 0.3f);
  
    }

    void EndPhase2() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase2;
        EndPhase();
        StartCoroutine(DialogueManager.StartDialogue(midFightDialogue, StartPhase3));
        
    }

    void StartPhase3() {
        SpellCardUI(namesOfSpellCards[1]);
        spellCardSFX.PlayClip();
        Invoke("StartPattern3", 3f);
    }

    void StartPattern3() {
        SwitchToBoss();
        Coroutine pattern3 = StartCoroutine(Pattern3());
        currentBoss.bosshealth.OnDeath += EndStage;
    }

    void EndStage() {
        EndPhase();
        bossImage.transform.position = new Vector2(0, 0);
        bossImage.SetActive(true);
        currentBoss.bosshealth.OnDeath -= EndStage;
        StartCoroutine(DialogueManager.StartDialogue(endFightDialogue, NextStage));
    
    }
    void NextStage() {
        GameManager.sceneLoader.OnGameOverEvent(true);


    }

    IEnumerator Pattern3() {
        Animator animator = currentBoss.gameObject.GetComponent<Animator>();
        while (animator) {
            animator.SetTrigger("Disappear");
            pattern3tpSFX.PlayClip();
            yield return new WaitForSeconds(1f);
            if (animator)
            {
                currentBoss.transform.position = GameManager.playerPosition + new Vector2(0, 1f);
                animator.SetTrigger("Appear");
            }
            if (animator)
            {
                yield return new WaitForSeconds(0.8f);
                Bullet punchbul = Instantiate(punch, currentBoss.transform.position, Quaternion.identity);
                punchbul.movement.SetSpeed(new Vector2(0, -punchSpeed));
                yield return new WaitForSeconds(1 / punchSpeed);


                if (punchbul)
                {
                    Vector2 pos = punchbul.transform.position;
                    Bullet explode = Instantiate(explosion, pos, Quaternion.identity);
                    ExplodingAndBack(pos);
                    Destroy(punchbul.gameObject);
                    pattern3smashSFX.PlayClip();
                   

                }

            }
            yield return new WaitForSeconds(pausetime);
        }

    }

 


    IEnumerator Pattern1(Enemy enemy) {
  
        while (true) {
            float angle = UnityEngine.Random.Range(0f, 360f);
            enemy.enemyAudio.PlayAudioTimes(pattern1SFX, pattern1SpawnRate, pattern1Number);
            for (int i = 0; i < numberOfCone; i++) {
                enemy.shooting.StartCoroutine(EnemyPatterns.ConePattern(pattern1ConePack.GetBullet(0), enemy, angle + i * 360f / numberOfCone, pattern1Speed, pattern1SpawnRate, pattern1Number, pattern1Spacing));
            }
            yield return new WaitForSeconds(pattern1PulseRate);
        
        }
    
    }
    IEnumerator MoveLeftAndRight() {
        bool left = true;
        while (true) {
            float toX = left ? -3.5f : 3.5f;
            float toY = UnityEngine.Random.Range(minY, maxY);
            float time = currentBoss.movement.MoveTo(new Vector2(toX, toY), moveSpeed);
            left = !left;
            yield return new WaitForSeconds(time);
        
        }
    
    }

    List<Bullet> ExplodingAndBack(Vector2 pos) {
        List<Bullet> bullets = new List<Bullet>();
        for (int i = 0; i < numberOfBulletsPattern3; i++) {
            Bullet bul = Instantiate(explodingBullet, pos, Quaternion.identity);
            bul.movement.destroyBoundary = 6f;
            bul.movement.SetCustomGraph(t =>
            {
                if (t < maxRadiusPattern3 / initialSpeedPattern3)
                {
                    return new Vector2(0, initialSpeedPattern3);

                }
                else if (t < maxRadiusPattern3 / initialSpeedPattern3 + shortDelayPattern3)
                {
                    return new Vector2(0, 0);
                }
                else
                {
                    return new Vector2(0, -finalSpeedPattern3);
                }



            }, MovementMode.Velocity);

            bul.movement.RotateTrajectory(i * 360 / numberOfBulletsPattern3);
        
        }

        return bullets;
    }

    IEnumerator RainOfArrows() {
        while (true) {
            Bullet bul = Instantiate(arrow, new Vector2(UnityEngine.Random.Range(-4f, 4f), 4.4f), Quaternion.identity);
            bul.setSpeed(new Vector2(0, -UnityEngine.Random.Range(minSpeed, maxSpeed)));

            yield return new WaitForSeconds(UnityEngine.Random.Range(arrowSpawnTimeMin, arrowSpawnTimeMax));
        }
    
    }
    void SwitchToImage(){
        bossImage.SetActive(true);
        bossImage.transform.position = currentBoss.transform.position;
        currentBoss.gameObject.SetActive(false);
    }

    void SwitchToBoss()
    {
        currentBoss.gameObject.SetActive(true);
        currentBoss.transform.position = bossImage.transform.position;
        bossImage.SetActive(false);
    }

    void PlayLifeDepletedSound() {
        lifeDepletedSFX.PlayClip();
        
    }

    void EndPhase() {
        PlayLifeDepletedSound();
        currentBoss.shooting.StopAllCoroutines();
        currentBoss.movement.StopMoving();
        SwitchToImage();
        GameManager.DestoryAllEnemyBullets();
        if (currentUI)
        { Destroy(currentUI.gameObject); }
    }

    void SpellCardUI(string name) {
        currentUI = Instantiate(spellCardUI);
        currentUI.SetText(name.Replace("\\n", "\n"));

    }

    
}