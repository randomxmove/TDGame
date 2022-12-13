using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameOver gameOverScreen;
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private Waypoints[] waypoints;
    [SerializeField] private Transform board;
    [SerializeField] private Transform enemySpawnTransform;
    [SerializeField] private GameObject enemyGroundPrefab;
    [SerializeField] private GameObject enemyAirPrefab;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnDelay;

    private int groundSpawnCount;
    private int airSpawnCount;

    private int groundSpawnPerLevel = 8;
    private int airSpawnPerLevel = 3;

    int level = 1;

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);
        string highscoreKey = "HighScore";
        int highscore = PlayerPrefs.GetInt(highscoreKey, 0);
        if (highscore < playerState.Score)
        {
            PlayerPrefs.SetInt(highscoreKey, playerState.Score);
            highscore = PlayerPrefs.GetInt(highscoreKey, 0);
        }

        gameOverScreen.SetHighScore(highscore);
        gameOverScreen.SetPlayerScore(playerState.Score);


        StopAllCoroutines();
    }

    private PlayerState playerState;
    private List<Enemy> spawnedEnemies;
    private List<BaseBuildingPivot> buildingCells;
    private bool isWaveStarted = false;
    private float timer;

    public List<BaseBuildingPivot> BuildingCells { get => buildingCells; set => buildingCells = value; }
    public bool IsInGame { get; internal set; }
    public PlayerState PlayerState => playerState;
    public float CurrentTimer => timer;
    public int RemainingEnemies => spawnedEnemies.Count;
    public int Level => level;


    private void Awake()
    {
        if (Instance == null) Instance = this;    
    }

    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            StartWave();
            level++;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Time.timeScale++;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Time.timeScale--;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            playerState.AddFunds(1000);
        }

        if (isWaveStarted)
        {
            if (spawnedEnemies.Count <= 0)
            {
                isWaveStarted = false;
                StartCoroutine(StartCountdown());
                level++;
            }
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene((int)SceneID.Main);
    }

    public void Initialize()
    {
        IsInGame = true;
        Time.timeScale = 1;
        // Initialize Player State
        playerState = new PlayerState();
        playerState.AddFunds(100);
        playerCore.Initialize();

        // Initialize Spawn
        if (spawnedEnemies != null)
        {
            foreach (Enemy enemy in spawnedEnemies)
                Destroy(enemy.gameObject);
            spawnedEnemies.Clear();
        }
        spawnedEnemies = new List<Enemy>();

        isWaveStarted = false;

        // Initialize Buildings
        if(buildingCells != null)
        {
            foreach(BaseBuildingPivot cell in buildingCells)
            {
                cell.RemoveBuilding();
            }
            buildingCells.Clear();
        }
        buildingCells = new List<BaseBuildingPivot>();

        // Initialize Game Data
        groundSpawnCount = groundSpawnPerLevel;
        airSpawnCount = airSpawnPerLevel;
        level = 1;
        timer = 0;

        StartCoroutine(StartCountdown());
    }

    private void StartWave()
    {
        isWaveStarted = true;
        // Initialize Wave Monsters
        List<Enemy> spawnList = new List<Enemy>();

        // Spawn ground enemy, 8 for each level, 8 -> 16 -> 24 and so on
        // Spawn air enemies starting at level 3, spawn 3 for each level
        groundSpawnCount = groundSpawnPerLevel * level;
        airSpawnCount = (level >= 3) ? airSpawnPerLevel * (level - 2) : 0;

        //Ground Enemies
        for (int i = 0; i < groundSpawnCount; i++)
        {
            GameObject enemyObject = Instantiate(enemyGroundPrefab, enemySpawnTransform.position, Quaternion.identity, board);
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            enemy.SetMultiplier(level);
            enemy.onDeath = OnEnemyDeath;
            enemy.onDestroyed = OnEnemyDestroyed;
            spawnedEnemies.Add(enemy);
            spawnList.Add(enemy);
        }

        // Air Enemies
        for (int i = 0; i < airSpawnCount; i++)
        {
            GameObject enemyObject = Instantiate(enemyAirPrefab, enemySpawnTransform.position, Quaternion.identity, board);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.onDeath = OnEnemyDeath;
            enemy.onDestroyed = OnEnemyDestroyed;
            spawnedEnemies.Add(enemy);
            spawnList.Add(enemy);
        }

        StartCoroutine(SpawnEnemies(spawnList));
        StartCoroutine(StartWaveTimer());
    }

    private Waypoints GetGroundWaypoint()
    {
        return Array.Find(waypoints, path => path.Type == WaypointType.Ground);
    }

    private Waypoints GetAirWaypoint()
    {
        // 1 = air path 1 || 2 = airpath 2
        int random = UnityEngine.Random.Range(1, 3);
        return Array.Find(waypoints, path => path.Type == (WaypointType)random);
    }

    public void OnEnemyDestroyed(Enemy enemy)
    {
        // Destroyed upon Death and Core Hit
        spawnedEnemies.Remove(enemy);
    }
    public void OnEnemyDeath(Enemy enemy)
    {
        // Killed by Turrets
        PlayerState.AddFunds(enemy.Funds);
        PlayerState.AddScore(enemy.Score);
    }

    #region Coroutines
    IEnumerator StartWaveTimer()
    {
        yield return new WaitForSeconds(1);

        while (isWaveStarted)
        {
            playerState.AddFunds(10);
            playerState.AddScore(10);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator StartCountdown()
    {
        timer = spawnTimer;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }

        StartWave();
    }

    IEnumerator SpawnEnemies(List<Enemy> spawnList)
    {
        foreach (Enemy enemy in spawnList)
        {
            if (enemy.Type == EnemyType.Ground) enemy.Path = GetGroundWaypoint();
            else if (enemy.Type == EnemyType.Air) enemy.Path = GetAirWaypoint();
            else enemy.Path = GetGroundWaypoint(); // DEFAULT WAYPOINT

            enemy.Spawn();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    #endregion
}
