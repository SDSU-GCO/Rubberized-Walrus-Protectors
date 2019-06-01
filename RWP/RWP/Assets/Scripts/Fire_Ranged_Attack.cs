using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Ranged_Attack : MonoBehaviour
{
    /*public float rangedCoolDownInSeconds;
    public float rangedCoolDownInSecondsDefault;
    public float offset = 1.5f;

    // Update is called once per frame
    void Update()
    {
        
    }

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

    }*/

}
