using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New TreeListSO", menuName = "Scriptable Objects/CreateTreeListSO")]
public class TreeListSO : ScriptableObject
{
    public List<SaveTree> trees = new List<SaveTree>();
    private void Awake()
    {
        trees = new List<SaveTree>();
    }
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
    public UnityEvent update;

}
