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
    private AudioClip _laserAudio;

    [SerializeField]
    private AudioClip _explosionAudio;

    private float _canFire = 0.0f;

    private float _fireDelay;

    private int movementTypeID;
    

    private void Start()
    {
        movementTypeID = Random.Range(1, 4);

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
    }

    private void CalculateMovement()
    {        
        switch (movementTypeID)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * (_enemySpeed / 2) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
                break;
            default:
                break;
        }        

        if (transform.position.y < -7.0f)
        {
            float newX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(newX, 7.0f, 0);
            Debug.Log("Rest to top of screen");
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
    void FireLaser()
    {
        _fireDelay = Random.Range(3.0f, 7.0f);
        // y position prevents enemy from firing before showing on screen
        if (Time.time > _canFire && transform.position.y < 6.0f) 
        {
            _canFire = Time.time + _fireDelay;
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(5, 11));
            }
            AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            Destroy(this.gameObject, 2.8f);
        }
        else if(other.tag == "Player")
        {            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            Destroy(this.gameObject, 2.8f);
        }
    }

}
