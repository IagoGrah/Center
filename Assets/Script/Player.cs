using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    Rigidbody rb;
    Camera mainCam;
    public float followSpeed = 1.5f;
    public int startingShockwaves = 6;
    private int shockwaves;
    [HideInInspector] public int Shockwaves
    { get { return shockwaves; }
        set { shockwaves = value;
        gameManager.hudPanel.transform.Find("Shockwaves").GetComponent<Text>().text = "SHOCKWAVES : " + shockwaves; }}
    public float shockwavePower = 100f;
    public GameObject Shockwave;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shockwaves = startingShockwaves;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shockwaves > 0) { ShootShockwave(); }
    }

    void FixedUpdate()
    {
        var newPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0f;
        rb.MovePosition(Vector3.MoveTowards(transform.position, newPos, followSpeed));
    }
    
    void ShootShockwave()
    {
        Shockwaves--;
        
        var shockwaveObject = Instantiate(Shockwave, transform.position, transform.rotation);
        Destroy(shockwaveObject, Shockwave.GetComponent<ParticleSystem>().main.duration);

        SoundManager.PlaySound("shockwave");
        
        var colliders = Physics.OverlapSphere(transform.position, shockwavePower);

        foreach (var obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(shockwavePower*25, transform.position, shockwavePower);
            }
        }
    }
}
