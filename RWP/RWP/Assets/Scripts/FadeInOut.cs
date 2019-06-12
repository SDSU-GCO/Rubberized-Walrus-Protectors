using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public TextMeshProUGUI textMeshProText;
    public AnimationCurve animationCurve;

    private void Awake()
    {
        if (textMeshProText == null)
        {
            textMeshProText = GetComponent<TextMeshProUGUI>();
        }
    }

    private Color temp;
    public float fadeSpeed = 1;
    private float time = 0;
    // Update is called once per frame
    private void Update()
    {
        temp = textMeshProText.color;
        time += Time.deltaTime * fadeSpeed;
        temp.a = animationCurve.Evaluate(time);
        textMeshProText.color = temp;
    }
}