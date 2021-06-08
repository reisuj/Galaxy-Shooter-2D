using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region MOVEMENT
    [SerializeField]
    private float _playerSpeed = 5.0f;
    [SerializeField]
    private float _baseSpeed = 5.0f;    
    [SerializeField]
    private float _speedMultiplier = 2.0f;
    #endregion MOVEMENT

    #region POWERUPS
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private bool multiShotActive = false;
    [SerializeField]
    private bool tripleShotActive = false;
    [SerializeField]
    private bool _shieldIsActive = false;
    #endregion POWERUPS

    #region LASERS
    [SerializeField]
    private float _laserOffset = 1.1f;
    [SerializeField]
    private float _fireDelay = 0.5f;
    [SerializeField]
    private float _canFire = 0.0f;
    [SerializeField]
    private int _maxAmmo = 1;
    [SerializeField]
    private int _currentAmmo;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _ammoDepleted;
    private float _playAmmoDepleted;
    private int _heldAmmo; // Holds ammo amount during Negative Powerup duration
    #endregion LASERS

    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private int _score;
    private int _shieldStrength = 0;
    private bool _playerAlive = true;
    
    private float _audioDelay = 5.0f;
    

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _particleSystem;
    
    [SerializeField]
    private GameObject _explosion;
    
    [SerializeField]
    private AudioClip _explosionAudio;
    private Renderer _shieldColor;
    private Color _shieldDefaultColor;
    
    [SerializeField]
    private Camera _camera;

    [Header("Thruster Properties")]
    [SerializeField]
    private float _thrusterSpeed = 10.0f;
    [SerializeField]
    private int _thrusterLevel = 100;
    [SerializeField]
    private BoostBar _boostbar;
    private bool _thrustAvailable;
    private bool _thrusterRegen = false;
    private int _thrusterMax = 100;

    void Start()
    {
        _currentAmmo = _maxAmmo;
        _thrustAvailable = true;
        _boostbar.SetStartBooster(_thrusterMax);
        _boostbar.SetBooster(_thrusterMax);
        _thrusterLevel = _thrusterMax;
        transform.position = new Vector3(0, -3, 0);
        
        
        _camera = Camera.main;
        _shieldColor = _shieldVisual.GetComponent<Renderer>();
        _shieldDefaultColor = _shieldVisual.GetComponent<Renderer>().material.GetColor("_Color");

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manageris NULL!!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }
    }
    void Update()
    {
        if (_playerAlive == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _thrustAvailable == true)
            {
                StartCoroutine(ThrusterActivated());
            }
            PlayerMovement();
            LaserControl();
        }               
    }
    void PlayerMovement()
    {        
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalMovement * _playerSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalMovement * _playerSpeed * Time.deltaTime);

        // Restricts Player's Vetical movement between -1.5 and 3.0 on the Y axis.
        transform.position = new Vector3(transform.position.x, (Mathf.Clamp(transform.position.y, -5.0f, 1.0f)), 0);

        // Wraps Player on the X Axis when going past each edge.
        if (transform.position.x < -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }        
    }
    void LaserControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _currentAmmo > 0)
        {            
            _canFire = Time.time + _fireDelay;
            _currentAmmo--;
            _uiManager.UpdateAmmo(_currentAmmo);
            AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
            if (tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, (new Vector3(transform.position.x, transform.position.y, 0)), Quaternion.identity);
            }
            else if (multiShotActive == true)
            {
                MultiShot();
            }
            else
            {
                Instantiate(_laserPrefab, (new Vector3(transform.position.x, transform.position.y + _laserOffset, 0)), Quaternion.identity);
            }
        }
        else if (_currentAmmo < 1 && Time.time > _playAmmoDepleted)
        {
            _playAmmoDepleted = Time.time + _audioDelay;
            _uiManager.AmmoDepleted();
            AudioSource.PlayClipAtPoint(_ammoDepleted, new Vector3(0f, 3.5f, -10f), 1.0f);
        }
    }
    public void MultiShot()
    {
        for (int fireangle = -67; fireangle < 83; fireangle += 15)
        {
            GameObject newBullet = Instantiate(_laserPrefab, (new Vector3(transform.position.x, transform.position.y + _laserOffset, 0)), Quaternion.identity);
            newBullet.transform.eulerAngles = Vector3.forward * fireangle;
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
        CameraControl cameraControl = Camera.main.GetComponent<CameraControl>();
        if (cameraControl != null)
        {
            cameraControl.CamShake();
        }
        _playerLives -= 1;

        EngineDamageCheck(_playerLives);

        _uiManager.UpdateLives(_playerLives);

        if(_playerLives < 1)
        {
            StartCoroutine(PlayerDead());
        }
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
                _shieldColor.material.SetColor("_Color", Color.red);
                break;
            case 2:
                _shieldColor.material.SetColor("_Color", Color.yellow);
                break;
            case 3:
                _shieldColor.material.SetColor("_Color", _shieldDefaultColor);
                break;
        }
    }
    private void EngineDamageCheck(int lives)
    {
        if (lives == 3)
        {
            _rightEngine.SetActive(false);
        }
        if (lives == 2)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(false);
        }
        else if (lives == 1)
        {
            _leftEngine.SetActive(true);
        }
    }
    public void TripleShotActive()
    {
        multiShotActive = false;
        tripleShotActive = true;
        AmmoCollected(5);
        StartCoroutine(TripleShotPowerDown());
    }
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(6.0f);
        tripleShotActive = false;
    }
    public void MultiShotActive()
    {
        tripleShotActive = false;
        multiShotActive = true;
        AmmoCollected(5);
        StartCoroutine(MultiShotPowerDown());
    }
    IEnumerator MultiShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        multiShotActive = false;
    }
    public void SpeedBoostActive()
    {
        _playerSpeed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }
    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(6.0f);
        _playerSpeed /= _speedMultiplier;
    }
    public void AmmoCollected(int ammoAmount)
    {
        _currentAmmo += ammoAmount;        
        if (_currentAmmo > 15)
        {
            _currentAmmo = _maxAmmo;
        }
        _uiManager.UpdateAmmo(_currentAmmo);
    }
    public void NegativePowerupCollected()
    {
        _particleSystem.SetActive(true);
        _heldAmmo = _currentAmmo;
        _currentAmmo = 0;
        _playerSpeed = 2;
        StartCoroutine(NegativePowerupRecovery());
    }
    IEnumerator NegativePowerupRecovery()
    {
        yield return new WaitForSeconds(5.0f);
        _particleSystem.SetActive(false);
        _currentAmmo = _heldAmmo;
        _playerSpeed = _baseSpeed;
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
    public void HealthCollected()
    {
        _playerLives++;
        if (_playerLives > 3)
        {
            _playerLives = 3;
        }
        EngineDamageCheck(_playerLives);
        _uiManager.UpdateLives(_playerLives);
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    private IEnumerator PlayerDead()
    {
        BoxCollider2D _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
        _collider.enabled = false;
        _playerAlive = false;
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        _spawnManager.StopSpawning();
        _uiManager.GameOver();
        Destroy(this.gameObject);
    }
    private IEnumerator ThrusterActivated()
    {
        _playerSpeed = _thrusterSpeed;
        while (Input.GetKey(KeyCode.LeftShift) && _thrusterLevel > 0)
        {
            yield return new WaitForSeconds(0.05f);
            _thrusterLevel = _thrusterLevel - 2;
            _boostbar.SetBooster(_thrusterLevel);
            if (_thrusterLevel < 1)
            {
                _thrustAvailable = false;
            }
        }
        _playerSpeed = _baseSpeed;
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ThrusterRecharge());
    }
    private IEnumerator ThrusterRecharge()
    {
        _thrusterRegen = true;
        yield return new WaitForSeconds(1.0f);
        while (_thrusterLevel < _thrusterMax && _thrusterRegen == true)
        {
            _thrusterLevel++;
            _boostbar.SetBooster(_thrusterLevel);
            yield return new WaitForSeconds(0.05f);
        }
        _thrustAvailable = true;
        _thrusterRegen = false;
    }
}
