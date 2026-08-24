using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PipeSpawner _pipeSpawner;

    [Header("UI")]
    [SerializeField] private GameObject _preGamePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _flapClip;
    [SerializeField] private AudioClip _scoreClip;
    [SerializeField] private AudioClip _hitClip;

    private int _score;
    private GameState _gameState = GameState.PreGame;

    public GameState CurrentState => _gameState;

    private void Start()
    {
        GameReset();
    }

    private void GameReset()
    {
        _gameState = GameState.PreGame;

        SetScore(0);

        _scoreText.gameObject.SetActive(true);
        _preGamePanel.SetActive(true);
        _gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (_gameState != GameState.PreGame)
        {
            return;
        }

        _gameState = GameState.Game;

        _preGamePanel.SetActive(false);

        _pipeSpawner.StartSpawning();
    }

    public void AddScore()
    {
        if (_gameState != GameState.Game)
        {
            return;
        }

        SetScore(_score + 1);
        PlayScoreSound();
    }

    private void SetScore(int score)
    {
        _score = score;
        _scoreText.text = $"{_score}";
    }

    public void StartDying()
    {
        if (_gameState != GameState.Game)
        {
            return;
        }

        _gameState = GameState.Dying;
        PlayHitSound();
        _pipeSpawner.StopSpawning();

        StopAllPipes();

        _scoreText.gameObject.SetActive(false);
    }

    private void StopAllPipes()
    {
        var pipes = FindObjectsByType<PipeController>(
            FindObjectsSortMode.None);

        foreach (var pipe in pipes)
        {
            pipe.StopMoving();
        }
    }

    public void GameOver()
    {
        if (_gameState != GameState.Dying)
        {
            return;
        }

        _gameState = GameState.GameOver;

        _finalScoreText.text = $"{_score}";
        _gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        if (_gameState != GameState.GameOver)
        {
            return;
        }

        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    public void PlayFlapSound()
    {
        _audioSource.PlayOneShot(_flapClip);
    }

    private void PlayScoreSound()
    {
        _audioSource.PlayOneShot(_scoreClip);
    }

    private void PlayHitSound()
    {
        _audioSource.PlayOneShot(_hitClip);
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}