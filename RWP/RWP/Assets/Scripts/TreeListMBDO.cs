using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeListMBDO : MBDataObject
{
    public List<SaveTree> trees = new List<SaveTree>();
    public UnityEvent update;

} 
