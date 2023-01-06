using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] enemyTypes;
    public float healChanceModifier = 0f;
    public float burstChanceModifier = 0f;
    public float spawnRate = 1f;
    public int enemiesPerWave = 25;
    public int enemiesPerWaveScaling = 10;
    private int currEnemiesPerWave;
    private int enemyCounter;

    void OnEnable()
    {
        currEnemiesPerWave = enemiesPerWave + (gameManager.Wave*enemiesPerWaveScaling);
        InvokeRepeating("SpawnCycle", 0, spawnRate - (gameManager.Wave*0.1f));
    }

    void SpawnCycle()
    {
        var type = PickEnemy();
        Spawn(type);
    }
    
    GameObject PickEnemy()
    {
        float total = 0;
        var probs = new List<float>();
        foreach (var enemy in enemyTypes)
        {
            var chance = enemy.GetComponent<Asteroid>().SpawnChance;
            if (enemy.name == "StandardHeal")
            { chance += healChanceModifier; }
            else if (enemy.name == "StandardBurst")
            { chance += burstChanceModifier; }
            
            probs.Add(chance);
            total += chance;
        }

        float randomPoint = Random.value * total;
        for (int i= 0; i < probs.Count; i++)
        {
            if (randomPoint < probs[i])
            {
                return enemyTypes[i];
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return enemyTypes[probs.Count - 1];
    }
    
    void Spawn(GameObject enemyType)
    {        
        var enemy = Instantiate(enemyType, transform.position+new Vector3(0, 10f, 0), transform.rotation, transform);
        enemy.transform.RotateAround(transform.position, Vector3.forward, (float)Random.Range(0, 360));
        
        enemyCounter++;
        if (enemyCounter > currEnemiesPerWave)
        { EndWave(); }
    }

    void EndWave()
    {
        enemyCounter = 0;
        if (gameManager.Wave == 8)
        {
            gameManager.Win();
        }
        else
        {
            gameManager.ChangeState(GameManager.State.Shop);
        }
    }

    void OnDisable()
    {
        if (IsInvoking("SpawnCycle"))
        {
            CancelInvoke("SpawnCycle");
            
            var enemies = GetComponentsInChildren<Asteroid>();
            foreach (var item in enemies)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
