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
            GameObject newPowerup = Instantiate(_powerups[Random.Range(0, 5)], (new Vector3(_powerupX, 10.5f, 0)), Quaternion.identity);
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
}
