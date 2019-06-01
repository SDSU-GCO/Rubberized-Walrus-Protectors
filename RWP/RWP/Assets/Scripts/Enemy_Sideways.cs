﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    public List<Transform> path;
    public float moveDelay = 3f;
    new Rigidbody2D rigidbody2D;
    private Vector2 velocity;
    public float moveVelocity = 5f;
    public float timePassed;
    bool switchDirection=false;
    public float idle=6f;
    public AnimationCurve animationCurve = new AnimationCurve();
    float pathProgress;
    Transform nextTarget;
    int pathIndex = 0;
    int nextPathIndex = 1;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        pathIndex = 0;
        nextPathIndex = 1;
        Debug.Assert(path.Count >= 2, "Error: path must contain at least two points!");
    }
    private void Update()
    {
        //move
        pathProgress += Time.deltaTime;
        while (pathProgress >= 1)
        {
            pathProgress -= 1;
            autoIncrement(ref pathIndex);
            autoIncrement(ref nextPathIndex);
        }
        Vector2 temp = Vector2.Lerp(path[pathIndex].position, path[nextPathIndex].position, animationCurve.Evaluate(pathProgress));

        Debug.Log(path[pathIndex].position);
        rigidbody2D.MovePosition(temp);


        //if (switchDirection == true && idle >= (moveDelay*2))
        //{
        //    moveVelocity = -moveVelocity;
        //    idle = 0;
        //}
        //else
        //{
        //    rigidbody2D.velocity += Vector2.left*0;
        //}

        //if (timePassed >= moveDelay && rigidbody2D.velocity.x <= 0.0000001)
        //{
        //    rigidbody2D.velocity += Vector2.left* moveVelocity;
        //    timePassed = 0;
            
        //    switchDirection = true;
            
        //}
        //else
        //{
        //    switchDirection = false;
        //}

        //timePassed += Time.deltaTime;
        //idle += Time.deltaTime;
        
    }

    private void autoIncrement(ref int pathIndex)
    {
        pathIndex++;
        if (pathIndex >= path.Count)
            pathIndex = 0;
    }
}