﻿using System;
//using System.Numerics;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public enum MovementMode { Position, Velocity, Acceleration, Homing }
public class Movement : MonoBehaviour
{
 

    Func<float, Vector2> graph = time => new Vector2(0,0);
    [HideInInspector]public Vector2 currentVelocity;
    GameObject target;
    float homingSpeed;
    float time = 0;
    bool destroyWhenOut = true;
    bool moving = true;
    MovementMode mode = MovementMode.Velocity;


    // Start is called before the first frame update
 

    public void SetSpeed(Vector2 vel) {
        mode = MovementMode.Velocity;
        graph = time => vel;
        ResetTimer();
    }

    public void Homing(GameObject tar, float spd) {
        mode = MovementMode.Homing;
        target = tar;
        homingSpeed = spd;
        currentVelocity = ((Vector2)(target.transform.position - transform.position)).normalized * homingSpeed;
    
    }

    public void ResetTimer() {
        time = 0;
    }

    public float MoveTo(Vector2 end, float speed) {
        StartMoving();
        Vector2 diff = end - (Vector2)transform.position;
        float timeTaken = diff.magnitude / speed;
        SetSpeed(diff / timeTaken);
        StopMovingAfter(timeTaken);

        return timeTaken;

    }

    public void SetCustomGraph(Func<float, Vector2> traj, MovementMode movementMode) {
        mode = movementMode;
        graph = traj;
    
    }

   
    public bool IsMoving() {
        return moving;
    }
    public void StartMoving() {
        moving = true;
    }

    public void StopMoving() {
        moving = false;
    }
    public void StopMovingAfter(float sec) {
        Invoke("StopMoving", sec);
        
    }
    public void StartMovingAfter(float sec) {
        Invoke("StartMoving", sec);
    }
    public void SetCustomPath(Func<float, Vector2> traj) {
        ResetTimer();
        mode = MovementMode.Position;
        graph = traj;
       
    }
    public void SetDestroyWhenOut(bool bl) {
        destroyWhenOut = bl;
    }

    public void RotateTrajectory(float angle) {
        graph = RotatePath(angle, graph);

    }
    public void SetPolarPath(Func<float, Polar> traj) {
        mode = MovementMode.Position;
        graph = t => traj(t).rect;
    }
    public Func<float, Vector2> RotatePath(float angle, Func<float, Vector2> oldTraj) {
        return t => Quaternion.Euler(0, 0, angle) * oldTraj(t);       
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (moving)
        {
            if (mode == MovementMode.Position)
            {
                currentVelocity = (graph(time + Time.deltaTime) - graph(time)) / Time.deltaTime;

            }
            else if (mode == MovementMode.Velocity)
            {
                currentVelocity = graph(time);

            }
            else if (mode == MovementMode.Acceleration){
                currentVelocity += graph(time) * Time.deltaTime;
            
            }
            else{
                if (!target)
                {
                    SetSpeed(currentVelocity);
                }
                else {
                    currentVelocity = ((Vector2)(target.transform.position - transform.position)).normalized * homingSpeed;
                
                }
            
            }

            
            time += Time.deltaTime;
            transform.position += (Vector3)currentVelocity * Time.deltaTime;
            if (destroyWhenOut)
            { OutOfBound(); }
        }
        

    }

    

    public void OutOfBound()
    {


        if (transform.position.x < -6 || transform.position.x > 6 || transform.position.y < -6 || transform.position.y > 6)
        {
            Destroy(gameObject);
        }
    }


}
