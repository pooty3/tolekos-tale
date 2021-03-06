﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4EndBoss : EnemyBossWave
{
    // Start is called before the first frame update
    [SerializeField] GameObject key;
    [SerializeField] Dialogue preFight1, preFight2;
    [SerializeField] BulletOrientation fireCircle;
    [SerializeField] ParticleSystem spawnEffect;
    [SerializeField] float delaybeforeSpinning, delaybeforeparticle, spinAcceleration, delayBeforeDialogue = 2f;
    [SerializeField] GameObject image;
    [SerializeField] Vector2 spawnLocation;
    [SerializeField] bool harder = false;
    Bullet star;
    [Header("Pattern1")]
    [SerializeField] float period1 = 0.5f, shootSpeed1 = 3f, spread1 = 50f, timeperPulse1 = 5f, shotRate1 = 0.05f;
    [SerializeField] float delaystart1 = 0.5f, angularvel = 10f, totaltimemove1 = 3f;
    [SerializeField] float delaytoNext = 2f, bounds = 0.8f, speed1 = 7f;
    [SerializeField] float fireBalldmg1 = 300f, fireBeamdmg1 = 3000f;
    [SerializeField] float arrowdmgharder1 = 300, arrowspeedharder1 = 0.8f, arrowpulserateharder1 = 0.5f;
    [SerializeField] int arrownumber1= 40;
    [Header("Pattern2")]
    [SerializeField] GameObject explosionCircle;
    [SerializeField] float delay2 = 1.5f, slowSpawnRate2 = 0.1f, fastSpawnRate2= 0.05f;
    [SerializeField] int numberofbulletsperexplosion2min = 10, numberofbulletsperexplosion2max = 30;
    [SerializeField] float maxSpeed2 =5, minSpeed2 = 1, dmg2star = 300f;
    [SerializeField] float maxangularvel = 500f;
    [SerializeField] float harderpulserate2 = 0.5f, harderradius2= 1f, harderspeed2 = 2.3f, harderdmg2 = 400;
    [SerializeField] int hardernumber2= 10;
    [SerializeField] Dictionary<int, GameObject> explosionCircles2 = new Dictionary<int, GameObject>();
    [Header("Pattern3")]
    [SerializeField] float fireballdmg3 = 350f, fireballspeed3, fireballshotrate, fireballpulserate, fireballpulsetime, fireballspread3 = 20f;
    [SerializeField] float laserDmg3 = 600f;
    [SerializeField] int fireballNumber3, numberOfCircles3, numberoflasers3;
    [SerializeField] float y3, x3, delayBetweenEach3=0.05f, lasershotRate3= 0.5f, spreadlaser3 = 10, laserspeed = 2f;
    [SerializeField] Vector2 bossLocation3;
    [SerializeField] float harderangularvel3 = 14f;
    [Header("Pattern4")]
    [SerializeField] float dmg4;
    [SerializeField] float speed4circle, radialVel4;
    [SerializeField] float delayBeforeMovingOff4 = 5f, delayPerBullet4 = 0.03f, shotRate4 = 0.1f, acc4 = 3f, timeAccelerating4 = 1f, timeBetweenCircles4 = 10f;
    [SerializeField] int numberOfCircles4= 8;
    [Header("Pattern5")]
    [SerializeField] float arcAngle5= 320f, arcshotRate5 = 1f, arcspeed5 = 3f, spacingAngle5 = 1f,firearcdmg5= 500f;
    [SerializeField] float speedBullet5Min = 2f, speedBullet5Max = 4f, shotRateMax = 0.1f, shotRateMin = 0.05f, stardmg5 = 300f, maxSpin5 = 500f;
    [SerializeField] int bulletCountMultiplier = 2;
    [SerializeField] float harderanglerange5 = 20f;
    [Header("Pattern6")]
    [SerializeField] float delay6 = 0.3f, spacing6Min = 0.8f, spacing6Max = 1f, startMinX6 = -4f, startMaxX6 = -3.5f, laserduration6 = 3f, dmglaser6= 1000, pulseRate6 = 5f,magiccirclespeed = 20f;
    [SerializeField] float fireballdmg6 = 300f, radialvel6 = 2f, angularvel6 = 10f, delayBetweenPulse6 = 0.5f, pulseRate6bullet = 3f, xpositionCircle = 2.5f, yposmin6 = -1.5f, yposMax6 = 3f;
    [SerializeField] int numberOfLasers = 5, numberOfBullets6 = 30, numberOfBigBullet6 = 30;
    [SerializeField] float speed6Big = 2f, dmg6Big = 400f, shotRatebig6 = 1.5f;
    [Header("Pattern7")]
    [SerializeField] float laserLastTime7 = 5f, laserDmg7 = 3000f, laserShotRate7 = 7f, distanceMin7 = 2f, distanceMax7 =3f, enemymovespeed7 = 15f;
    [SerializeField] float minSpeed7 = 1f, maxSpeed7 = 5f, shotRate7=0.3f,shotinterval7 = 0.05f, shotdmg7= 150f;
    [SerializeField] int numberMin7 = 3, numberMax7 = 7;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] float harderanglerange7;
    [Header("Dialogues")]
    [SerializeField] Dialogue midFightDialogue, endDialogue;
    protected override void SwitchToBoss()
    {
        bossImage.GetComponent<ParticleSystem>().Stop();
        base.SwitchToBoss();
    }

    protected override void SwitchToImage()
    {
        bossImage.GetComponent<ParticleSystem>().Play();
        base.SwitchToImage();
    }
    public override void SpawnWave()
    {
        star = GameManager.gameData.starBullet.GetItem(DamageType.Fire);
        StartCoroutine(PreFight1());

    }
    //Set up Boss Fight
    IEnumerator PreFight1() {
        try
        {
            GameObject.Find("stage4quad").GetComponent<Animator>().SetTrigger("StopMoving");
        }
        catch (Exception ex) { Debug.Log(ex.ToString()); }
        AudioManager.current.PlaySFX(GameManager.gameData.magicCircleSummonSFX);
        BulletOrientation circle = Instantiate(fireCircle, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(delaybeforeSpinning);
        circle.SetCustomAngularVel(t => spinAcceleration * t);
        yield return new WaitForSeconds(delaybeforeparticle);
        AudioManager.current.PlaySFX(GameManager.gameData.firebossSummonSFX);
        StartCoroutine(GameManager.maincamera.ShakeCamera(0.1f, 0.2f));
        Destroy(circle.gameObject);
        Destroy(Instantiate(spawnEffect, spawnLocation, Quaternion.Euler(-90, 0, 0)).gameObject, 5f);
        bossImage = Instantiate(image, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(delayBeforeDialogue);
        StartCoroutine(DialogueManager.StartDialogue(preFight1, PreFight2));

    }
    void PreFight2() {
        GameManager.PlayEndBossMusic();
        StartCoroutine(DialogueManager.StartDialogue(preFight2, Phase1));
    }
    //Phase 1
    void Phase1() {
        currentBoss = Instantiate(boss, spawnLocation, Quaternion.identity);
        SwitchToBoss();
        GameManager.currentBoss = currentBoss;
        currentBoss.shooting.StartShooting(Functions.RepeatCustomAction(i => {
            float time = currentBoss.movement.MoveTo(Functions.RandomLocation(spawnLocation-new Vector2(0,bounds/2), bounds, false), speed1);
            currentBoss.shooting.StartShootingAfter(PulsePhase1(

             Functions.AimAtPlayer(currentBoss.transform)), time);
            
            }, delaytoNext + timeperPulse1));
        if (harder)
        {
            currentBoss.shooting.StartShooting(EnemyPatterns.PulsingBullets(GameManager.gameData.ellipseBullet.GetItem(DamageType.Fire), arrowdmgharder1, currentBoss.transform,
              arrowspeedharder1, arrowpulserateharder1, arrownumber1, GameManager.gameData.gunSFX));
        }
        currentBoss.bosshealth.OnLifeDepleted += EndPhase1;
    }
    //Lasers narrow in which the boss shoots in a sine wave
    IEnumerator PulsePhase1(float angle)
    {

        Bullet fireBall = GameManager.gameData.fireBullet;
        Bullet fireBeam = GameManager.gameData.fireBeam;
        currentBoss.shooting.StartShootingFor(EnemyPatterns.CustomSpinningStraightBullets(
            fireBall, fireBalldmg1, currentBoss.transform, shootSpeed1, t => angle + spread1 * Mathf.Sin((float)(2 * Math.PI / period1 * t)),
            4, shotRate1, GameManager.gameData.firestreamingSFX
            ), 0, timeperPulse1);
        Bullet laserLeft = GameManager.bulletpools.SpawnBullet(fireBeam, currentBoss.transform.position, Quaternion.Euler(0, 0, angle - spread1));
        Bullet laserRight = GameManager.bulletpools.SpawnBullet(fireBeam, currentBoss.transform.position, Quaternion.Euler(0, 0, angle + spread1));
        AudioManager.current.PlaySFX(GameManager.gameData.laser1SFX);
        laserLeft.SetDamage(fireBeamdmg1);
        laserRight.SetDamage(fireBeamdmg1);
        laserLeft.orientation.SetFixedOrientation(angle - spread1);
        laserRight.orientation.SetFixedOrientation(angle + spread1);

        yield return new WaitForSeconds(delaystart1);

        laserLeft.orientation.SetCustomAngularVel(t => t < totaltimemove1 ? angularvel : 0);
        laserRight.orientation.SetCustomAngularVel(t => t < totaltimemove1 ? -angularvel : 0);
        yield return new WaitForSeconds(timeperPulse1 - delaystart1);
        laserLeft.Deactivate();
        laserRight.Deactivate();
    }
    void EndPhase1() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase1;
        EndPhase();
        Invoke("StartPhase2", endPhaseTransition);
    }
    //Phase 2 (circles narrow in and explode)
    void StartPhase2() {
        SpellCardUI(namesOfSpellCards[0]);
        Invoke("Phase2", spellCardTransition);
    }
    void Phase2() {
        SwitchToBoss();
        float maxHP = currentBoss.bosshealth.maxHP;
        currentBoss.shooting.StartShooting(Functions.RepeatCustomActionCustomTime(
            i=> currentBoss.shooting.StartShooting(SummonExplosion(Functions.RandomLocation(-3.5f,3.5f,-3.5f,3.5f, false)
            , (int)((numberofbulletsperexplosion2max-numberofbulletsperexplosion2min)*(1-currentBoss.bosshealth.GetCurrentHP()/maxHP))+ numberofbulletsperexplosion2min))
             ,i=> (slowSpawnRate2- fastSpawnRate2) * currentBoss.bosshealth.GetCurrentHP() / maxHP+ fastSpawnRate2));
        if (harder) {
            currentBoss.shooting.StartShooting(Functions.RepeatAction(
                () => Patterns.RingAroundBossAimAtPlayer(GameManager.gameData.fireBall, harderdmg2, currentBoss.transform.position, harderradius2, harderspeed2, hardernumber2, GameManager.gameData.gunSFX),
                harderpulserate2));
        }
        currentBoss.bosshealth.OnLifeDepleted += EndPhase2;
    }
    //Summons a magic circle before exploding
    IEnumerator SummonExplosion(Vector2 location, int numberofBullets)
    {

        GameObject obj = Instantiate(explosionCircle, location, Quaternion.identity);
        explosionCircles2.Add(obj.GetInstanceID(), obj);
        yield return new WaitForSeconds(delay2);
        if (obj)
        {
            for (int i = 0; i < numberofBullets; i++)
            {
                float speed = UnityEngine.Random.Range(minSpeed2, maxSpeed2);
                Bullet bul = Patterns.ShootStraight(star, dmg2star, location, UnityEngine.Random.Range(0f, 360f),
                    speed, GameManager.gameData.firepulseSFX);
                bul.orientation.StartRotating(maxangularvel * speed / maxSpeed2);


            }
            Destroy(obj);
            explosionCircles2.Remove(obj.GetInstanceID());
        }

    }
    //Remove the remaining magic circles after end
    void DestroyAllMagicCicles()
    {

        foreach (GameObject circle in explosionCircles2.Values)
        {
            Destroy(circle, 0.05f);
        }
        explosionCircles2 = null;


    }
    void EndPhase2()
    {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase2;
        DestroyAllMagicCicles();
        EndPhase();
        Invoke("StartPhase3", endPhaseTransition);
    }
    //Phase 3
    void StartPhase3()
    {
        SwitchToBoss();
        float time = currentBoss.movement.MoveTo(bossLocation3, speed1);
    
         currentBoss.shooting.StartShootingAfter(Functions.RepeatAction(
              () => currentBoss.shooting.StartShooting(Phase3BossPulse(Functions.AimAtPlayer(currentBoss.transform))),
              fireballpulserate), time);
         for (int i = 0; i < numberOfCircles3; i++)
        {
            Bullet circle = GameManager.bulletpools.SpawnBullet(GameManager.gameData.fireCircle, currentBoss.transform.position);
            circle.movement.MoveTo(new Vector2(-x3 + i * 2 * x3 / (numberOfCircles3 - 1), y3), speed1);
            circle.GetComponent<Shooting>().StartShootingAfter(EnemyPatterns.ShootAtPlayerWithLines(
                GameManager.gameData.fireShortLaser, laserDmg3, circle.transform, laserspeed, lasershotRate3, spreadlaser3, numberoflasers3,GameManager.gameData.firestreamingSFX)
                , delayBetweenEach3 * i + 1.5f);
        }
        currentBoss.bosshealth.OnLifeDepleted += EndPhase3;
    }
    //The bullet subpattern of phase 3
    IEnumerator Phase3BossPulse(float angle)
    {
        SFX sfx = GameManager.gameData.magicPulse1SFX;
        Bullet fireball = GameManager.gameData.smallRoundBullet.GetItem(DamageType.Fire);
        if (!harder)
        {
            return Functions.RepeatActionXTimes(() => Patterns.ShootMultipleStraightBullet(fireball, fireballdmg3, currentBoss.transform.position, fireballspeed3,
                angle, fireballspread3, fireballNumber3, sfx), fireballshotrate, (int)(fireballpulsetime / fireballshotrate));
        }
        else
        {
            return Functions.RepeatCustomActionXTimes(i => Patterns.SpirallingOutwardsRing(
                        fireball, fireballdmg3, currentBoss.transform.position, fireballspeed3, (i % 2 == 0 ? 1 : -1) * harderangularvel3, fireballNumber3, 0, sfx),
                        fireballshotrate, (int)(fireballpulsetime / fireballshotrate));
        }


    }
    void EndPhase3() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase3;
        EndPhase();
        Invoke("StartPhase4", endPhaseTransition);

    }
    //Phase 4 (Spiral Magic circles)
    void StartPhase4() {
        SpellCardUI(namesOfSpellCards[1]);
        Invoke("Phase4", spellCardTransition);
    }
    void Phase4() {
        SwitchToBoss();
        float time = currentBoss.movement.MoveTo(new Vector2(0, 0), speed1);

        currentBoss.shooting.StartShootingAfter(Functions.RepeatCustomAction(
            i =>
            {
                for (int j = 0; j < numberOfCircles4; j++)
                {
                    int z = i % 4;
                    ShootMagicCircleRadiallyOutward(j * 360f / numberOfCircles4, z < 2, z % 2 == 0);
                }
            }, timeBetweenCircles4)
            , time);
        currentBoss.bosshealth.OnLifeDepleted += EndPhase4;
    }
    //Shoot the circle in a spiralling out movement
    void ShootMagicCircleRadiallyOutward(float startAngle, bool outwards, bool left)
    {
        Bullet magicCircle = GameManager.bulletpools.SpawnBullet(GameManager.gameData.fireCircle, currentBoss.transform.position);
        AudioManager.current.PlaySFX(GameManager.gameData.gunSFX);
        magicCircle.movement.Reset();
        magicCircle.movement.destroyBoundary = 5f;
        magicCircle.movement.SetCustomGraph(t => new Polar((radialVel4 * (t + 0.01f)), Mathf.Rad2Deg * (left ? 1 : -1) * speed4circle / radialVel4 * (Mathf.Log((t + 0.01f) / 0.01f)) + startAngle).rect, MovementMode.Position);
        magicCircle.GetComponent<Shooting>().StartShooting(Functions.RepeatCustomAction(
            i => Pattern4Bullet(magicCircle.orientation.angle + (outwards ? 90 : -90), magicCircle.transform.position, delayBeforeMovingOff4 +
            i * delayPerBullet4, acc4, timeAccelerating4), shotRate4));


    }
    //Bullets move after a certain delay
    Bullet Pattern4Bullet(float angle, Vector2 origin, float delay, float acceleration, float totalAccelerationTime)
    {
        Bullet bul = Patterns.ShootCustomBullet(star, dmg4, origin,
            Movement.RotatePath(angle, t => new Vector2(t < delay ? 0 : t < delay + totalAccelerationTime ? acceleration : 0, 0)), MovementMode.Acceleration, GameManager.gameData.kirasoftSFX);
        bul.transform.localScale = 0.8f * bul.transform.localScale;
        return bul;


    }
    void EndPhase4() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase4;
        EndPhase();
        Invoke("Phase5", endPhaseTransition);
    }
    //Phase 5
    void Phase5() {
        SwitchToBoss();
        Bullet fire = GameManager.gameData.fireBullet;
        float time = currentBoss.movement.MoveTo(spawnLocation, speed1);
        currentBoss.shooting.StartShootingAfter(Functions.RepeatCustomAction(
            i =>
            {
                float originAngle = 90 + (i % 2 == 0 ? 1 : -1) * arcAngle5 / 2;
                Patterns.ShootMultipleStraightBullet(fire, firearcdmg5, currentBoss.transform.position, arcspeed5, originAngle,
                    spacingAngle5, (int)(arcAngle5 / (spacingAngle5 * 2)),GameManager.gameData.firepulseSFX);

            }, arcshotRate5
            ), time);
        currentBoss.shooting.StartShootingAfter(Functions.RepeatCustomActionCustomTime(
            i=> {
                for (int j = 0; j < bulletCountMultiplier; j++) {
                    float speed = UnityEngine.Random.Range(speedBullet5Min, speedBullet5Max);
                    Bullet star2 = Patterns.ShootStraight(star, stardmg5, new Vector2(UnityEngine.Random.Range(-3.9f, 3.9f), 4.1f),
                        -90 + (harder? UnityEngine.Random.Range(-harderanglerange5, harderanglerange5):0),
                        speed,null);
                    star2.orientation.StartRotating(speed / speedBullet5Max * maxSpin5);
                }
            }, i=> UnityEngine.Random.Range(shotRateMin, shotRateMax)
            ),time);
        currentBoss.bosshealth.OnLifeDepleted += EndPhase5;
    }
    void EndPhase5() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase5;
        EndPhase();
        Invoke("StartPhase6", endPhaseTransition);
    }
    //Phasee 6 (Lasers and rotating balls)
    void StartPhase6() {
        SpellCardUI(namesOfSpellCards[2]);
        Invoke("Phase6", spellCardTransition);
    }
    void Phase6() {
        SwitchToBoss();
        currentBoss.shooting.StartShooting(Functions.RepeatCustomAction(
            i => currentBoss.shooting.StartShooting(SummonLasers(UnityEngine.Random.Range(startMinX6, startMaxX6))), pulseRate6));
        currentBoss.shooting.StartShooting(Functions.RepeatCustomAction(
            i => {
                float time = currentBoss.movement.MoveTo(i % 2 == 0 ? Functions.RandomLocation(-3, -1, 1, 3, false) : Functions.RandomLocation(1, 3, 1, 3, false), speed1);
                float ypos = UnityEngine.Random.Range(yposmin6, yposMax6);
                currentBoss.shooting.StartShootingAfter(SummonFireCircles(new Vector2(xpositionCircle, ypos)), time);
                currentBoss.shooting.StartShootingAfter(SummonFireCircles(new Vector2(-xpositionCircle, ypos)), time);

            }, pulseRate6bullet));
        currentBoss.shooting.StartShootingAfter(EnemyPatterns.PulsingBulletsRandomAngle(
            GameManager.gameData.bigBullet.GetItem(DamageType.Fire), dmg6Big, currentBoss.transform, speed6Big, shotRatebig6, numberOfBigBullet6,GameManager.gameData.gunSFX)
            , shotRatebig6);
        currentBoss.bosshealth.OnLifeDepleted += EndPhase6;
    }
    //Fire Circle subpattern for phase 6
    IEnumerator SummonFireCircles(Vector2 position)
    {

        Bullet fireRoundBall = GameManager.gameData.smallRoundBullet.GetItem(DamageType.Fire);
        Bullet fireCircle = GameManager.bulletpools.SpawnBullet(GameManager.gameData.fireCircle, currentBoss.transform.position);
        float time = fireCircle.movement.MoveTo(position, magiccirclespeed);
        yield return new WaitForSeconds(time);
        if (fireCircle)
        {
            fireCircle.GetComponent<Shooting>().StartShooting(Functions.RepeatCustomActionXTimes(
                i =>
                {

                    Patterns.CustomRing(angle => {
                        int y = i;
                        Bullet bul = Patterns.ShootCustomBullet(fireRoundBall, fireballdmg6, fireCircle.transform.position,
                t => new Polar(radialvel6 * t, (y % 2 == 0 ? -1 : 1) * angularvel6 * t + angle).rect, MovementMode.Position, GameManager.gameData.firepulseSFX);
                        bul.movement.destroyBoundary = 8f;
                        return bul;
                    }, 0, numberOfBullets6);
                }, delayBetweenPulse6, 2
                ));
        }
        yield return new WaitForSeconds(delayBetweenPulse6 + 0.1f);
        if (fireCircle)
        {
            fireCircle.Deactivate();
        }
    }
    //Fire Laser subpattern for phase 6
    IEnumerator SummonLasers(float startX)
    {
        float x = startX;
        float time = 0;
        List<Bullet> buls = new List<Bullet>();
        for (int i = 0; i < numberOfLasers; i++)
        {
            Bullet fireCircle = GameManager.bulletpools.SpawnBullet(GameManager.gameData.fireCircle, currentBoss.transform.position);
            fireCircle.movement.destroyBoundary = 5f;
            float time1 = fireCircle.movement.MoveTo(new Vector2(x, -4.4f), magiccirclespeed);
            buls.Add(fireCircle);
            time = time > time1 ? time : time1;
            x += UnityEngine.Random.Range(spacing6Min, spacing6Max);

        }
        yield return new WaitForSeconds(delay6 + time);
        for (int i = 0; i < numberOfLasers; i++)
        {
            if (buls[i])
            {
                Bullet bul = Instantiate(GameManager.gameData.fireBeam2, buls[i].transform.position, Quaternion.Euler(0, 0, 90));
                bul.movement.destroyBoundary = 5f;
                bul.SetDamage(dmglaser6);
                bul.orientation.SetFixedOrientation(90);
                Destroy(bul.gameObject, laserduration6);
                Destroy(buls[i].gameObject, laserduration6);
            }
        }
        yield return new WaitForSeconds(1);
        AudioManager.current.PlaySFX(GameManager.gameData.laser1SFX);

    }
    void EndPhase6() {
        currentBoss.bosshealth.OnLifeDepleted -= EndPhase6;
        Instantiate(GameManager.gameData.defaultBombDrop, currentBoss.transform.position, Quaternion.identity);
        EndPhase();
        StartCoroutine(DialogueManager.StartDialogue(midFightDialogue, StartPhase7));
    }
    //Phase 7 (Master Spark)
    void StartPhase7() {
        SpellCardUI(namesOfSpellCards[3]);
        Invoke("Phase7", spellCardTransition);
    }
    void Phase7() {
        SwitchToBoss();
        Bullet laser = GameManager.gameData.masterSpark;
        currentBoss.shooting.StartShooting(Functions.RepeatAction(()=>
       {
           float angle = Functions.AimAt(new Vector2(0, 0), GameManager.playerPosition);
           Vector2 spawn = new Vector2(0, 0) + (Vector2)(Quaternion.Euler(0, 0, 180 + angle) * new Vector2(UnityEngine.Random.Range(distanceMin7, distanceMax7), 0));
           currentBoss.shooting.StartShooting(ShootLaserBeams(laser, spawn, enemymovespeed7));
           }, laserShotRate7 ));
        try
        {
            currentBoss.shooting.StartShooting(Functions.RepeatAction(
                () =>
                {
                    float ratio =1- currentBoss.bosshealth.GetCurrentHP() / currentBoss.bosshealth.maxHP;
                    int number = numberMin7 + (int)(ratio * (numberMax7 - numberMin7));
                    float speed = minSpeed7 + (ratio * (maxSpeed7 - minSpeed7));
                    currentBoss.shooting.StartShooting(Phase7ShortLine(number, true, speed));
                    currentBoss.shooting.StartShooting(Phase7ShortLine(number, false, speed));
                }
                , shotRate7));
               
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        currentBoss.bosshealth.OnDeath += EndPhase7;
    }
    //Master spark
    IEnumerator ShootLaserBeams(Bullet lase, Vector2 position, float speed)
    {
        float time = currentBoss.movement.MoveTo(position, speed);
        yield return new WaitForSeconds(time);
        float activationtime = 1f;
        float angle = Functions.AimAtPlayer(currentBoss.transform);
        Bullet laser = GameManager.bulletpools.SpawnBullet(lase, currentBoss.transform.position, Quaternion.Euler(0, 0, angle));
        laser.orientation.SetFixedOrientation(angle);
        laser.SetDamage(laserDmg7);
        yield return new WaitForSeconds(activationtime);
        currentBoss.shooting.StartShooting(GameManager.maincamera.ShakeCamera(0.06f, laserLastTime7 - activationtime));
        AudioManager.current.PlaySFX(GameManager.gameData.mastersparkSFX);
        yield return new WaitForSeconds(laserLastTime7 - activationtime);
        if (laser)
        {
            laser.GetComponent<Animator>().SetTrigger("FadeOut");
        }

    }
    //Lines from top and bottom
    IEnumerator Phase7ShortLine(int number, bool up, float speed) {
        Bullet bul = GameManager.gameData.ellipseBullet.GetItem(DamageType.Fire);
        Vector2 pos = new Vector2(UnityEngine.Random.Range(-3.9f, 3.9f), up? -4.1f: 4.1f);
        float randomfactor = harder ? UnityEngine.Random.Range(-harderanglerange7, harderanglerange7) :0;
        return Functions.RepeatActionXTimes(
            () => Patterns.ShootStraight(bul, shotdmg7, pos,
            up ? 90 + randomfactor: 270 + randomfactor, speed,null), shotinterval7, number);
    }
    void EndPhase7() {
        if (currentBoss)
        {
            currentBoss.bosshealth.OnDeath -= EndPhase7;
        }
        EndPhase();
        Vector2 pos = bossImage.transform.position;
        Destroy(bossImage.gameObject);
        Instantiate(key, pos, Quaternion.identity);
        Instantiate(deathEffect, pos, Quaternion.Euler(-90, 0, 0));
        Invoke("Collect", 0.1f); 
        Invoke("EndDialogue", 1f);
    }
    void EndDialogue() {

        StartCoroutine(DialogueManager.StartDialogue(endDialogue, NextStage));
    }

    
   

   
   

   

}
