using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Script : MonoBehaviour
{
    public TextMeshProUGUI healthCounter;
    public TextMeshProUGUI trees;
    public TextMeshProUGUI enemies;
    public TreeListSO treeCounter;
    public EnemyListSO enemyCounter;
    public Entity_Logic entityLogic;
    public Canvas PauseMenu;
    public GameObject Druid;
    float health;
    public Canvas gameOverScreen;
    IEnumerator coroutine;
    int MaxTreeCount = 0;
    int MaxEnemyCount = 0;
    Color tint = new Color(1.0f, 0, 1.0f, 1.0f);

    private void Awake()
    {
        treeCounter.update.AddListener(updateTrees);
        enemyCounter.update.AddListener(updateEnemies);
    }

    private void OnDestroy()
    {
        treeCounter.update.RemoveListener(updateTrees);
        enemyCounter.update.RemoveListener(updateEnemies);
    }

    public void updateEnemies()
    {
        if (enemyCounter.enemies.Count > MaxEnemyCount)
        {
            MaxEnemyCount = enemyCounter.enemies.Count;
        }
        enemies.SetText("Trees Left: " + enemyCounter.enemies.Count);
        Color temp = Color.Lerp(tint, Color.white, enemyCounter.enemies.Count / (MaxTreeCount + MaxEnemyCount));
    }

    public void updateTrees()
    {
        if(treeCounter.trees.Count>MaxTreeCount)
        {
            MaxTreeCount = treeCounter.trees.Count;
        }
        trees.SetText("Trees Left: " + treeCounter.trees.Count);
        Color temp = Color.Lerp(tint, Color.white, treeCounter.trees.Count / (MaxTreeCount+MaxEnemyCount));
    }
    

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
        updateEnemies();
        updateTrees();
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
