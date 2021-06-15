using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 1.0f;

    private Player _player;

    private Animator _anim;

    private BoxCollider2D _collider;
    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _laserAudio;

    [SerializeField]
    private AudioClip _explosionAudio;

    private float _canFire = 0.0f;
    private float _canFireBack = 0.0f;

    private float _fireDelay;

    private int movementTypeID;

    [SerializeField]
    private int _shieldChance;
    [SerializeField]
    private bool _shieldIsActive = false;
    [SerializeField]
    private GameObject _shields;

    [SerializeField]
    private float _rotationSpeed = 0.5f;

    private Vector3 _direction = Vector3.down;
    private bool _canRotate = true;
    private bool _isBackwards = false;
    

    private void Start()
    {
        movementTypeID = Random.Range(1, 4);

        _spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL!");
        }

        _shieldChance = Random.Range(1, 101);

        if (_shieldChance >= 75)
        {
            _shieldIsActive = true;
            _shields.SetActive(true);
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL!");
        }

        _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
        //FireBack();
        ScanBack();
    }

    private void CalculateMovement()
    {        
        switch (movementTypeID)
        {
            case 1:
                transform.Translate((_direction + Vector3.left) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((_direction + Vector3.right) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(_direction * _enemySpeed * Time.deltaTime);
                break;
            default:
                break;
        }        

        if (transform.position.y < -7.0f)
        {
            transform.Rotate(0f, 0f, 180f, Space.World);
            _canRotate = true;
            _isBackwards = false;
            float newX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(newX, 7.0f, 0);
            _direction = Vector3.down;
        }

        if (transform.position.x <= -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
        else if (transform.position.x >= 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }

        if (transform.position.y < _player.transform.position.y && _canRotate == true)
        {
            transform.Rotate(0f, 0f, 180f, Space.World);
            _canRotate = false;
            _direction = Vector3.up;
            _isBackwards = true;
            //_direction = new Vector3(0, -1, 0);
        }
    }
    void FireLaser()
    {
        float _fireDelay = Random.Range(3.0f, 7.0f);
        // y position prevents enemy from firing before showing on screen
        if (Time.time > _canFire && transform.position.y < 6.0f && _isBackwards == true) 
        {
            _canFire = Time.time + (_fireDelay / 3);
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            Instantiate(_enemyLaser, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f));
        }
        else if (Time.time > _canFire && transform.position.y < 6.0f && _isBackwards == false)
        {
            _canFire = Time.time + _fireDelay;
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }
    }

    //public void FireBack()
    //{
    //    if (Time.time > _canFireBack)
    //    {
    //        _canFireBack = Time.time + _fireDelay;
    //        Debug.Log("Reverse Fire");
    //        AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
    //        Instantiate(_enemyLaser, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f));
    //    }
        
    //}        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            if (_shieldIsActive == false)
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(Random.Range(5, 11));
                }
                EnemyDestroyed();
            }
            else
            {
                Destroy(other.gameObject);
                _shieldIsActive = false;
                _shields.SetActive(false);
            }            
        }
        else if(other.tag == "Player")
        {
            if (_shieldIsActive == false)
            {
                Debug.Log("Enemy Collider hit by " + other.name);
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
                EnemyDestroyed();
            }
            else
            {
                _player.Damage();
                _shieldIsActive = false;
                _shields.SetActive(false);
            }            
        }
    }
    private void EnemyDestroyed()
    {
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        _anim.SetTrigger("OnEnemyDeath");
        _collider.enabled = false;
        _spawnManager.EnemyKilled();
        Destroy(this.gameObject, 2.8f);
    }

    private void ScanBack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.0f, transform.TransformDirection(Vector2.up), 100.0f);
        
        if (hit.collider != null)
        {
            Debug.Log("Collided with " + hit.collider.name);
            if (hit.collider.name == "Player")
            {
                FireBack();
            }           
        }        
    }
}
