using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity_Logic : MonoBehaviour
{
    public Event_One_Float hpUpdated = new Event_One_Float();

    //entity parameters
    public bool disableColliderOnDeath = true;

    public float health = 3f;

    private void OnValidate()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public GameObject onDeathReplaceWith;

    [SerializeField, HideInInspector]
    private SpriteRenderer spriteRenderer;

    //initialize ambiguous parameters
    public void Awake() => OnValidate();
    private void Start() => hpUpdated.Invoke(health);
    private void OnEnable() => hpUpdated.Invoke(health);

    private void Update() => invincibility = Mathf.Min(invincibilityTime, invincibility + Time.deltaTime);

    private float invincibility = 0;

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
                CommitSuduku();
            }
            else
            {
                StartCoroutine(ChangeColor());
            }
        }
    }

    public void CommitSuduku()
    {
        Enemy_Logic tmp = GetComponent<Enemy_Logic>();
        if (tmp != null)
        {
            tmp.enabled = false;
        }

        if (gameObject.layer == 11 && onDeathReplaceWith == null)
        {
            gameObject.layer = 12;
            Animator tempAnimator = GetComponent<Animator>();
            if (tempAnimator != null)
            {
                tempAnimator.SetBool("isSaved", true);
            }

            if (disableColliderOnDeath)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
        else if (onDeathReplaceWith != null)
        {
            Instantiate(onDeathReplaceWith, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private enum GoToColor
    {
        hurtColor, normalColor
    };

    public float flashSpeed = 20f;

    [SerializeField]
#pragma warning disable CS0414
#pragma warning disable IDE0044 // Add readonly modifier
    private bool flashCustomColor = false;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649

    [SerializeField, OneLine.OneLine, ShowIf("flashCustomColor")]
    private FlashingColors flashingColors;

    [System.Serializable]
    private struct FlashingColors
    {
        public Color GetHurtColor() => hurtColor ?? Color.red;

        [SerializeField]
#pragma warning disable CS0649
#pragma warning disable IDE0044 // Add readonly modifier
        private Color? hurtColor;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649

        public Color GetNormalColor() => normalColor ?? Color.white;

        [SerializeField]
#pragma warning disable CS0649
#pragma warning disable IDE0044 // Add readonly modifier
        private Color? normalColor;
#pragma warning restore CS0649,IDE0044
    }

    private IEnumerator ChangeColor()
    {
        float flashingTime = timeToFlashOnHit;
        GoToColor goToColor = GoToColor.hurtColor;
        float amount = 0;
        while (flashingTime > 0)
        {
            flashingTime -= Time.deltaTime;
            if (goToColor == GoToColor.hurtColor)
            {
                amount += Time.deltaTime * flashSpeed;
            }
            else
            {
                amount -= Time.deltaTime * flashSpeed;
            }

            if (amount >= 1)
            {
                goToColor = GoToColor.normalColor;
            }
            else if (amount <= 0)
            {
                goToColor = GoToColor.hurtColor;
            }

            spriteRenderer.color = Color.Lerp(flashingColors.GetNormalColor(), flashingColors.GetHurtColor(), amount);

            yield return null;
        }
        spriteRenderer.color = Color.white;
    }
}

[System.Serializable]
public class Event_One_Float : UnityEvent<float>
{
}