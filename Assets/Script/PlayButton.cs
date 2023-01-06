using UnityEngine;

public class PlayButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
    
    void OnMouseDown()
    {
        if (this.enabled)
        {
            FindObjectOfType<GameManager>().ChangeState(GameManager.State.Play);
            SoundManager.PlaySound("button");
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime*50f);
    }
}
