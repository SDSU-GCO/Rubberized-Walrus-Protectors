using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Logic : MonoBehaviour
{
    Entity_Logic entityLogic;

    private void Awake()
    {
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            entityLogic.doAttack();
        }
    }
}
