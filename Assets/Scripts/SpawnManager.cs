using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    //[SerializeField]
    //private GameObject _powerupPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private bool _playerIsAlive;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private float _startDelay;
    private int powerUpID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2.0f);
        while (_playerIsAlive == true)
        {
            float _enemyX = Random.Range(-9.0f, 9.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, (new Vector3(_enemyX, 10.5f, 0)), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(4.0f);
        }        
    }

    IEnumerator SpawnPowerup()
    {
        while (_playerIsAlive == true)
        {
            float _powerupX = Random.Range(-9.0f, 9.0f);
            PowerUpSelector();
            GameObject newPowerup = Instantiate(_powerups[powerUpID], (new Vector3(_powerupX, 10.5f, 0)), Quaternion.identity);
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
        
        if (weight < 10)
        {
            powerUpID = 5;
        }
        else if (weight < 30)
        {
            powerUpID = 4;
        }
        else if (weight< 50)
        {
            powerUpID = 3;
        }
        else if (weight < 70)
        {
            powerUpID = 2;
        }
        else if (weight < 90)
        {
            powerUpID = 1;
        }
        else if (weight < 110)
        {
            powerUpID = 0;
        }
        else
        {
            return;
        }
            
    }
}
