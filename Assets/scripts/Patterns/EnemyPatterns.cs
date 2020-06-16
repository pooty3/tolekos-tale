﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyPatterns : MonoBehaviour {

    public static IEnumerator ShootAt(Bullet bullet, float dmg, Transform enemy, float angle, float speed, float shotRate)
    {
        return Functions.RepeatAction(() => Patterns.ShootStraight(bullet, dmg, enemy.position, angle, speed), shotRate);

    }


    public static IEnumerator ExplodingLineAtPlayer(Bullet bullet, float dmg, Transform enemy, float intialSpeed, float finalSpeed, int number, float minTime, float maxTime, float shotRate)
    {
        return Functions.RepeatAction(() => Patterns.ExplodingLine(bullet, dmg, enemy.position, Patterns.AimAt(enemy.transform.position, GameManager.playerPosition), intialSpeed, finalSpeed, number, minTime, maxTime), shotRate);

    }
    public static IEnumerator ShootAtPlayer(Bullet bullet, float dmg, Transform enemy, float speed, float shotRate)

    {
        return ShootAtPlayerWithLines(bullet, dmg, enemy, speed, shotRate, 0, 0);

    }

    public static IEnumerator ShootAtPlayerWithLines(Bullet bullet, float dmg, Transform enemy, float speed, float shotRate, float spreadAngle, int lines)
    {


        return Functions.RepeatAction(() =>
            Patterns.ShootMultipleStraightBullet(bullet, dmg, enemy.position, speed,
                Patterns.AimAt(enemy.position, GameManager.playerPosition), spreadAngle, lines), shotRate);

    }
    public static IEnumerator PulsingLine(Bullet bullet, float dmg, Transform enemy, float speed, float angle, float shotRate, int number) {

        return Functions.RepeatActionXTimes(() => Patterns.ShootStraight(bullet, dmg, enemy.position, angle, speed), shotRate, number);
    }

    public static IEnumerator RepeatSubPatternWithInterval(Func<IEnumerator> subpattern, Shooting enemy, float interval) {
        return Functions.RepeatAction(() => enemy.StartCoroutine(subpattern()), interval);
    }
    public static IEnumerator PulsingLines(Bullet bullet, float dmg, Transform enemy, float speed, float angle, float shotRate, int lines, int number) {
        return Functions.RepeatActionXTimes(() => Patterns.RingOfBullets(bullet, dmg, enemy.position, lines, angle, speed), shotRate, number);
    } 
    public static IEnumerator PulsingBullets(Bullet bullet, float dmg, Transform enemy, float speed, float shotRate, int lines)
    {


        return Functions.RepeatAction(() => Patterns.RingOfBullets(bullet, dmg, enemy.position, lines, Patterns.AimAt(enemy.position, GameManager.playerPosition), speed),
            shotRate);


    }
    public static IEnumerator PulsingBulletsRandomAngle(Bullet bullet, float dmg, Transform enemy, float speed, float shotRate, int lines) {
        return Functions.RepeatAction(() => Patterns.RingOfBullets(bullet, dmg, enemy.position, lines, UnityEngine.Random.Range(0f, 360f), speed),
            shotRate);
    }

    public static IEnumerator PulsingBulletsRandom(List<Bullet> bullets, float dmg, Transform enemy, float speed, float shotRate, int lines)
    {

        return Functions.RepeatAction(() => Patterns.RingOfBullets(bullets[UnityEngine.Random.Range(0, bullets.Count)], dmg, enemy.position, lines, 
            Patterns.AimAt(enemy.position, GameManager.playerPosition), speed),
            shotRate);
    }

    public static IEnumerator CustomSpinningCustomBulletsCustomSpacing(Func<float,Bullet> bulFunction, Func<int, float> spacingFunction, Func<float, float> spinningFunction, int lines, float shotRate)
    {
        return Functions.RepeatCustomAction(
            i => Patterns.CustomRingWithCustomSpacing(angle => bulFunction(angle),
            spacingFunction, spinningFunction(i * shotRate), lines), shotRate);


    }
    public static IEnumerator CustomSpinningStraightBulletsCustomSpacing(Bullet bullet, float dmg, Transform enemy, float speed, Func<int,float> spacingFunction, Func<float, float> spinningFunction, int lines, float shotRate) {
        return CustomSpinningCustomBulletsCustomSpacing(angle => Patterns.ShootStraight(bullet, dmg, enemy.position, angle, speed),
            spacingFunction, spinningFunction, lines, shotRate);

    
    }

    public static IEnumerator CustomSpinningStraightBullets(Bullet bullet, float dmg, Transform enemy, float speed, Func<float, float> spinningFunction, int lines, float shotRate) {
        return CustomSpinningStraightBulletsCustomSpacing(bullet, dmg, enemy, speed, i => 360f * i / lines, spinningFunction, lines, shotRate);
    }

    public static IEnumerator ConstantSpinningStraightBullets(Bullet bullet, float dmg, Transform enemy, float speed, float angularVel, float start, int lines, float shotRate) {
        return CustomSpinningStraightBullets(bullet, dmg, enemy, speed, time => start + angularVel * time, lines, shotRate);
    }
    public static IEnumerator BorderOfWaveAndParticle(Bullet bullet, float dmg, Transform enemy, float speed, float shotRate, int lines, float angularVel)
    {

        return CustomSpinningStraightBullets(bullet, dmg, enemy, speed, time => (float)(180 * Math.Sin(time * angularVel)), lines, shotRate);

    }

    public static void StartFanningPattern(Bullet bullet, float dmg, Shooting enemy, float speed, float angularVel, float start, int lines, float shotRate, int number, float speedDiff) {
        Functions.StartMultipleCustomCoroutines(enemy, i => ConstantSpinningStraightBullets(bullet, dmg, enemy.transform, speed + i * speedDiff, angularVel, start, lines, shotRate), number);
    }

    public static Bullet SummonMagicCircle(Bullet magicCircle, float dmg, Transform enemy, float timeToRadius, float angle, float radius, float rotationSpeed) {
        Bullet bul = Patterns.ShootCustomBullet(magicCircle, dmg, enemy.position, Movement.RotatePath(
             angle, t => new Polar(t > timeToRadius ? radius : radius * t / timeToRadius, rotationSpeed * t).rect), MovementMode.Position);
        bul.transform.SetParent(enemy);
        return bul;
    }

    public static Bullet OutAndSpinBullet(Bullet bul, float dmg, Transform origin, float initialSpeed, float radius, float radialSpeed2, float angularVel, float delay, float initialAngle) {
        return Patterns.ShootCustomBullet(bul, dmg, origin.position,
            t => new Polar(t < radius / initialSpeed ? initialSpeed * t : t < radius / initialSpeed + delay ? radius : radius + radialSpeed2 * (t - delay - radius / initialSpeed),
            initialAngle + (t < radius / initialSpeed + delay ? 0 : angularVel * (t - radius / initialSpeed - delay))).rect, MovementMode.Position);
    }

    public static List<Bullet> OutAndSpinRingOfBullets(Bullet bul, float dmg, Transform origin, float intialSpeed, float radius, float radialSpeed2, float angularVel, float delay, float offset, int lines) {
        return Patterns.CustomRing(angle => OutAndSpinBullet(bul, dmg, origin, intialSpeed, radius, radialSpeed2, angularVel, delay, angle), offset, lines);
    }
    public static IEnumerator ShootSine(Bullet bullet, float dmg, Transform enemy, float angle, float speed, float shotRate, float angularVel, float amp)
    {
        
         return Functions.RepeatAction(()=>Patterns.ShootSinTrajectory(bullet, dmg, enemy.position, angle, speed, angularVel, amp), shotRate);
        
    }


    public static IEnumerator ShootLaserBeam(Bullet actualLaser, Bullet warningLaser, Transform enemy, float angle, float timeWarning, float duration) {
        Bullet warning = Instantiate(warningLaser, enemy.position, Quaternion.Euler(0, 0, angle), enemy);
        warning.orientation.SetFixedOrientation(Quaternion.Euler(0, 0, angle));
        yield return new WaitForSeconds(timeWarning);
        Destroy(warning.gameObject);
        Bullet actual = Instantiate(actualLaser, enemy.position, Quaternion.Euler(0, 0, angle), enemy);
        actual.orientation.SetFixedOrientation(Quaternion.Euler(0, 0, angle));
        yield return new WaitForSeconds(duration);
        
        Destroy(actual.gameObject);


    }
    public static IEnumerator ConePattern(Bullet bullet, float dmg, Transform spawner, float angle, float speed, float spawnRate, int number, float spacing)
    {
       
        return Functions.RepeatCustomActionXTimes(i => {
            Vector2 start = spawner.position - Quaternion.Euler(0, 0, angle) * new Vector2(0, (spacing * i) / 2);
            for (int j = 0; j < i + 1; j++)
            {
                Bullet bul = Instantiate(bullet, start + (Vector2)(Quaternion.Euler(0, 0, angle) * new Vector2(0, spacing * j)), Quaternion.identity);
                bul.SetDamage(dmg);
                bul.movement.SetSpeed(Quaternion.Euler(0, 0, angle) * new Vector2(speed, 0));
            }

        }, spawnRate, number);


      




    }
}
