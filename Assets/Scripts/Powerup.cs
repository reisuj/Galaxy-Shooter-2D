using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID = 0;
    [SerializeField]
    private AudioClip _powerupAudio = null;
    [SerializeField]
    private int ammoRefillAmount = 15;
    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private bool _isBeingCollected = false;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _explosionAudio;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isBeingCollected = true;
        }

        if (_isBeingCollected == true)
        {
            BeingCollected();
        }
        else
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }
        
        if (transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BeingCollected()
    {
        Vector3 dir = this.transform.position - _player.transform.position;
        dir = dir.normalized;
        this.transform.position -= dir * Time.deltaTime * (_speed * 2);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldPickedUp();
                        break;
                    case 3:                        
                        player.AmmoCollected(ammoRefillAmount);
                        break;
                    case 4:
                        player.HealthCollected();
                        break;
                    case 5:
                        player.MultiShotActive();
                        break;
                    case 6:
                        player.NegativePowerupCollected();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }                
            }
            AudioSource.PlayClipAtPoint(_powerupAudio, new Vector3(0, 0, -10), 1.0f);
            Destroy(this.gameObject);
        }

        if (other.tag == "EnemyLaser")
        {
            Debug.Log("Hit by: " + other.tag);
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
            Instantiate(_explosion, transform.position, Quaternion.identity);                     
            Destroy(this.gameObject);
        }
    }
}
