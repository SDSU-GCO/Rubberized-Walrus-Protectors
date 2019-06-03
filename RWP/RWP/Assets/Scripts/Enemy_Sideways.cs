using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy_Sideways : MonoBehaviour
{
    public List<Transform> path;
    public float moveDelay = 3f;
    new Rigidbody2D rigidbody2D;
    private Vector2 velocity;
    public float moveVelocity = 5f;
    public float timePassed;
    public float idle=6f;

    public AnimationCurve animationCurve = new AnimationCurve();
    float pathProgress;
    Transform nextTarget;
    int pathIndex = 0;
    int nextPathIndex = 1;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (path[pathIndex].position.x < path[nextPathIndex].position.x)
            spriteRenderer.flipX = false;
        if (path[pathIndex].position.x > path[nextPathIndex].position.x)
            spriteRenderer.flipX = true;
        Vector2 temp = Vector2.Lerp(path[pathIndex].position, path[nextPathIndex].position, animationCurve.Evaluate(pathProgress));
        
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
