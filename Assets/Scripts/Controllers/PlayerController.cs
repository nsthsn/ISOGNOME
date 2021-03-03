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

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Body>();

        _playerState.AddState(PlayerState.Idle, null, null, null);
        _playerState.AddState(PlayerState.Move, null, null, null);
        _playerState.AddState(PlayerState.Jump, JumpEnter, JumpStay, JumpExit);

        _playerState.CurrentState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        _body.DoGravity();

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

    public void TryStateChange(PlayerState tryState) {
        if(CanChangeState(tryState)) {
            _playerState.CurrentState = tryState;
        }
    }

    void JumpEnter() {
        _body.Jump();
    }
    void JumpStay() {
        if(_body.Grounded) {
            _playerState.CurrentState = PlayerState.Idle;
        }
    }
    void JumpExit() { 
    
    }

}
