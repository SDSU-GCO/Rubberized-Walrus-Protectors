using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy_Sideways : MonoBehaviour
{
    public float interpolationRate = 0.5f;
    public AnimationCurve animationCurve = new AnimationCurve();
    [ReorderableList]
    public List<Transform> path;

    private new Rigidbody2D rigidbody2D;
    private float pathProgress;
#pragma warning disable IDE0044 // Add readonly modifier
    private Transform nextTarget;
#pragma warning restore IDE0044 // Add readonly modifier
    private int pathIndex = 0;
    private int nextPathIndex = 1;

    [SerializeField, HideInInspector]
    private SpriteRenderer spriteRenederer;

    private void OnValidate()
    {
        if (spriteRenederer == null)
        {
            spriteRenederer = GetComponent<SpriteRenderer>();
        }
    }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (spriteRenederer == null)
        {
            Debug.Log("Bootstrapping");
            spriteRenederer = GetComponent<SpriteRenderer>();
        }
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
        pathProgress += Time.deltaTime * interpolationRate;
        while (pathProgress >= 1)
        {
            pathProgress -= 1;
            AutoIncrement(ref pathIndex);
            AutoIncrement(ref nextPathIndex);
        }
        if (path[pathIndex].position.x < path[nextPathIndex].position.x)
        {
            spriteRenederer.flipX = false;
        }

        if (path[pathIndex].position.x > path[nextPathIndex].position.x)
        {
            spriteRenederer.flipX = true;
        }

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

    private void AutoIncrement(ref int pathIndex)
    {
        pathIndex++;
        if (pathIndex >= path.Count)
        {
            pathIndex = 0;
        }
    }
}