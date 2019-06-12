using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class TreeListMBDO : MBDataObject
{
    public List<SaveTree> trees = new List<SaveTree>();
    public UnityEvent update;

    [Button]
    void CureAllTrees()
    {
        SaveTree[] treesArray = trees.ToArray();
        foreach(SaveTree saveTree in treesArray)
        {
            saveTree.CureTree();
            update.Invoke();
        }
    }

} 
