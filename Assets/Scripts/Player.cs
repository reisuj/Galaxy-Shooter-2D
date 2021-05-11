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
    [SerializeField]
    private int _maxAmmo = 1;
    [SerializeField]
    private int _currentAmmo;
    private float _audioDelay = 5.0f;
    private float _playAmmoDepleted;

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
    private bool multiShotActive = false;
    [SerializeField]
    private bool tripleShotActive = false;
    [SerializeField]
    private bool _shieldIsActive = false;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _ammoDepleted;
    [SerializeField]
    private AudioClip _explosionAudio;
    private Renderer _shieldColor;
    private Color _shieldDefaultColor;
    private bool _thrusterEmpty = false;
    private bool _thrusterActive = false;
    private int _thrusterMax = 100;
    [SerializeField]
    private int _thrusterLevel = 100;
    [SerializeField]
    private BoostBar _boostbar;



    void Start()
    {
        _currentAmmo = _maxAmmo;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();        
        _shieldColor = _shieldVisual.GetComponent<Renderer>();
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
        _playerLives -= 1;

        EngineDamageCheck(_playerLives);

        _uiManager.UpdateLives(_playerLives);

        if(_playerLives < 1)
        {
            StartCoroutine(PlayerDead());
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
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }
    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(6.0f);
        _speed /= _speedMultiplier;
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
    void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterEmpty == false)
        {
            _thrusterActive = true;
            StartCoroutine(ThrusterDrain());
            _speed = _thrusterSpeed;
        }
        else
        {
            _thrusterActive = false;
            _speed = _baseSpeed;
        }
    }
    private IEnumerator ThrusterDrain()
    {
        while (_thrusterActive)
        {
            yield return new WaitForSeconds(2.0f);
            _thrusterLevel = _thrusterLevel - 1;
            _boostbar.SetBooster(_thrusterLevel);
            yield return new WaitForSeconds(2.0f);
        }
        
    }
    private IEnumerator ThrusterRecharge()
    {
        yield return new WaitForSeconds(5.0f);
    }
}
