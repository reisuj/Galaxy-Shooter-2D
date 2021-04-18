using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private int _score = 0;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerScored()
    {
        _score += 10;
        _scoreText.text = "Score: " + _score;
    }
}
