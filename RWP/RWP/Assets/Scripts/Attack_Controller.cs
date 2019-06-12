using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Controller : MonoBehaviour
{
    [SerializeField]
    private float timeToLive = 0.5f;//in seconds
    [SerializeField]
#pragma warning disable IDE0044 // Add readonly modifier
    private bool LiveForever = false;
#pragma warning restore IDE0044 // Add readonly modifier
    private float originalTimeToLive;//in seconds
    public float AttackDelay = 0.75f;
    public int damage;
    public float speed = 7;
    [ReorderableList]
    public List<CustomGCOTypes.CollisionLayerKey> targetLayer = new List<CustomGCOTypes.CollisionLayerKey>();
    private new Collider collider = null;

    public GameObject onDestroySpawnPrefab;

    private void Awake()
    {
        originalTimeToLive = timeToLive;
        collider = GetComponent<Collider>();
        if (collider != null && collider.isTrigger == false && LiveForever == false)
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (LiveForever != true)
        {
            timeToLive -= Time.deltaTime;
        }

        if (collider != null && timeToLive < originalTimeToLive / 2.0f)
        {
            collider.enabled = true;
        }
        if (timeToLive <= 0)
        {
            PoofObject();
        }
    }
    //enemy/ally check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (targetLayer.Contains((CustomGCOTypes.CollisionLayerKey)collision.gameObject.layer))
        {
            Entity_Logic temp = collision.gameObject.GetComponent<Entity_Logic>();
            if (temp != null)
            {
                temp.TakeDamage(damage);
            }
        }
        if ((CustomGCOTypes.CollisionLayerKey)collision.gameObject.layer != CustomGCOTypes.CollisionLayerKey.platforms)
        {
            PoofObject();
        }
    }

    private void PoofObject()
    {
        if (onDestroySpawnPrefab != null)
        {
            Instantiate(onDestroySpawnPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}