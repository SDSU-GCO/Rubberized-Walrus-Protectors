using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOut : MonoBehaviour
{
    
    public TextMeshProUGUI textMeshProText;
    public AnimationCurve animationCurve;

    private void Awake()
    {
        if (textMeshProText == null)
            textMeshProText = GetComponent<TextMeshProUGUI>();
    }
    
    Color temp;
    public float fadeSpeed = 1;
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        temp = textMeshProText.color;
        time += Time.deltaTime * fadeSpeed;
        temp.a = animationCurve.Evaluate(time);
        textMeshProText.color = temp;
        

    }
}
