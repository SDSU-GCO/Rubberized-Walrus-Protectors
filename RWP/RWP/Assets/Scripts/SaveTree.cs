using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTree : MonoBehaviour
{
    public Animator animator;
    public TreeListMBDO treeListMBDO;
    bool saved = false;

    private void OnValidate()
    {
        if (treeListMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref treeListMBDO);
        }
    }

    private void OnEnable()
    {
        treeListMBDO.trees.Add(this);
        treeListMBDO.update.Invoke();
    }
    private void OnDisable() 
    {
        if(saved!=true)
        {
            treeListMBDO.trees.Remove(this);
            treeListMBDO.update.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LayerMask hitLayer = (1 << collision.gameObject.layer);

        LayerMask targetLayer = 
            (
            (1 << ((int)CustomGCOTypes.CollisionLayerKey.ally))
            |  (1 << ((int)CustomGCOTypes.CollisionLayerKey.allyAttack))
            );

        if ((hitLayer & targetLayer)!=0)
        {
            CureTree();
            treeListMBDO.update.Invoke();
        }
    }

    public void CureTree()
    {
        if (saved == false)
        {
            saved = true;
            animator.SetBool("SaveTree", true);
            treeListMBDO.trees.Remove(this);
        }
    }
}
