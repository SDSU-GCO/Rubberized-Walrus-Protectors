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
    public Transform playerPosition;
    
    delegate void attackMethod();
    attackMethod attack_method;
    //initialize ambiguous parameters
    public void Awake()
    {

        rangedCoolDownInSeconds = rangedAttack.AttackDelay;
        rangedCoolDownInSecondsDefault = rangedCoolDownInSeconds;
        my2DRigidbody = GetComponent<Rigidbody2D>();
        damage = rangedAttack.damage;

        if (gameObject.layer == 10)
        {
            attack_method = PlayerRangedAttack;
        }
        else if (gameObject.layer == 11)
        {
            attack_method = EnemyRangedAttack;
        }
        
    }

    void OnEnable()
    {
        Debug.Log(damaged);
        damaged.Invoke(health);
    }

    
    void Update()
    {
        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);
        

    }

    public void doAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            attack_method();
            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
        }
    }

   
    //shoot player projectile
    void PlayerRangedAttack()
    {

        if (rangedCoolDownInSeconds == 0)
        {
            

            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);


            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseposition = (mouseposition - (Vector2)transform.position).normalized * offset;



            
            GameObject childInstance = Instantiate(rangedAttack.gameObject, mouseposition + (Vector2)transform.position, transform.rotation);
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * mouseposition.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;

        }

    }
    //shoot enemy projectile
    void EnemyRangedAttack()
    {

        if (rangedCoolDownInSeconds == 0)
        {

            Vector2 direction = new Vector2();

            direction = ((Vector2)playerPosition.position - (Vector2)transform.position).normalized * offset;

            float rotation = Mathf.Rad2Deg * (Mathf.Atan(direction.y / direction.x));
            rotation += -90;
            if (direction.x < 0)
            {
                rotation += 180;
            }

            

            GameObject childInstance = Instantiate(rangedAttack.gameObject, direction + (Vector2)transform.position, Quaternion.Euler(0, 0, rotation));
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * direction.normalized;

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