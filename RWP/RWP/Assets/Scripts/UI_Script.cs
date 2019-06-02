using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Script : MonoBehaviour
{
    public TextMeshProUGUI healthCounter;
    public Entity_Logic entityLogic;
    public Canvas PauseMenu;
    float health;
    public Canvas gameOverScreen;
    IEnumerator coroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.gameObject.activeInHierarchy == false)
            {
                PauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
            }
            else
            {
                PauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f;
            }

        }
        
    }

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
        {
            gameOverScreen.gameObject.SetActive(true);
            coroutine = WaitAndPrint(3.0f);
            StartCoroutine(coroutine);
            
        }
    }
    private IEnumerator WaitAndPrint(float waitTime)
    {
        float elapsedTime=0f;
        bool running = true;
        while (running)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
                running = false;

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
