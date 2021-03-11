using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

/// <summary>
/// SRP : Manage Player State
/// Class is used by input controllers to attempt changes to player state.
/// Class manages resources to affect player state change.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Flags]
    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        Attack
    };
    StateMachine<PlayerState> _playerState = new StateMachine<PlayerState>();

    [Flags]
    public enum FaceState {
        Right,
        Left
    }
    StateMachine<FaceState> _faceState = new StateMachine<FaceState>();
    
    Body _body;

    Vector2 _moveInput;
    Vector2 _lastMoveInput;

    Hurtbox _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Body>();

        _weapon = GetComponentInChildren<Hurtbox>();

        _playerState.AddState(PlayerState.Idle, null, IdleUpdate, null);
        _playerState.AddState(PlayerState.Move, MoveStart, MoveUpdate, MoveStop);
        _playerState.AddState(PlayerState.Jump, JumpStart, JumpUpdate, JumpStop);
        _playerState.AddState(PlayerState.Attack, AttackStart, AttackUpdate, AttackStop);

        _playerState.CurrentState = PlayerState.Idle;

        _faceState.AddState(FaceState.Left, LeftStart, null, null);
        _faceState.AddState(FaceState.Right, RightStart, null, null);

        _faceState.CurrentState = FaceState.Right;
    }
    void FixedUpdate() {
        _playerState.Update();
        _faceState.Update();
    }

    bool CanChangeState(PlayerState nextState) {
        bool rtn = false;

        switch(nextState) {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Jump:
                if(_playerState.CurrentState != PlayerState.Jump) {
                    rtn = true;
                }
                break;
            case PlayerState.Attack:
                rtn = true;
                break;
        }

        return rtn;
    }
    void TryChangeFace() {
        if (_moveInput.x < 0 && _faceState.CurrentState == FaceState.Right) {
            _faceState.CurrentState = FaceState.Left;
        } else if (_moveInput.x > 0 && _faceState.CurrentState == FaceState.Left) {
            _faceState.CurrentState = FaceState.Right;
        }
    }
    public void TryStateChange(PlayerState tryState, Vector2? data = null) {
        if(data != null) {
            // null coalescing operator
            _moveInput = data ?? Vector2.zero;
        }
        if(CanChangeState(tryState)) {
            _playerState.CurrentState = tryState;
        }
    }
    // MovementUpdate
    void PhysicsUpdate() {
        _body.Move(_moveInput);
        _body.DoGravity();
        TryChangeFace();
    }

    //PlayerState.Idle
    void IdleUpdate() {
        PhysicsUpdate();
    }
    // PlayerState.Jump
    void JumpStart() {
        _body.Jump();
    }
    void JumpUpdate() {
        if(_body.Grounded) {
            _playerState.CurrentState = PlayerState.Idle;
        }
        PhysicsUpdate();
    }
    void JumpStop() { 
    
    }
    // PlayerState.Move
    void MoveStart() {

    }
    void MoveUpdate() {
        PhysicsUpdate();
    }
    void MoveStop() {

    }
    // PlayerState.Attack
    void AttackStart() {
        _weapon.Activate();
    }
    void AttackUpdate() {
        PhysicsUpdate();
    }
    void AttackStop() {

    }

    // FaceState
    void RightStart() {
        transform.SetScaleX(1);
    }
    void LeftStart() {
        transform.SetScaleX(-1);
    }
}
