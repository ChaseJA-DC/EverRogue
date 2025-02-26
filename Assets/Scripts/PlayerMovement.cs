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
    private bool _isSliding = false;
    private bool _slideOnCooldown = false;
    private float _slideSpeedMultiplier = 200f;
    private float _slideDuration = 0.2f;
    private float _slideCooldown = 2f;
    private Vector2 _slideDirection;


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
        if (!_isSliding && _canMove)
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !_isSliding && !_slideOnCooldown && _movement != Vector2.zero)
        {
            StartCoroutine(Slide());
        }

        if (!_isSliding)
        {
            bool success = TryMove(_movement);
            if (!success) success = TryMove(new Vector2(_movement.x, 0));
            if (!success) success = TryMove(new Vector2(0, _movement.y));

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
    private IEnumerator Slide()
{
    _isSliding = true;
    _canMove = false;
    _slideOnCooldown = true; // Start cooldown

    _animator.SetBool("isSliding", true); // Use boolean for animation transition

    // Store movement direction before starting slide
    _slideDirection = _movement.normalized;

    if (_slideDirection == Vector2.zero)
    {
        _isSliding = false;
        _canMove = true;
        _slideOnCooldown = false;
        _animator.SetBool("isSliding", false); // Reset animation if canceled
        yield break;
    }

    float slideSpeed = _moveSpeed * _slideSpeedMultiplier;

    // Apply an instant force to the Rigidbody2D for a fast slide
    _rb.velocity = _slideDirection * slideSpeed;

    yield return new WaitForSeconds(_slideDuration);

    // Stop movement after sliding
    _rb.velocity = Vector2.zero;
    _isSliding = false;
    _canMove = true;
    _animator.SetBool("isSliding", false); // Transition back to Idle after sliding

    // Wait for cooldown duration before allowing another slide
    yield return new WaitForSeconds(_slideCooldown);
    _slideOnCooldown = false;
}



}
