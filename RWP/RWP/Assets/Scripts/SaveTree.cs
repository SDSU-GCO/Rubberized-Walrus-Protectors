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
        GameObject cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
        MBDatabaseObjectReferences mbDatabaseObjectReferences = null;
        if (cardinalSubsystem != null)
            mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDatabaseObjectReferences>();
            
        if(cardinalSubsystem !=null && cardinalSubsystem.scene != gameObject.scene)
        {
            treeListMBDO = null;
        }
        if (treeListMBDO == null && cardinalSubsystem.scene == gameObject.scene)
        {
            if (mbDatabaseObjectReferences != null)
                mbDatabaseObjectReferences.tryPopulate(out treeListMBDO);
            if (treeListMBDO == null)
                Debug.LogWarning("ScriptableObject Object treeListMBDO: " + treeListMBDO + "is null in: " + this);
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
        if (collision.gameObject.layer == 10)
        {
            saved = true;
            animator.SetBool("SaveTree", true);
            treeListMBDO.trees.Remove(this);
            treeListMBDO.update.Invoke();
        }
    }
}
