using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private float _flashDelay = 0.75f;
    [SerializeField]
    private Text _countDownText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.text = "";
        _restartLevelText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
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
}
