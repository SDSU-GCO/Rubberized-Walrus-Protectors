using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public TextMeshProUGUI healthCounter;
    public TextMeshProUGUI trees;
    public TextMeshProUGUI enemies;
    
    public TreeListSO treeCounter;
    public EnemyListSO enemyCounter;
    public Entity_Logic playerEntityLogic;

    public Canvas PauseMenu;
    float health;
    public Canvas gameOverScreen;
    int MaxTreeCount = 0;
    int MaxEnemyCount = 0;
    
    public Color tint = new Color(1.0f, 0, 1.0f, 1.0f);
    
    public List<Tilemap> tilemaps = new List<Tilemap>();
    public List<Image> images = new List<Image>();
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    
    private void OnValidate()
    {
        if (treeCounter == null || enemyCounter == null)
        {
            GameObject CardinalSubsystem = GameObject.Find("Cardinal Subsystem");
            ScriptableObjectReferences scriptableObjectReferences = CardinalSubsystem.GetComponent<ScriptableObjectReferences>();
            if (treeCounter == null)
            {
                scriptableObjectReferences.tryPopulate(ref treeCounter);
                if (treeCounter == null)
                    Debug.LogWarning("ScriptableObject Object treeCounter: " + treeCounter + "is null in: "+this);
            }
            if (enemyCounter == null)
            {
                scriptableObjectReferences.tryPopulate(ref enemyCounter);
                if (enemyCounter == null)
                    Debug.LogWarning("ScriptableObject Object enemyCounter: " + enemyCounter + "is null in: " + this);
            }
        }
    }

    private void Awake()
    {
        OnValidate();
    }

    private void Start()
    {
        treeCounter.update.Invoke();
        enemyCounter.update.Invoke();
        updateColor();
    }


    void updateColor()
    {
        float t = (treeCounter.trees.Count + enemyCounter.enemies.Count) / (float)(MaxTreeCount + MaxEnemyCount);
        Color temp = Color.Lerp(tint, Color.white, 1-t);

        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap!=null)
                tilemap.color = temp;
        }
        foreach (Image image in images)
        {
            if (image != null)
                image.color = temp;
        }
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = temp;
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
        playerEntityLogic.damaged.AddListener(updateHealth);
        updateEnemies();
        updateTrees();
    }

    private void OnDisable()
    {
        treeCounter.update.RemoveListener(updateTrees);
        enemyCounter.update.RemoveListener(updateEnemies);
        playerEntityLogic.damaged.RemoveListener(updateHealth);
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
