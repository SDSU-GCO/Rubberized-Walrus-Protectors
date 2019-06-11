using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGCOTypes : MonoBehaviour
{
    [ExtendEnum]
    public CollisionLayerKey collisionLayerKey;
    public enum CollisionLayerKey
    {
        platforms = 8,
        solidGround = 9,
        ally = 10,
        enemy = 11,
        npc = 12,
        enemyAttack = 13,
        allyAttack = 14,
        npcAttack = 14,
    }
}
