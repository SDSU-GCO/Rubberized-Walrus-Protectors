using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public TextMeshProUGUI healthCounter;
    public Entity_Logic entityLogic;
    float health;

    private void OnEnable()
    {
        entityLogic.damaged.AddListener(updateHealth);
    }

    private void OnDisable()
    {
        entityLogic.damaged.RemoveListener(updateHealth);
    }

    public void updateHealth(float health)
    {
        healthCounter.SetText("HP: " + health);
    }
}
