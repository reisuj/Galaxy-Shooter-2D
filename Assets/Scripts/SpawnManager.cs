using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _powerupPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _playerIsAlive;
    [SerializeField]
    private GameObject[] _powerups;
    // Start is called before the first frame update
    void Start()
    {
        _playerIsAlive = true;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
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
            Instantiate(_powerups[Random.Range(0, 2)], (new Vector3(_powerupX, 10.5f, 0)), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }        
    }

    public void StopSpawning()
    {
        _playerIsAlive = false;
    }
}
