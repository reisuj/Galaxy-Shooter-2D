using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private bool isGamePaused = false;    
    [SerializeField]
    private Text _gameOverText;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            _uiManager.RestartCountdown();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseToggle();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public void PauseToggle()
    {
        if (isGamePaused == false)
        {
            Time.timeScale = 0;
            isGamePaused = true;
        }
        else if(isGamePaused == true)
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
    }
}
