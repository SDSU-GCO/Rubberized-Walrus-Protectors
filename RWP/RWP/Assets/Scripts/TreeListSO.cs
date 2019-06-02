using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New TreeListSO", menuName = "Scriptable Objects/CreateTreeListSO")]
public class TreeListSO : ScriptableObject
{
    public List<SaveTree> trees = new List<SaveTree>();
    public UnityEvent update;

}
