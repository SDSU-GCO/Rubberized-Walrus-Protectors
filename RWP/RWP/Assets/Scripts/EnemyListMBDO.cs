using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyListMBDO : MBDataObject
{
    public List<Enemy_Logic> enemies = new List<Enemy_Logic>();
    public UnityEvent update;

}

