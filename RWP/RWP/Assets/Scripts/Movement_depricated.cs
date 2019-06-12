using UnityEngine;

public class Movement_deprecated : MonoBehaviour
{
    //speed variables
    public float moveSpeed = 1;
    public float jumpForce = 1;
    public float maxSpeed = 3;
    public float drag;
    public float gravity = 1;

    //reference variables
    private Vector2 playerInput;
    private Rigidbody2D thisRB;
    private Vector2 acceleration = Vector2.zero;

    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        if (thisRB == null)
        {
            thisRB = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        //jump
        if (Input.GetAxis("Vertical") > 0)
        {
            acceleration.y += jumpForce;
        }

        acceleration.y -= Time.deltaTime * gravity;

        //movement
        acceleration.x = Input.GetAxis("Horizontal");

        Debug.Log(velocity);

        velocity = velocity + (acceleration * Time.deltaTime);
        if (velocity.x > 0)
        {
            velocity.x -= ((velocity.x + 1) * (drag * Time.deltaTime));
            velocity.x = Mathf.Min(0, velocity.x);
        }
        else
        {
            velocity.x -= ((velocity.x - 1) * (drag * Time.deltaTime));
            velocity.x = Mathf.Max(0, velocity.x);
        }

        if (velocity.y > 0)
        {
            velocity.y -= ((velocity.y + 1) * (drag * Time.deltaTime));
            velocity.y = Mathf.Min(0, velocity.y);
        }
        else
        {
            velocity.y -= ((velocity.y - 1) * (drag * Time.deltaTime));
            velocity.y = Mathf.Max(0, velocity.y);
        }

        Debug.Log(velocity);
        //speed cap
        if (velocity.magnitude > maxSpeed)
        {
            float factor = maxSpeed / velocity.magnitude;
            Debug.Log(factor);
            velocity.x *= factor;
            velocity.y *= factor;
        }
        Debug.Log(velocity);

        //normalize velocity
        thisRB.MovePosition((velocity * Time.deltaTime) + thisRB.position);
    }
}