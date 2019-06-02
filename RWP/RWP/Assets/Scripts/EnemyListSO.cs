using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EnemyListSO", menuName = "Scriptable Objects/CreateEnemyListSO")]
public class EnemyListSO : ScriptableObject
{
    public List<Enemy_Logic> enemies = new List<Enemy_Logic>();
    public UnityEvent update;
}

