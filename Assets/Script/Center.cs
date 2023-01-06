using UnityEngine;
using UnityEngine.UI;

public class Center : MonoBehaviour
{
    public GameManager gameManager;
    public Text healthText;
    private Renderer rend;
    public float startingMaxHealth = 1000f;
    [HideInInspector] public float maxHealth;
    private float hp;
    [HideInInspector] public float HP
    { get { return hp; }
        set { hp = value;
        healthText.text = "HP : " + (int)value; }}

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
        maxHealth = startingMaxHealth;
        hp = maxHealth;
    }

    void Update()
    {
        if (hp <= 0)
        {
            gameManager.ChangeState(GameManager.State.GameOver);
        }
    }

    void FixedUpdate()
    {
        rend.material.color = Color.Lerp(Color.red, Color.white, hp/maxHealth);
        transform.Rotate(0, 0, Time.deltaTime*50f);
    }

    public void Damage(float dmg)
    {
        HP -= dmg;
    }

    public void Heal(float value)
    {
        HP += value;
        if (HP > maxHealth) { HP = maxHealth; }
        SoundManager.PlaySound("heal");
    }
}
