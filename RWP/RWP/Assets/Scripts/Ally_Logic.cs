using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Logic : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    Entity_Logic entityLogic;
    public SpriteRenderer spriteRenderer;
    public Animator MainStage;
    public Movement movement;
    public void setToAttack()
    {
        MainStage.SetInteger("MainStage", 3);
    }
    public void setToJump()
    {
        MainStage.SetInteger("MainStage", 1);
    }

    private void Awake()
    {
        
        
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        MainStage = GetComponent<Animator>();
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    void Update()
    {
        if (rigidbody2D.velocity.x < -0.00001)
            spriteRenderer.flipX = false;
        if (rigidbody2D.velocity.x > 0.000001)
            spriteRenderer.flipX = true;

        if(MainStage.GetInteger("MainStage")==3)
            MainStage.SetInteger("MainStage", 0);

        if(!(Input.GetAxis("Vertical") > 0 && movement.airbornTime <= movement.allowedAirbornTime))
        {
            if (MainStage.GetInteger("MainStage") == 1)
                MainStage.SetInteger("MainStage", 0);
        }


        if (Input.GetMouseButton(1))
        {
            entityLogic.doAttack();
        }

        
    }
}
