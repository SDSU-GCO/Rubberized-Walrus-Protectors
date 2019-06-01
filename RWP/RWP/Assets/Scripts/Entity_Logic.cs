using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity_Logic : MonoBehaviour
{
    public Event_One_Float damaged = new Event_One_Float();
    //entity parameters
    public float health = 3f;
    public Attack_Controller rangedAttack;
    public float rangedCoolDownInSeconds;
    float rangedCoolDownInSecondsDefault;
    Rigidbody2D my2DRigidbody;
    public float offset = 1.5f;
    public int damage;
    

    //initialize ambiguous parameters
    public void Awake()
    {

        rangedCoolDownInSeconds = rangedAttack.AttackDelay;
        rangedCoolDownInSecondsDefault = rangedCoolDownInSeconds;
        my2DRigidbody = GetComponent<Rigidbody2D>();
        damage = rangedAttack.damage;
        
    }

    void OnEnable()
    {
        Debug.Log(damaged);
        damaged.Invoke(health);
    }

    
    void Update()
    {
        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);
        if (Input.GetMouseButtonDown(1) && rangedCoolDownInSeconds == 0)
        {
           FireRangedAttack();
           rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
        }

    }

    


    //shoot projectile
    void FireRangedAttack()
    {

        if (rangedCoolDownInSeconds == 0)
        {


            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);


            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseposition = (mouseposition - (Vector2)transform.position).normalized * offset;



            Vector3 shoot = (new Vector3(mouseposition.x, mouseposition.y, 0) - transform.position).normalized;
            GameObject childInstance = Instantiate(rangedAttack.gameObject, mouseposition + (Vector2)transform.position, transform.rotation);
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * mouseposition.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;

        }

    }
    //take damage function
    public void TakeDamage(float amount)
    {
        health -= amount;
        damaged.Invoke(health);
        if (health <= 0)
            Destroy(gameObject);
    }
    
}
[System.Serializable]
public class Event_One_Float : UnityEvent<float>
{

}