﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;
using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity_Logic : MonoBehaviour
{
    public Event_One_Float hpUpdated = new Event_One_Float();
    //entity parameters
    public bool disableColliderOnDeath = true;
    public float health = 3f;

    private void OnValidate()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public GameObject onDeathReplaceWith;
    
    [SerializeField, HideInInspector]
    SpriteRenderer spriteRenderer;
    
    //initialize ambiguous parameters
    public void Awake()
    {
        OnValidate();
    }

    private void Start()
    {
        hpUpdated.Invoke(health);
    }

    void OnEnable()
    {
        hpUpdated.Invoke(health);
    }
    
    public void LogHP(float t)
    {
        Debug.Log("hpUpdated: "+t);
    }

    void Update()
    {
        invincibility = Mathf.Min(invincibilityTime, invincibility + Time.deltaTime);
    }
    
   
    float invincibility = 0;
    [MinValue(0f)]
    public float invincibilityTime = 0.5f;
    [MinValue(0f)]
    public float timeToFlashOnHit = 0.5f;

    //take damage function
    public void TakeDamage(float amount)
    {
        if (invincibility >= invincibilityTime)
        {
            health -= amount;
            invincibility = 0;

            hpUpdated.Invoke(health);
            if (health <= 0)
            {
                Enemy_Logic tmp = GetComponent<Enemy_Logic>();
                if(tmp!=null)
                    tmp.enabled=false;

                if(gameObject.layer==11 && onDeathReplaceWith==null)
                {
                    gameObject.layer = 12;
                    Animator tempAnimator = GetComponent<Animator>();
                    if (tempAnimator != null)
                        tempAnimator.SetBool("isSaved", true);
                    if(disableColliderOnDeath)
                        GetComponent<Collider2D>().enabled = false;
                }
                else if(onDeathReplaceWith!=null)
                {
                    Instantiate(onDeathReplaceWith, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(ChangeColor());
            }
        }
    }

    enum GoToColor { hurtColor, normalColor};
    public float flashSpeed = 20f;
#pragma warning disable IDE0044 // Add readonly modifier
    bool flashCustomColor = false;
#pragma warning restore IDE0044 // Add readonly modifier
    
    [ShowIf("flashCustomColor")]
    FlashingColors flashingColors = new FlashingColors { hurtColor = Color.red, normalColor = Color.white };

    public struct FlashingColors
    {
        public Color hurtColor;
        public Color normalColor;
    }

    IEnumerator ChangeColor()
    {
        float flashingTime = timeToFlashOnHit;
        GoToColor goToColor = GoToColor.hurtColor;
        float amount = 0;
        while(flashingTime>0)
        {
            flashingTime -= Time.deltaTime;
            if (goToColor == GoToColor.hurtColor)
                amount += Time.deltaTime * flashSpeed;
            else
                amount -= Time.deltaTime * flashSpeed;

            if (amount >= 1)
                goToColor = GoToColor.normalColor;
            else if (amount <= 0)
                goToColor = GoToColor.hurtColor;

            spriteRenderer.color = Color.Lerp(flashingColors.normalColor, flashingColors.hurtColor, amount);

            yield return null;
        }
        spriteRenderer.color = Color.white;
    }
    
}
[System.Serializable]
public class Event_One_Float : UnityEvent<float>
{

}