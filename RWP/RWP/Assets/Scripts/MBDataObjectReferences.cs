using System.Collections.Generic;
using UnityEngine;

public class MBDataObjectReferences : MonoBehaviour
{
    [SerializeField]
    public List<MBDataObject> mbDataObjects = new List<MBDataObject>();

    public void TryPopulate<T>(out T mbdo) where T : MBDataObject
    {
        mbdo = null;
        foreach (MBDataObject mbDataObject in mbDataObjects)
        {
            if (mbDataObject is T)
            {
                mbdo = (T)mbDataObject;
            }
        }
    }

    public T GetMBDataObjectOfType<T>() where T : MBDataObject
    {
        foreach (MBDataObject mbDataObject in mbDataObjects)
        {
            if (mbDataObject is T)
            {
                return (T)mbDataObject;
            }
        }
        return null;
    }

    public List<MBDataObject> GetMBDataObjectsOfType<T>()
    {
        List<MBDataObject> tempList = new List<MBDataObject>();
        foreach (MBDataObject mbDataObject in mbDataObjects)
        {
            if (mbDataObject is T)
            {
                tempList.Add(mbDataObject);
            }
        }
        return tempList;
    }
}