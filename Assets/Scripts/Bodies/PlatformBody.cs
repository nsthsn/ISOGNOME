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
    float _jumpHeight = 3.25f;
    float _timeToJumpHeight = .5f;
    float _timeToFall = .353f;

    // move levers

    // control variables
    protected Vector2 _velocity = Vector2.zero;
    protected Rigidbody2D _rb2d;
    protected bool _grounded = false;

    // collision variables
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    protected Vector2 _groundNormal;

    // jump variables
    float _jumpVelocity = 0;
    float _currentGravity = 0;
    float _baseGravity = 0;
    float _downGravity = 0;

    // 

    void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start() {
        int layerMask = LayerMask.NameToLayer("Terrain");

        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(layerMask);
        _contactFilter.useLayerMask = true;

        _baseGravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToJumpHeight, 2);
        _downGravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToFall, 2);
    }
    public override bool IsGrounded() {
        bool rtn = false;

        return rtn;
    }
    public override void Jump() {

    }
    public override void Move(Vector2 direction) {

    }
    public override void DoGravity() {
        _currentGravity = _baseGravity;

        if(_velocity.y <= 0) {
            _currentGravity = _downGravity;
        }

        float moveStep = ((_velocity + Vector2.up * _currentGravity * Time.deltaTime * 0.5f) * Time.deltaTime).y;
        
        _velocity.y += moveStep * Time.deltaTime;
        _grounded = false;

        DoMovement(_velocity, true);
    }

    void DoMovement(Vector2 move, bool moveY) {
        float distance = move.magnitude;

        if(distance >= _minMoveDistance) {
            int count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);
            _hitBufferList.Clear();

            for (int i = 0; i <count; i++) {
                _hitBufferList.Add(_hitBuffer[i]);

                Vector2 currentNormal = _hitBufferList[i].normal;

                // is this the ground or a wall?
                // this will not handle sliding down slopes as is
                if(currentNormal.y < minGroundNormalY) {
                    _grounded = true;
                    if(moveY) {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                // modify velocity if it is pushing into a collider
                float projection = Vector2.Dot(_velocity, currentNormal);
                if(projection < 0) {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            for(int i = 0; i < _hitBufferList.Count; i++) {

            }

            if(count > 0) {
                Debug.Log(count);
            }
        }

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }
}
