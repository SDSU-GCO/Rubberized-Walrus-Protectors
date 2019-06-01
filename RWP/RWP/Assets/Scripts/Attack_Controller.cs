using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Controller : MonoBehaviour
{

    [SerializeField]
    float timeToLive = 0.5f;//in seconds
    [SerializeField]
    private bool LiveForever = false;
    float originalTimeToLive;//in seconds
    public float AttackDelay = 0.75f;
    public bool attackingFriendlies = false;
    public int damage;
    public float speed = 7;
    new Collider collider = null;


    private void Awake()
    {
        originalTimeToLive = timeToLive;
        collider = GetComponent<Collider>();
        if (collider != null && collider.isTrigger == false && LiveForever == false)
            collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LiveForever != true)
            timeToLive -= Time.deltaTime;
        if (collider != null && timeToLive < originalTimeToLive / 2.0f)
        {
            collider.enabled = true;
        }
        if (timeToLive <= 0)
            Destroy(gameObject);

    }
    //enemy/ally check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity_Logic>().TakeDamage(damage);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Entity_Logic>().TakeDamage(damage);
        }
        Destroy(gameObject);

    }
    
    }



