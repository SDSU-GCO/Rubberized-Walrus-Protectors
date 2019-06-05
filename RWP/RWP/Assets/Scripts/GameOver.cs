using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class GameOver : MonoBehaviour
{
    Image image;
    Animator animator;
    float secondsToFadeIn=3;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            Debug.Log("GameOver: Reloading scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
