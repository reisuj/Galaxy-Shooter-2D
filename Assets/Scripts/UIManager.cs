using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region TEXT REFEENCES
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _waveCountText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    [SerializeField]
    private Text _countDownText;
    [SerializeField]
    private Text _ammoDepletedText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _missileCountText;
    #endregion TEXT REFERENCES

    #region OTHER REFERENCES
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Sprite[] _livesSprites;
    #endregion OTHER REFERENCES

    #region VARIABLES INT, FLOAT, STRING, BOOL
    // INTEGERS
    [SerializeField]
    private int _ammoCount;
    [SerializeField]
    private int _maxAmmo = 15;
    [SerializeField]
    private int _missileCount;
    private int _maxMissiles = 3;
    //FLOATS
    [SerializeField]
    private float _flashDelay = 0.75f;
    //BOOLEANS
    [SerializeField]
    private bool _ammoDepleted = false;
    [SerializeField]
    private bool _missileDepleted;
    #endregion VARIABLES INT, FLOAT, STRING, BOOL

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.text = "";
        _restartLevelText.gameObject.SetActive(false);
    }

    public void GameStarted()
    {
        StartCoroutine(WaveDisplay(1));
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        _livesImg.sprite = _livesSprites[currentLives];        
    }
    
    public void UpdateAmmo(int currentAmmo)
    {
        _ammoCountText.text = "AMMO: " + currentAmmo + " / " + _maxAmmo;
        if (currentAmmo < 1)
        {
            AmmoDepleted();
        }
        else
        {
            _ammoDepleted = false;
        }
    }

    public void UpdateMissileCount(int missileCount)
    {
        _missileCount += missileCount;
        if (_missileCount < 0)
        {
            _missileCount = 0;
        }
        else if (_missileCount > _maxMissiles)
        {
            _missileCount = 3;
        }
        _missileCountText.text = "Missiles: " + _missileCount;
    }

    public void UpdateWave(int currentWave)
    {
        StartCoroutine(WaveDisplay(currentWave));
    }

    IEnumerator WaveDisplay(int currentWave)
    {
        _waveText.text = "WAVE " + currentWave;
        _waveCountText.text = "WAVE " + currentWave;
        yield return new WaitForSeconds(5.0f);
        _waveText.text = "";
    }

    public void BossWave()
    {
        StartCoroutine(BossDisplay());
    }

    IEnumerator BossDisplay()
    {
        _waveText.text = "BOSS WAVE!!";
        _waveCountText.text = "BOSS WAVE!!";
        yield return new WaitForSeconds(5.0f);
        _waveText.text = "";
    }

    public IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();
        _restartLevelText.gameObject.SetActive(true);        
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }        
    }

    public void RestartCountdown()
    {
        StartCoroutine(RestartCountdownRoutine());
    }

    IEnumerator RestartCountdownRoutine()
    {
        _restartLevelText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        yield return new WaitForSeconds(_flashDelay);
        _countDownText.text = "5";
        yield return new WaitForSeconds(_flashDelay);
        _countDownText.text = "4";
        yield return new WaitForSeconds(_flashDelay);
        _countDownText.text = "3";
        yield return new WaitForSeconds(_flashDelay);
        _countDownText.text = "2";
        yield return new WaitForSeconds(_flashDelay);
        _countDownText.text = "1";
        yield return new WaitForSeconds(_flashDelay);
        _gameManager.RestartScene();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    public void AmmoDepleted()
    {
        _ammoDepleted = true;
        StartCoroutine(AmmoDepletedRoutine());
    }    

    IEnumerator AmmoDepletedRoutine()
    {
        while (_ammoDepleted == true)
        {
            _ammoDepletedText.text = "AMMO DEPLETED!";
            yield return new WaitForSeconds(0.5f);
            _ammoDepletedText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
