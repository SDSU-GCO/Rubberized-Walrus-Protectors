using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTree : MonoBehaviour
{
    public Animator animator;
    public TreeListSO treeListSO;
    bool saved = false;

    private void OnEnable()
    {
        treeListSO.trees.Add(this);
        treeListSO.update.Invoke();
    }
    private void OnDisable()
    {
        if(saved!=true)
        {
            treeListSO.trees.Remove(this);
            treeListSO.update.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            saved = true;
            animator.SetBool("SaveTree", true);
            treeListSO.trees.Remove(this);
            treeListSO.update.Invoke();
        }
    }
}
