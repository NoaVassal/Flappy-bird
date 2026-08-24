using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float _flapForce = 5f;

    private Rigidbody2D _rigidbody2D;
    private InputAction _flap;
    private bool _flapRequested;

    private void Awake()
    {
        GetReferences();
    }

    private void Start()
    {
        _rigidbody2D.simulated = false;
    }

    private void GetReferences()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        var player = InputSystem.actions.FindActionMap(
            Constants.PlayerActionMap,
            throwIfNotFound: true);

        _flap = player.FindAction(
            Constants.FlapAction,
            throwIfNotFound: true);
    }

    private void Update()
    {
        if (!_flap.WasPressedThisFrame())
        {
            return;
        }

        switch (GameManager.Instance.CurrentState)
        {
            case GameState.PreGame:
                StartGame();
                break;

            case GameState.Game:
                _flapRequested = true;
                break;

            case GameState.Dying:
                break;

            case GameState.GameOver:
                GameManager.Instance.RestartGame();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState != GameState.Game)
        {
            _flapRequested = false;
            return;
        }

        if (!_flapRequested)
        {
            return;
        }

        _flapRequested = false;
        Flap();
    }

    private void StartGame()
    {
        _rigidbody2D.simulated = true;

        GameManager.Instance.StartGame();

        _flapRequested = true;
    }

    private void Flap()
    {
        var velocity = _rigidbody2D.linearVelocity;

        velocity.y = 0f;

        _rigidbody2D.linearVelocity = velocity;

        _rigidbody2D.AddForce(
            Vector2.up * _flapForce,
            ForceMode2D.Impulse);
        GameManager.Instance.PlayFlapSound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameState.Game:
                HandleGameCollision(collision);
                break;

            case GameState.Dying:
                HandleDyingCollision(collision);
                break;
        }
    }

    private void HandleGameCollision(Collision2D collision)
    {
        
        if (collision.collider.CompareTag(Constants.GroundTag))
        {
            _flapRequested = false;
            _rigidbody2D.linearVelocity = Vector2.zero;

            GameManager.Instance.StartDying();
            GameManager.Instance.GameOver();

            return;
        }

        
        if (!collision.collider.CompareTag(Constants.ObstacleTag))
        {
            return;
        }

        _flapRequested = false;

        var velocity = _rigidbody2D.linearVelocity;
        velocity.x = 0f;
        velocity.y = 0f;
        _rigidbody2D.linearVelocity = velocity;

        GameManager.Instance.StartDying();
    }

    private void HandleDyingCollision(Collision2D collision)
    {
        if (!collision.collider.CompareTag(Constants.GroundTag))
        {
            return;
        }

        _rigidbody2D.linearVelocity = Vector2.zero;

        GameManager.Instance.GameOver();
    }
}