using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBody : Body
{
    // collision levers
    protected float _minMoveDistance = .001f;
    protected float _shellRadius = .01f;
    protected float minGroundNormalY = .65f;

    // jump levers
    float _jumpHeight = 6f;
    float _timeToJumpHeight = .6f;
    float _timeToFall = .4f;
    //float _groundGravity = .1f;

    // move levers
    float _changeMoveTotalTime = .3f;
    float _maxSpeed = 14.14f;

    // control variables
    protected Vector2 _velocity = Vector2.zero;
    protected Rigidbody2D _rb2d;

    // collision variables
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    protected Vector2 _groundNormal = Vector2.one;

    // jump variables
    float _jumpVelocity = 0;
    float _currentGravity = 0;
    float _baseGravity = 0;
    float _downGravity = 0;
    //bool _wasGrounded = false;
    //bool _firstJumpFrame = false;

    // move variables
    Vector2 _lastDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _targetDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _startDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _currentDirection = Vector2.zero;
    float _targetVelocity = 0;
    float _changeMoveStartTime = 0;
    float _changeMoveElapsedTime = 0;

    void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start() {
        int layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);

        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(layerMask);
        _contactFilter.useLayerMask = true;

        _baseGravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToJumpHeight, 2);
        _downGravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToFall, 2);

        _jumpVelocity = Mathf.Abs(_baseGravity) * _timeToJumpHeight;
    }
    public override void Jump() {
        if (_grounded) {
            _velocity.y = _jumpVelocity;
        }
    }
    public override void Move(Vector2 direction) {

        _changeMoveElapsedTime += Time.deltaTime;

        // if direction has changed we have a new target
        if (_lastDirection != direction) {
            _changeMoveElapsedTime = 0;
            _startDirection = _currentDirection;
            _targetDirection = direction;
        }

        float t = Mathf.Clamp(_changeMoveElapsedTime / _changeMoveTotalTime, 0, 1);

        t = t * t * (3f - 2f * t);

        _currentDirection = Vector2.Lerp(_startDirection, _targetDirection, Mathf.Abs(t));

        _lastDirection = direction;

        Vector2 nextDirection = _currentDirection * _maxSpeed * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);

        // modify direction by normal to handle slopes
        nextDirection = nextDirection.x * moveAlongGround;
        
        DoMovement(nextDirection, false);
    }
    public override void DoGravity() {
        _currentGravity = _baseGravity;

        if(_velocity.y < 0 && !_grounded) {
            _currentGravity = _downGravity;
        } 


        Vector2 moveStep = ((_velocity + Vector2.up * _currentGravity * Time.deltaTime * 0.5f) * Time.deltaTime);

        _grounded = false;
        moveStep.x = 0;
        DoMovement(moveStep, true);

        _velocity.y += _currentGravity * Time.deltaTime;
    }

    void DoMovement(Vector2 move, bool moveY) {
        float distance = move.magnitude;
        
        if(distance >= _minMoveDistance) {
            int count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);
            _hitBufferList.Clear();
            for (int i = 0; i <count; i++) {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for(int i = 0; i < _hitBufferList.Count; i++) {
                Vector2 currentNormal = _hitBufferList[i].normal;

                // is this the ground or a wall?
                // this will not handle sliding down slopes as is
                if (currentNormal.y > minGroundNormalY) {
                    _grounded = true;
                    if (moveY) {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                        _velocity.y = 0;
                    }
                }

                // modify velocity if it is pushing into a collider
                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0) {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }
}
