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

        if(treeListMBDO == null)
        {
            GameObject cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
            MBDataObjectReferences mbDatabaseObjectReferences = null;
            if (cardinalSubsystem != null)
                mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDataObjectReferences>();

            if (cardinalSubsystem != null && cardinalSubsystem != null && cardinalSubsystem.scene != gameObject.scene)
            {
                treeListMBDO = null;
            }
            if (treeListMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
            {
                if (mbDatabaseObjectReferences != null)
                    mbDatabaseObjectReferences.tryPopulate(out treeListMBDO);
                if (treeListMBDO == null)
                    Debug.LogWarning("ScriptableObject Object treeListMBDO: " + treeListMBDO + "is null in: " + this);
            }
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
        if (collision.gameObject.layer == (int)CustomGCOTypes.CollisionLayerKey.ally || collision.gameObject.layer == (int)CustomGCOTypes.CollisionLayerKey.allyAttack)
        {
            if(saved==false)
            {
                saved = true;
                animator.SetBool("SaveTree", true);
                treeListMBDO.trees.Remove(this);
                treeListMBDO.update.Invoke();
            }
        }
    }
}
