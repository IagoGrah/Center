using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameManager gameManager;

    public void Buy(int item)
    {
        switch (item)
        {
            case 1:
                gameManager.player.GetComponent<Player>().Shockwaves += 3;
                break;

            case 2:
                gameManager.center.GetComponent<Center>().maxHealth += 100;
                gameManager.center.GetComponent<Center>().Heal(100);
                break;

            case 3:
                gameManager.spawner.GetComponent<Spawner>().burstChanceModifier -= 1f;
                break;

            case 4:
                gameManager.spawner.GetComponent<Spawner>().healChanceModifier += 5f;
                break;
        }

        SoundManager.PlaySound("button");

        gameManager.Wave++;
        gameManager.ChangeState(GameManager.State.Play);
    }
}