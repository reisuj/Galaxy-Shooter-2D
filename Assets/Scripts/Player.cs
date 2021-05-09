using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _baseSpeed = 5.0f;
    [SerializeField]
    private float _thrusterSpeed = 10.0f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private float _laserOffset = 1.1f;    
    [SerializeField]
    private float _fireDelay = 0.5f;
    [SerializeField]
    private float _canFire = 0.0f;
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private int _score;
    private int _shieldStrength = 0;
    private bool _playerAlive = true;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private bool tripleShotActive = false;
    [SerializeField]
    private bool _shieldIsActive = false;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _explosionAudio;
    private Renderer shieldColor;
    private Color _shieldDefaultColor;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        shieldColor = _shieldVisual.GetComponent<Renderer>();
        _shieldDefaultColor = _shieldVisual.GetComponent<Renderer>().material.GetColor("_Color");
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manageris NULL!!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_playerAlive == true)
        {
            ThrusterControl();
            PlayerMovement();
            LaserControl();
        }
               
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

    void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _thrusterSpeed;
        }
        else
        {
            _speed = _baseSpeed;
        }
    }

    void LaserControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireDelay;
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            if (tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, (new Vector3(transform.position.x, transform.position.y, 0)), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, (new Vector3(transform.position.x, transform.position.y + _laserOffset, 0)), Quaternion.identity);
            }
            
        }
    }

    public void Damage()
    {
        if (_shieldIsActive == true)
        {
            _shieldStrength--;
            ShieldCheck();
            return;
        }
        _playerLives -= 1;

        if (_playerLives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_playerLives == 1)
        {
            _leftEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_playerLives);

        if(_playerLives < 1)
        {
            StartCoroutine(PlayerDead());
        }
    }

    public void TripleShotActive()
    {
        tripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(6.0f);
        tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(6.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldPickedUp()
    {
        if (_shieldStrength < 3)
        {
            _shieldStrength++;
        }
        _shieldIsActive = true;
        _shieldVisual.SetActive(true);        
        ShieldCheck();
        Debug.Log("Player Collected Shield");
    }

    public void ShieldCheck()
    {
        switch (_shieldStrength)
        {
            case 0:
                _shieldIsActive = false;
                _shieldVisual.SetActive(false);
                break;
            case 1:
                shieldColor.material.SetColor("_Color", Color.red);
                break;
            case 2:
                shieldColor.material.SetColor("_Color", Color.yellow);
                break;
            case 3:
                shieldColor.material.SetColor("_Color", _shieldDefaultColor);
                break;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private IEnumerator PlayerDead()
    {
        _playerAlive = false;
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        _spawnManager.StopSpawning();
        _uiManager.GameOver();
        Destroy(this.gameObject);
    }
}
