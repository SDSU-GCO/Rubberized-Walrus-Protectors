using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRefMBDO : MBDataObject
{
    public Transform player = null;
    public UnityEvent update;

    public void Log(string str)
    {
        Debug.Log(str);
    }
}