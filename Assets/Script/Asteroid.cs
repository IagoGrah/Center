using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed = 0.5f;
    public float Damage = 10f;
    public bool IsBurst = false;
    public bool IsHeal = false;
    public float ScoreValue = 100f;
    public float SpawnChance = 0.5f;
    public GameObject Explosion;
    private Rigidbody rb;
    private Renderer rend;
    private BoxCollider coll;
    private GameManager gameManager;
    private bool isDying;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        gameManager = FindObjectOfType<GameManager>();
        coll = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        rb.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, Vector3.zero, Speed), 0.1f);
    }

    void OnTriggerStay(Collider col)
    {
        if (!IsBurst)
        {
            var center = col.GetComponent<Center>();
            if (center != null)
            {
                center.Damage(Time.deltaTime * Damage); 
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        SoundManager.PlaySound("centerHurt");
        
        if (IsBurst)
        {
            var center = col.GetComponent<Center>();
            if (center != null)
            {
                if (IsHeal) { center.Heal(Damage); }
                else { center.Damage(Damage); }
                Explode();
            }
        }
    }

    void Explode()
    {
        isDying = true;
        
        var explosionObject = Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(explosionObject, Explosion.GetComponent<ParticleSystem>().main.duration);

        SoundManager.PlaySound("explosion");
        
        var colliders = Physics.OverlapSphere(transform.position, Damage);

        foreach (var obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(Damage*25, transform.position, Damage/2);
            }
        }
        Destroy(this.gameObject);
    }
    
    IEnumerator Die()
    {
        isDying = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.isKinematic = true;
        rend.enabled = false;
        coll.enabled = false;
        gameManager.Score += ScoreValue;
        SoundManager.PlaySound("enemyDeath");
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
        if (!isDying && this.gameObject.activeInHierarchy)
        {
            StartCoroutine(Die());
        }
    }
}
