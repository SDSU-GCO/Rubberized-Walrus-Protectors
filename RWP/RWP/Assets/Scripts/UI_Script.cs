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

    [SerializeField, HideInInspector]
    public TreeListMBDO treeListMBDO;
    [SerializeField, HideInInspector]
    public EnemyListMBDO enemyListMBDO;
    
    public Entity_Logic hpEntityLogic;

    public Canvas pauseMenu;
    public Canvas gameOverScreen;
    int MaxTreeCount = 0;
    int MaxEnemyCount = 0;
    
    public Color tint = new Color(1.0f, 0, 1.0f, 1.0f);
    
    public List<Tilemap> tilemaps = new List<Tilemap>();
    public List<Image> images = new List<Image>();
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    
    private void OnValidate()
    {
        if(treeListMBDO == null || enemyListMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref treeListMBDO);
            mBDOInitializationHelper.SetupMBDO(ref enemyListMBDO);
        }

        if (hpEntityLogic == null)
        {
            Entity_Logic tmp = FindObjectOfType<Ally_Logic>().GetComponent<Entity_Logic>();
            if(tmp.gameObject.scene == gameObject.scene)
                hpEntityLogic = tmp;
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
        UpdateColor();
    }


    void UpdateColor()
    {
        float t = 0;
        if ((MaxTreeCount + MaxEnemyCount) != 0)
            t = (treeListMBDO.trees.Count + enemyListMBDO.enemies.Count) / (float)(MaxTreeCount + MaxEnemyCount);
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

    public void UpdateEnemies()
    {
        if (enemyListMBDO.enemies.Count > MaxEnemyCount)
        {
            MaxEnemyCount = enemyListMBDO.enemies.Count;
        }
        enemies.SetText("Enemies Left: " + enemyListMBDO.enemies.Count);
        UpdateColor();
    }

    public void UpdateTrees()
    {
        if(treeListMBDO.trees.Count>MaxTreeCount)
        {
            MaxTreeCount = treeListMBDO.trees.Count;
        }
        trees.SetText("Trees Left: " + treeListMBDO.trees.Count);

        UpdateColor();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
                if(gameOverScreen.gameObject.activeInHierarchy!=true)
                {
                    Time.timeScale = 0;
                    Time.fixedDeltaTime = 0;
                }
            }
            else
            {
                pauseMenu.gameObject.SetActive(false);
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
        treeListMBDO.update.AddListener(UpdateTrees);
        enemyListMBDO.update.AddListener(UpdateEnemies);
        if (hpEntityLogic == null)
            Debug.LogError("hptracker's EntityLogic == null in: " + this);
        if (hpEntityLogic != null)
        {
            hpEntityLogic.hpUpdated.AddListener(UpdateHealth);
        }
    }

    private void OnDisable()
    {
        if(hpEntityLogic!=null)
        {
            hpEntityLogic.hpUpdated.RemoveListener(UpdateHealth);
        }

        treeListMBDO.update.RemoveListener(UpdateTrees);
        enemyListMBDO.update.RemoveListener(UpdateEnemies);
    }

    public void UpdateHealth(float health)
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
