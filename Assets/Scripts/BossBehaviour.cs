using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private float _destinationY = 5.0f;
    private float _speed = 1.5f;
    [SerializeField]
    private int _laserToFire;
    [SerializeField]
    private int _bossHealth;
    [SerializeField]
    private int _bossMaxHealth = 200;
    [SerializeField]
    private BossHealthBar _bossHealthBar;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private int _missileActivation;
    [SerializeField]
    private bool _canFireMissile;
    [SerializeField]
    private int _lightningActivation;
    [SerializeField]
    private bool _canUseLightning;
    [SerializeField]
    private GameObject _bossMissile;
    [SerializeField]
    private GameObject _leftLightning;
    [SerializeField]
    private GameObject _rightLightning;

    private Player _player;

    [SerializeField]
    private GameObject _explosion1;
    [SerializeField]
    private GameObject _explosion2;
    // Start is called before the first frame update
    void Start()
    {
        _bossHealth = _bossMaxHealth;        

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        StartCoroutine(WeaponControl());        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToStart();
    }

    private void MoveToStart()
    {
        if (transform.position.y > _destinationY)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, _destinationY, 0);         
        }
    }
    private void HitByPlayer(int damage)
    {
        _bossHealth -= damage;
        _bossHealthBar.DamageBoss(_bossHealth);
        _canFireMissile = true;
        _canUseLightning = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLaser")
        {
            GameObject newExplosion = Instantiate(_explosion1, other.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Destroy(other.gameObject);
            HitByPlayer(5);
        }
        else if (other.tag == "HomingMissile")
        {
            GameObject newExplosion = Instantiate(_explosion2, other.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Destroy(other.gameObject);
            HitByPlayer(10);
        }
    }

    IEnumerator WeaponControl()
    {
        yield return new WaitForSeconds(5.0f);
        while (transform.position.y ==_destinationY)
        {
            LaserControl();
            MissileControl();
            StartCoroutine(LightningControl());
            yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
        }        
    }

    private void MissileControl()
    {
        if (_bossHealth%_missileActivation == 0 && _canFireMissile == true)
        {
            Debug.Log("Fire Missile");
            int missileToFire = Random.Range(0, 2);
            if (missileToFire == 0)
            {
                Instantiate(_bossMissile, (new Vector3(-1.0f, 3.6f, 0)), Quaternion.identity);
            }
            else
            {
                Instantiate(_bossMissile, (new Vector3(1.0f, 3.6f, 0)), Quaternion.identity);
            }            
            _canFireMissile = false;
        }
    }

    IEnumerator LightningControl()
    {
        if (_bossHealth%_lightningActivation == 0 && _canUseLightning== true)
        {
            Debug.Log("Activate Lightning");
            _leftLightning.SetActive(true);
            _rightLightning.SetActive(true);
            _canUseLightning = false;
            yield return new WaitForSeconds(2.0f);
            _leftLightning.SetActive(false);
            _player.Damage();
            _rightLightning.SetActive(false);
            _player.Damage();
            yield break;

        }
        else
        {
            yield break;
        }
    }

    private void LaserControl()
    {
        _laserToFire = Random.Range(0, 3);
        switch (_laserToFire)
        {
            case 0:
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, new Vector3(0.0f, 1.4f, 0), Quaternion.identity);
                break;
            case 1:
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, new Vector3(-7.0f, 4.0f, 0), Quaternion.identity);
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, new Vector3(7.0f, 4.0f, 0), Quaternion.identity);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, new Vector3(-2.1f, 1.15f, 0), Quaternion.identity);
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, new Vector3(2.1f, 1.15f, 0), Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
