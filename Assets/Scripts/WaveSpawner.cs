using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
public class WaveSpawner : MonoBehaviour
{
    /* Code taken and modified from Brackey's "How to make a Wave Spawner in Unity 5" Video
     * 
     *
     */

    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;

        // Number of enemies to spawn
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public Transform[] spawnPoints;

    public int enemiesAlive = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public TextMeshProUGUI waveText;

    public bool startRound = false;

    //private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        enemiesAlive = 0;
        waveCountdown = timeBetweenWaves;

        // Find spawnpoints gameobject, check how many there are, and assign to spawnpoints;
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (!startRound)
        {
            return;
        }

        waveText.text = waves[nextWave].name;

        if (state == SpawnState.WAITING)
        {
            // Check if enemies are still alive
            if (!CheckIfEnemiesAlive())
            {
                // Begin new round
                BeginNewWave();
                 
            } else
            {
                // if enemies are still alive
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Start spawning waves
                if (waves.Length > 0)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
        } else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void BeginNewWave()
    {
        Debug.Log("Wave completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        //waveText.text = "Wave: " + ((nextWave + 1).ToString());

        if (nextWave >= waves.Length - 1)
        {
            // Reached end of waves array!
            nextWave = 0;
            Debug.Log("All waves complete!! Looping to first wave");
        } else
        {
            nextWave++;
        }
    }

    bool CheckIfEnemiesAlive()
    {
        /*
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {

        }
        */

        if (enemiesAlive == 0)
        {
            return false;
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        // Spawn wave
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            enemiesAlive++;
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        
        if (spawnPoints.Length == 0)
        {
            Debug.Log("NO SPAWNPOITNS FOUDNS!!!!");
            return;
        }
        
        // choose randmo spawnpoint
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn enemy
        Instantiate(_enemy, _sp.position, _sp.rotation);
        
    }
}
