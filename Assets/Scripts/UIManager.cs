using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;

    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

    [SerializeField] private Image _livesImg;

    [SerializeField] private Sprite[] _liveSprites;

    private bool _showing = true;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _livesImg.sprite = _liveSprites[3];
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        //assign text component to the handle
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
            
        }
    }

    private void GameOverSequence()
    {
        StartCoroutine(DisplayGameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

    }

    private IEnumerator DisplayGameOverFlickerRoutine()
    {
        while (_showing)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
