using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerBody : Body
{
    // collision levers
    protected float _minMoveDistance = .001f;
    protected float _shellRadius = .01f;
    public float minGroundNormalY = .65f;

    // jump levers
    protected float _jumpHeight = 6f;
    protected float _timeToJumpHeight = .6f;
    protected float _timeToFall = .4f;

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

    // move variables
    Vector2 _lastDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _targetDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _startDirection = Vector2.zero; // use Vector2 for convenient access to lerp
    Vector2 _currentDirection = Vector2.zero;
    float _targetVelocity = 0;
    float _changeMoveStartTime = 0;
    float _changeMoveElapsedTime = 0;

    void OnEnable() {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Jump() {
        // no op currently for flyers
        // i still think it is appropriate to leave in, ie for flapping enemies
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

        t = 1;// t * t * (3f - 2f * t);

        _currentDirection = Vector2.Lerp(_startDirection, _targetDirection, Mathf.Abs(t));

        _lastDirection = direction;

        Vector2 nextDirection = _currentDirection * _maxSpeed * Time.deltaTime;

        //if(_hitBufferList.Count > 0) {
        //    Vector2 moveAlongGround = new Vector2(_groundNormal.y, _groundNormal.x);
        //    nextDirection = nextDirection * moveAlongGround;
        //    Debug.Log(moveAlongGround);
        //}

        // modify direction by normal to handle slopes


        DoMovement(nextDirection, false);
    }
    public override void DoGravity() {
        // also a no-op for flyers
        // will leave it in the base class for now as well, ie a falling flyer (stunned)
    }
    void DoMovement(Vector2 move, bool moveY) {
        float distance = move.magnitude;

        //if (distance >= _minMoveDistance) {
        //    int count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);
        //    _hitBufferList.Clear();
        //    for (int i = 0; i < count; i++) {
        //        _hitBufferList.Add(_hitBuffer[i]);
        //    }

        //    for (int i = 0; i < _hitBufferList.Count; i++) {
        //        Vector2 currentNormal = _hitBufferList[i].normal;

        //        // is this the ground or a wall?
        //        // this will not handle sliding down slopes as is
        //        //if (currentNormal.y > minGroundNormalY) {
        //        //    _grounded = true;
        //        //    if (moveY) {
        //                _groundNormal = currentNormal;
        //                //currentNormal.x = 0;
        //                //_velocity.y = 0;
        //        //    }
        //        //}
        //        //else if (_style == MoveStyle.Sky) {
        //        //    _groundNormal = currentNormal;
        //        //    _groundNormal.y = -_groundNormal.y;
        //        //}
        //        //    currentNormal = Vector2.one;
        //        //}

        //        // modify velocity if it is pushing into a collider
        //        float projection = Vector2.Dot(_velocity, currentNormal);
        //        if (projection < 0) {
        //            _velocity = _velocity + projection * currentNormal;
        //        }

        //        float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
        //        distance = modifiedDistance < distance ? modifiedDistance : distance;
        //    }
        //}

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }
}
