using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject portal;
    public GameObject fog;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject archerPrefab; 
    [SerializeField] private GameObject warrokPrefab; 
    [SerializeField] private GameObject reaperPrefab; 
    [SerializeField] private Transform[] spawnPoints;
    private int currentWave = 0;
    private bool waveInProgress = false;
    private bool portalActive = false;
    // waveTimer and spawnInterval do not do anything as of now.
    // we can decide whether to remove or implement enemies spawning in
    // certain intervals throughout the wave
    // TODO: SPAWN INTERVALS
    private float waveTimer;
    private float spawnInterval;
    private float zombiesPerWave, archersPerWave, warroksPerWave, reapersPerWave;
    private float zombiesSpawned, archersSpawned, warroksSpawned, reapersSpawned;

    void Update()
    {
        if (currentWave == 10)
        {
            if (!waveInProgress && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !portalActive)
            {
                // ALL WAVES COMPLETED: Add code to allow player to transfer to boss fight
                portal.SetActive(true);
                portalActive = true;
                fog.SetActive(true);
                PlayerDoorOpen door = FindObjectOfType<PlayerDoorOpen>();
                door.isOpening = true;
                door.OpenDoor();
            }
            return;
        }
        waveTimer += Time.deltaTime;
        // If a wave is not currently in progress and there's no enemies left alive, start the next wave
        if (!waveInProgress && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            StartNextWave();
        }
        // Check to see if the wave is over at every frame
        checkEndOfWave();
    }

    void StartNextWave()
    {
        // Begin the wave
        waveInProgress = true;
        currentWave++;
        waveTimer = 0f;
        
        switch (currentWave)
        {
            case 1:
                zombiesPerWave = 5;
                archersPerWave = 0;
                break;
            case 2:
                zombiesPerWave = 5;
                archersPerWave = 2;
                break;
            case 3:
                zombiesPerWave = 7;
                archersPerWave = 3;
                break;
            case 4:
                zombiesPerWave = 5;
                archersPerWave = 2;
                warroksPerWave = 1;
                break;
            case 5:
                zombiesPerWave = 7;
                archersPerWave = 3;
                warroksPerWave = 2;
                break;
            case 6:
                zombiesPerWave = 10;
                archersPerWave = 4;
                warroksPerWave = 3;
                break;
            case 7:
                zombiesPerWave = 10;
                archersPerWave = 7;
                warroksPerWave = 1;
                break;
            case 8:
                zombiesPerWave = 5;
                archersPerWave = 3;
                warroksPerWave = 2;
                reapersPerWave = 1;
                break;
            case 9:
                zombiesPerWave = 12;
                archersPerWave = 4;
                warroksPerWave = 2;
                reapersPerWave = 1;
                break;
            case 10:
                zombiesPerWave = 3;
                archersPerWave = 2;
                warroksPerWave = 2;
                reapersPerWave = 3;
                break;
            default:
                break;
        }

        Debug.Log("Current Wave: " + currentWave);

        SpawnWave(currentWave);
    }

    void SpawnWave(int waveNumber)
    {
        for (int i = 0; i < zombiesPerWave; i++)
        {
            SpawnEnemy(zombiePrefab);
            zombiesSpawned++;
        }

        for (int i = 0; i < archersPerWave; i++)
        {
            SpawnEnemy(archerPrefab);
            archersSpawned++;
        }

        for (int i = 0; i < warroksPerWave; i++)
        {
            SpawnEnemy(warrokPrefab);
            warroksSpawned++;
        }

        for (int i = 0; i < reapersPerWave; i++)
        {
            SpawnEnemy(reaperPrefab);
            reapersSpawned++;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        // Instantiate the enemy prefab at the randomized spawn position
        Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
    }

    void checkEndOfWave()
    {
        // Ends the wave spawning when enough enemies have been spawned
        // TODO: NEEDS BALANCING (we may change how the end of wave logic works)
        if (waveInProgress && zombiesSpawned + archersSpawned + warroksSpawned + reapersSpawned >= zombiesPerWave+ archersPerWave + warroksPerWave + reapersPerWave)
        {
            waveInProgress = false;
        }
    }
}
