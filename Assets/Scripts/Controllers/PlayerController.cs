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
        Jump
    };
    StateMachine<PlayerState> _playerState = new StateMachine<PlayerState>();
    Body _body;

    Vector2 _moveInput;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Body>();

        _playerState.AddState(PlayerState.Idle, null, IdleUpdate, null);
        _playerState.AddState(PlayerState.Move, MoveStart, MoveUpdate, MoveStop);
        _playerState.AddState(PlayerState.Jump, JumpStart, JumpUpdate, JumpStop);

        _playerState.CurrentState = PlayerState.Idle;
    }
    void FixedUpdate() {
        _playerState.Update();
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
        }

        return rtn;
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
}
