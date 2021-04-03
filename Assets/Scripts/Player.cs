using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;    
    [SerializeField]
    private float _laserOffset = 0.85f;    
    [SerializeField]
    private float _fireDelay = 0.5f;
    [SerializeField]
    private float _canFire = 0.0f;
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _laserPrefab;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manageris NULL!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        LaserControl();        
    }

    void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalMovement * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalMovement * _speed * Time.deltaTime);

        // Restricts Player's Vetical movement between -1.5 and 3.0 on the Y axis.
        transform.position = new Vector3(transform.position.x, (Mathf.Clamp(transform.position.y, -1.5f, 3.0f)), 0);

        // Wraps Player on the X Axis when going past each edge.
        if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }        
    }

    void LaserControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireDelay;
            Instantiate(_laserPrefab, (new Vector3(transform.position.x, transform.position.y + _laserOffset, 0)), Quaternion.identity);
        }
    }

    public void Damage()
    {
        _playerLives -= 1;

        if(_playerLives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }
}
