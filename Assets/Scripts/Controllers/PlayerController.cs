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
        _playerState.AddState(PlayerState.Idle, null, null, null);
        _playerState.AddState(PlayerState.Move, null, null, null);
        _playerState.AddState(PlayerState.Jump, null, null, null);

        _body = GetComponent<Body>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        _body.DoGravity();
    }

    bool CheckCanChangeState(PlayerState nextState) {
        bool rtn = false;

        switch(nextState) {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Jump:
                break;
        }

        return rtn;
    }
}
