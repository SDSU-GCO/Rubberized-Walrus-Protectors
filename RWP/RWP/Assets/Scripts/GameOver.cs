using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class GameOver : MonoBehaviour
{
    [SerializeField, HideInInspector]
    Image image;
    [SerializeField, HideInInspector]
    Animator animator;
#pragma warning disable IDE0044 // Add readonly modifier
    float secondsToFadeIn=3;
#pragma warning restore IDE0044 // Add readonly modifier

    private void OnValidate()
    {
        if(animator==null)
            animator = GetComponent<Animator>();
        if (image == null)
            image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Color temp = image.color;
        temp.a = 0;
        currentTime = 0;
        image.color = temp;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    float currentTime = 0;
    public float animationLength = 4;
    float currentAnimationTime = 0;
    // Update is called once per frame
    void Update()
    {
        currentAnimationTime += Time.unscaledDeltaTime*0.2f;
        while (currentAnimationTime > animationLength)
            currentAnimationTime -= animationLength;

        animator.SetFloat("Time", currentAnimationTime);
        currentTime = Mathf.Min(secondsToFadeIn, currentTime + Time.unscaledDeltaTime);
        Color temp = image.color;
        temp.a = Mathf.InverseLerp(0, secondsToFadeIn, currentTime);
        image.color = temp;
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && image.color.a==1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            Debug.Log("GameOver: Reloading scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
