using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _enemyPrefab = null;
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private GameObject _powerupContainer = null;
    [SerializeField]
    private bool _playerIsAlive = false;
    [SerializeField]
    private GameObject[] _powerups = null;
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


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        WaveControl();
    }
    IEnumerator SpawnEnemy()
    {
        _uiManager.UpdateWave(_waveCount);
        yield return new WaitForSeconds(5.0f);
        while (_enemiesSpawned < _enemiesInWave && _playerIsAlive == true)
        {
            float _enemyX = Random.Range(-9.0f, 9.0f);
            GameObject newEnemy = Instantiate(_enemiesToSpawn[0], (new Vector3(_enemyX, 10.5f, 0)), Quaternion.identity);            
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned++;
            _enemiesAlive++;            
            yield return new WaitForSeconds(4.0f);
        }        
    }
    public void EnemyKilled()
    {
        _enemiesAlive--;
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SpawnEnemy());
    }

    private void WaveControl()
    {
        if (_waveCount <= _waveMax)
        {
            if (_enemiesSpawned == _enemiesInWave && _enemiesAlive == 0)
            {
                _enemiesSpawned = 0;
                _enemiesInWave += 2;
                _enemiesAlive = 0;
                _waveCount++;
                StartCoroutine(NextWave());
            }
        }
        else
        {
            StopAllCoroutines();
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
    public void StopSpawning()
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
    }

    private void PowerUpSelector()
    {
        int weight = Random.Range(1, 111);
        
        if (weight < 5)
        {
            _powerUpID = 6; //Negative_powerUp
        }
        else if (weight < 10)
        {
            _powerUpID = 5; //Multi-Shot_PowerUp
        }
        else if (weight < 30)
        {
            _powerUpID = 4; //Health_PowerUp
        }
        else if (weight< 50)
        {
            _powerUpID = 3; //Ammo_PowerUp
        }
        else if (weight < 70)
        {
            _powerUpID = 2; //ShieldPowerup
        }
        else if (weight < 90)
        {
            _powerUpID = 1; //SpeedPowerup
        }
        else if (weight < 110)
        {
            _powerUpID = 0; //TripleShotPowerup
        }
        else
        {
            return;
        }            
    }
}
