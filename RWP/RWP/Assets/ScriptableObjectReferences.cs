using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectReferences : MonoBehaviour
{
    [SerializeField]
    public List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();

    public void tryPopulate<T>(ref T so) where T : ScriptableObject
    {
        foreach (ScriptableObject scriptableObject in scriptableObjects)
        {
            if (scriptableObject is T)
                so = (T) scriptableObject;
        }
    }

    public T getScriptableObjectOfType<T>() where T : ScriptableObject
    {
        foreach (ScriptableObject scriptableObject in scriptableObjects)
        {
            if (scriptableObject is T)
                return (T) scriptableObject;
        }
        return null;
    }

    public List<ScriptableObject> getScriptableObjectsOfType<T>()
    {
        List<ScriptableObject> tempList = new List<ScriptableObject>();
        foreach (ScriptableObject scriptableObject in scriptableObjects)
        {
            if (scriptableObject is T)
                tempList.Add(scriptableObject);
        }
        return tempList;
    }
}
