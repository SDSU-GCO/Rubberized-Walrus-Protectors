using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
