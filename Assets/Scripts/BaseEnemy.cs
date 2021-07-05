using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    #region GAMEOBJECTS
    [SerializeField]
    protected GameObject _enemyLaser;
    [SerializeField]
    protected GameObject _explosionFX;
    [SerializeField]
    protected GameObject _shields;
    #endregion

    #region AUDIO
    [SerializeField]
    protected AudioClip _laserAudio;
    [SerializeField]
    protected AudioClip _explosionAudio;
    #endregion

    #region OTHER REFERENCES
    [SerializeField]
    protected SpawnManager _spawnManager;
    protected Player _player;
    protected BoxCollider2D _collider;
    #endregion

    #region VARIABLES INT, FLOAT, STRING, BOOL
    protected int _shieldChance;
    protected int movementTypeID;
    [SerializeField]
    protected float _enemySpeed = 3.0f;
    protected float _randomX;
    protected float _fireTime = 0.0f;
    [SerializeField]
    protected bool _shieldIsActive = false;
    protected bool _canFire = false;
    #endregion

    protected virtual void Start()
    {
        movementTypeID = Random.Range(1, 4);

        GetReferences();        

        InitialShieldCheck();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateMovement();

        FireLaser();
    }

    protected virtual void GetReferences()
    {
        _spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL!");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
    }

    protected virtual void InitialShieldCheck()
    {
        _shieldChance = Random.Range(1, 101);
        if (_shieldChance >= 75)
        {
            _shieldIsActive = true;
            _shields.SetActive(true);
        }
    }

    protected virtual void CalculateMovement()
    {

        switch (movementTypeID)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * (_enemySpeed / 2) * Time.deltaTime, Space.World);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * (_enemySpeed / 2) * Time.deltaTime, Space.World);
                break;
            case 3:
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime, Space.World);
                break;
            default:
                break;
        }
        ResetScreenPosition();
    }

    protected virtual void ResetScreenPosition()
    {
        if (transform.position.y < -8.0f)
        {
            _randomX = Random.Range(-9.8f, 9.8f);
            transform.position = new Vector3(_randomX, 7.0f, 0);
        }

        if (transform.position.x <= -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
        else if (transform.position.x >= 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
    }

    protected virtual void FireLaser()
    {
        float _fireDelay = Random.Range(3.0f, 5.0f);
        if (Time.time > _fireTime && _canFire == true)
        {
            _fireTime = Time.time + _fireDelay;
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLaser" || other.tag == "HomingMissile")
        {
            if (_shieldIsActive == false)
            {
                _canFire = false;
                if (_player != null)
                {
                    _player.AddScore(Random.Range(5, 11));
                }
                Destroy(other.gameObject);
                EnemyDestroyed();
            }
            else
            {
                Destroy(other.gameObject);
                _shieldIsActive = false;
                _shields.SetActive(false);
            }
        }
        else if (other.tag == "Player")
        {
            if (_shieldIsActive == false)
            {
                _canFire = false;
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

    protected virtual void EnemyDestroyed()
    {
        _enemySpeed = 0;
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosionFX, transform.position, Quaternion.identity);
        _collider.enabled = false;
        _spawnManager.EnemyKilled();
        Destroy(this.gameObject, 1.0f);
    }

    private void OnBecameVisible()
    {
        Debug.Log(this.transform.name + " became visible");
        _canFire = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log(this.transform.name + " became invisible");
        _canFire = false;
    }    
}
