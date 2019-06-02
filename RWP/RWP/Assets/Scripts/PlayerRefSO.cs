using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New PlayerRefSO", menuName = "Scriptable Objects/CreatePlayerRefSO")]
public class PlayerRefSO : ScriptableObject
{
    public Transform player = null;

}