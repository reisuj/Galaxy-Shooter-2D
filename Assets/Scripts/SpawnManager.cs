using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private GameObject _powerupContainer = null;
    [SerializeField]
    private bool _playerIsAlive = false;
    [SerializeField]
    private GameObject[] _powerups = null;
    [SerializeField]
    private int _powerUpID = 0;

    // Wave System
    [SerializeField]
    private int _waveCount = 1;
    [SerializeField]
    private int _waveMax = 3;
    [SerializeField]
    private int _enemiesInWave = 0;
    [SerializeField]
    private int _enemiesSpawned = 0;
    [SerializeField]
    private int _enemiesAlive = 0;
    private UIManager _uiManager = null;

    [SerializeField]
    private GameObject[] _enemiesToSpawn;
    [SerializeField]
    private int _enemyID;

    private IEnumerator _enemyRoutine;
    


    private int[] _table =
    {
        100, // Ammo Powerup
        75, // TripleShot Powerup
        50, // Shield Powerup
        50, // Speed Powerup
        50, // Health Powerup
        30, // Negative Powerup
        30, // MultiShot Powerup
        25  // Missile Powerup
    };

    [SerializeField]
    private int weight;
    [SerializeField]
    private int _total;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }

        foreach (var item in _table)
        {
            _total += item;
        }

        _enemyRoutine = SpawnEnemy();
    }
    // Update is called once per frame
    void Update()
    {
        WaveControl();
    }
    IEnumerator SpawnEnemy()
    {        
        yield return new WaitForSeconds(3.0f);
        while (_enemiesSpawned < _enemiesInWave && _playerIsAlive == true)
        {
            float _enemyX = Random.Range(-9.0f, 9.0f);
            _enemyID = Random.Range(0, _waveCount);
            GameObject newEnemy = Instantiate(_enemiesToSpawn[_enemyID], (new Vector3(_enemyX, 10.5f, 0)), Quaternion.identity);            
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned++;
            _enemiesAlive++;            
            yield return new WaitForSeconds(4.0f);
        }        
    }
    IEnumerator SpawnPowerup()
    {
        while (_playerIsAlive == true)
        {
            float _powerupX = Random.Range(-9.0f, 9.0f);
            PowerUpSelector();
            GameObject newPowerup = Instantiate(_powerups[_powerUpID], (new Vector3(_powerupX, 10.5f, 0)), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }
    IEnumerator NextWave()
    {
        _uiManager.UpdateWave(_waveCount);
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SpawnEnemy());
    }

    public void EnemyKilled()
    {
        _enemiesAlive--;
    }

    private void WaveControl()
    {
        if (_waveCount <= _waveMax)
        {
            if (_enemiesSpawned == _enemiesInWave && _enemiesAlive == 0)
            {
                _enemiesSpawned = 0;
                _enemiesInWave += 5;
                _enemiesAlive = 0;
                _waveCount++;
                StartCoroutine(NextWave());
            }
        }
        else
        {
            StopCoroutine(_enemyRoutine);
            StartBossLevel();
        }
    }


    public void PlayerDied()
    {
        _playerIsAlive = false;
        Destroy(_enemyContainer);
        Destroy(_powerupContainer);
    }
    public void StartSpawning()
    {
        _playerIsAlive = true;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
        _uiManager.GameStarted();
    }

    private void PowerUpSelector()
    {
        weight = Random.Range(0, _total);
        
        for (int i = 0; i < _table.Length; i++)
        {
            if (weight <= _table[i])
            {
                _powerUpID = i;
                return;
            }
            else
            {
                weight -= _table[i];
            }
        }           
    }

    private void StartBossLevel()
    {
        // Set Wave message to Boss Wave
        // Instantiate boss above screen
        // Play dramatic music
        // Have boss move slowly down to center screen

        // Start BossRoutine coroutine for battle
        StartCoroutine(BossRoutine());
    }
    private IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
