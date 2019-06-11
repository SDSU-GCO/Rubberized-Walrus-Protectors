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
    
    public TreeListMBDO treeListMBDO;
    public EnemyListMBDO enemyListMBDO;
    public PlayerRefMBDO playerRefMBDO;

    public Entity_Logic hpEntityLogic;

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
        GameObject cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
        MBDatabaseObjectReferences mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDatabaseObjectReferences>();
        if (cardinalSubsystem != null && cardinalSubsystem.scene != gameObject.scene)
        {
            treeListMBDO = null;
            enemyListMBDO = null;
        }
        if (treeListMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            mbDatabaseObjectReferences.tryPopulate(out treeListMBDO);
            if (treeListMBDO == null)
                Debug.LogWarning("ScriptableObject Object treeCounter: " + treeListMBDO + "is null in: " + this);
        }
        if (playerRefMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            mbDatabaseObjectReferences.tryPopulate(out playerRefMBDO);
            if (playerRefMBDO == null)
                Debug.LogWarning("ScriptableObject Object playerRefMBDO: " + playerRefMBDO + "is null in: " + this);
        }
        if (enemyListMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            mbDatabaseObjectReferences.tryPopulate(out enemyListMBDO);
            if (enemyListMBDO == null)
                Debug.LogWarning("ScriptableObject Object enemyCounter: " + enemyListMBDO + "is null in: " + this);
        }
    }

    private void Awake()
    {
        OnValidate();
    }

    private void Start()
    {
        treeListMBDO.update.Invoke();
        enemyListMBDO.update.Invoke();
        playerRefMBDO.update.Invoke();
        updateColor();
    }


    void updateColor()
    {
        float t = (treeListMBDO.trees.Count + enemyListMBDO.enemies.Count) / (float)(MaxTreeCount + MaxEnemyCount);
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
        if (enemyListMBDO.enemies.Count > MaxEnemyCount)
        {
            MaxEnemyCount = enemyListMBDO.enemies.Count;
        }
        enemies.SetText("Enemies Left: " + enemyListMBDO.enemies.Count);
        updateColor();
    }

    public void updateTrees()
    {
        if(treeListMBDO.trees.Count>MaxTreeCount)
        {
            MaxTreeCount = treeListMBDO.trees.Count;
        }
        trees.SetText("Trees Left: " + treeListMBDO.trees.Count);

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

    void updateHPTracker()
    {
        if(hpEntityLogic==null)
            hpEntityLogic = playerRefMBDO.GetComponent<Entity_Logic>();
        if (hpEntityLogic != null)
            hpEntityLogic.hpUpdated.AddListener(updateHealth);
    }

    private void OnEnable()
    {
        treeListMBDO.update.AddListener(updateTrees);
        enemyListMBDO.update.AddListener(updateEnemies);
        playerRefMBDO.update.AddListener(updateHPTracker);
    }

    private void OnDisable()
    {
        if(hpEntityLogic!=null)
        {
            hpEntityLogic.hpUpdated.RemoveListener(updateHealth);
        }

        treeListMBDO.update.RemoveListener(updateTrees);
        enemyListMBDO.update.RemoveListener(updateEnemies);
        playerRefMBDO.update.AddListener(updateHPTracker);
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
