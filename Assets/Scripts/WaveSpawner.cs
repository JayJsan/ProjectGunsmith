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

    public enum SpawnState { SPAWNING, WAITING, COUNTING, START };

    [System.Serializable]
    public class Wave
    {
        public enum TypeOfWave { ENEMY, BOSS, SHOP }
        public TypeOfWave waveType;
        public string name;
        public List<Transform> enemies;

        // Number of enemies to spawn
        // Position on int corresponds to which enemy on enemy list
        public List<int> enemyCount;
        public float rate;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public Transform[] spawnPoints;

    public int enemiesAlive = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public TextMeshProUGUI waveText;

    public bool firstRound = false;

    //private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;
    public GameObject Shop;
    public GameObject ShopUI;
    public GameObject Bob;
    public GameObject BobUI;
    private void Start()
    {
        firstRound = false;
        state = SpawnState.START;
        enemiesAlive = 0;
        waveCountdown = timeBetweenWaves;
        waveText.gameObject.SetActive(false);
        // Find spawnpoints gameobject, check how many there are, and assign to spawnpoints;
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (state == SpawnState.START)
        {
            return;
        }

        if (waves[nextWave].waveType == Wave.TypeOfWave.SHOP)
        {
            //OnShop = true;
            // there's supposed to be a start timer here
            waveText.text = "Shopping! :D";
            Bob.SetActive(true);
            BobUI.SetActive(true);
            Shop.SetActive(true);
            ShopUI.SetActive(true);
            return;
        }

        waveText.gameObject.SetActive(true);
        waveText.text = waves[nextWave].name;
        Shop.SetActive(false);
        ShopUI.SetActive(false);


        if (state == SpawnState.WAITING)
        {
            // Check if enemies are still alive
            if (!CheckIfEnemiesAlive())
            {
                // Begin new round
                BeginNewWave();

            }
            else
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
        }
        else
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
        }
        else
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

        int temp = 0;
        foreach (Transform enemy in _wave.enemies)
        {
            for (int i = 0; i < _wave.enemyCount[temp]; i++)
            {
                SpawnEnemy(enemy);
                AddEnemiesAlive();
                yield return new WaitForSeconds(1f / _wave.rate);
            }
            //AddEnemiesAlive(_wave.enemyCount[temp]);
            temp++;
        }
        /*
        for (int i = 0; i < _wave.enemies.Count - 1; i++)
        {

        }
        */

        // Spawn wave
        /*
        for (int i = 0; i < _wave.count; i++)
        {
            //SpawnEnemy(_wave.enemy);
            enemiesAlive++;
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        */


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

    public void StartRound()
    {
        if (firstRound == false)
        {
            firstRound = true;
            state = SpawnState.COUNTING;
            return;
        }
        nextWave++;
    }

    public void SubtractEnemiesAlive()
    {
        enemiesAlive--;
    }

    public void SubtractEnemiesAlive(int amount)
    {
        enemiesAlive -= amount;
    }

    public void AddEnemiesAlive()
    {
        enemiesAlive++;
    }
    public void AddEnemiesAlive(int amount)
    {
        enemiesAlive += amount;
    }
}
