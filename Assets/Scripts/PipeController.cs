using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _destroyX = -6f;

    private Rigidbody2D _rigidbody2D;
    private bool _isMoving = true;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
        {
            return;
        }

        if (GameManager.Instance.CurrentState != GameState.Game)
        {
            return;
        }

        var movement =
            Vector2.left * (_moveSpeed * Time.fixedDeltaTime);

        var newPosition =
            _rigidbody2D.position + movement;

        _rigidbody2D.MovePosition(newPosition);

        if (_rigidbody2D.position.x < _destroyX)
        {
            Destroy(gameObject);
        }
    }

    public void StopMoving()
    {
        _isMoving = false;

        _rigidbody2D.linearVelocity = Vector2.zero;

        DisableColliders();
    }

    private void DisableColliders()
    {
        var colliders = GetComponentsInChildren<Collider2D>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
}