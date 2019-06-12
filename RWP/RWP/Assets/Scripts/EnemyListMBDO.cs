using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EnemyListMBDO : MBDataObject
{
    public List<Enemy_Logic> enemies = new List<Enemy_Logic>();
    public UnityEvent update;

    [Button]
    public void CureAllEnemies()
    {
        Enemy_Logic[] enemy_Logics = enemies.ToArray();
        foreach(Enemy_Logic enemy_Logic in enemy_Logics)
        {
            Entity_Logic tmp = enemy_Logic.GetComponent<Entity_Logic>();
            tmp.health = 0;
            tmp.CommitSuduku();
        }
    }
}

