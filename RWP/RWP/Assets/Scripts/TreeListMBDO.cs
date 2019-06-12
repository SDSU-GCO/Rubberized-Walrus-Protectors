using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine.Events;

public class TreeListMBDO : MBDataObject
{
    public List<SaveTree> trees = new List<SaveTree>();
    public UnityEvent update;

    [Button]
    private void CureAllTrees()
    {
        SaveTree[] treesArray = trees.ToArray();
        foreach (SaveTree saveTree in treesArray)
        {
            saveTree.CureTree();
            update.Invoke();
        }
    }
}