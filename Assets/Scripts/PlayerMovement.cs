using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 0.03f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    private bool _canMove = true;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    private void Update()
    {
        if (_canMove)
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

            // Attempt to move using collision detection
            bool success = TryMove(_movement);

            if (!success) 
            {
                // Try moving only in the X direction
                success = TryMove(new Vector2(_movement.x, 0));
            }
            if (!success) 
            {
                // Try moving only in the Y direction
                success = TryMove(new Vector2(0, _movement.y));
            }

            // Update animations
            _animator.SetFloat(_horizontal, _movement.x);
            _animator.SetFloat(_vertical, _movement.y);

            if (_movement != Vector2.zero)
            {
                _animator.SetFloat(_lastHorizontal, _movement.x);
                _animator.SetFloat(_lastVertical, _movement.y);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = _rb.Cast(
                direction,                 // Direction of movement
                movementFilter,            // Filter for valid collisions
                castCollisions,            // List to store collisions
                _moveSpeed * Time.deltaTime + collisionOffset // Distance to check
            );

            if (count == 0) 
            {
                _rb.MovePosition(_rb.position + direction * _moveSpeed);

                return true;
            }
        }
        return false;
    }
}
