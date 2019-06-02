using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTree : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==10)
            animator.SetBool("SaveTree", true);
    }
}
