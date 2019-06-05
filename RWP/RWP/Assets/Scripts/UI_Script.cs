using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class UI_Script : MonoBehaviour
{
    public TextMeshProUGUI healthCounter;
    public TextMeshProUGUI trees;
    public TextMeshProUGUI enemies;
    public TreeListSO treeCounter;
    public EnemyListSO enemyCounter;
    public Entity_Logic entityLogic;
    public Canvas PauseMenu;
    float health;
    public Canvas gameOverScreen;
    int MaxTreeCount = 0;
    int MaxEnemyCount = 0;
    
    public Color tint = new Color(1.0f, 0, 1.0f, 1.0f);
    public List<Tilemap> tilemaps = new List<Tilemap>();

    private void Start()
    {
        treeCounter.update.Invoke();
        enemyCounter.update.Invoke();
        updateColor();
    }


    void updateColor()
    {
        Debug.Log("Color Update");
        float t = (treeCounter.trees.Count + enemyCounter.enemies.Count) / (float)(MaxTreeCount + MaxEnemyCount);
        Debug.Log(t);
        Color temp = Color.Lerp(tint, Color.white, 1-t);

        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap!=null)
                tilemap.color = temp;
        }
    }

    public void updateEnemies()
    {
        if (enemyCounter.enemies.Count > MaxEnemyCount)
        {
            MaxEnemyCount = enemyCounter.enemies.Count;
        }
        enemies.SetText("Enemies Left: " + enemyCounter.enemies.Count);
        updateColor();
    }

    public void updateTrees()
    {
        if(treeCounter.trees.Count>MaxTreeCount)
        {
            MaxTreeCount = treeCounter.trees.Count;
        }
        trees.SetText("Trees Left: " + treeCounter.trees.Count);

        updateColor();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.gameObject.activeInHierarchy == false)
            {
                PauseMenu.gameObject.SetActive(true);
                if(gameOverScreen.gameObject.activeInHierarchy!=true)
                {
                    Time.timeScale = 0;
                    Time.fixedDeltaTime = 0;
                }
            }
            else
            {
                PauseMenu.gameObject.SetActive(false);
                if (gameOverScreen.gameObject.activeInHierarchy != true)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f;
                }
            }
        }
    }

    private void OnEnable()
    {
        treeCounter.update.AddListener(updateTrees);
        enemyCounter.update.AddListener(updateEnemies);
        entityLogic.damaged.AddListener(updateHealth);
        updateEnemies();
        updateTrees();
    }

    private void OnDisable()
    {
        treeCounter.update.RemoveListener(updateTrees);
        enemyCounter.update.RemoveListener(updateEnemies);
        entityLogic.damaged.RemoveListener(updateHealth);
    }

    public void updateHealth(float health)
    {
        healthCounter.SetText("HP: " + health);
        if (health <= 0)
        {
            trees.enabled = false;
            enemies.enabled = false;
            healthCounter.enabled = false;
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
