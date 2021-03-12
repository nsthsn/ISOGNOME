using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// SRP : Accepts user input and commands PlayerController
/// </summary>
public class PlayerInput : MonoBehaviour
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //static void OnLoad() {
    //    var tmp = PlayerInput.Instance;
    //    DontDestroyOnLoad(tmp);
    //}

    PlayerController _playerController;

    Player _rewiredPlayer;
    int _playerID = 0;

    Vector2 _moveInput = Vector2.zero;
    bool _jumpInput = false;
    bool _attackInput = false;


    private void OnDisable() {

    }
    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();

        _rewiredPlayer = ReInput.players.GetPlayer(_playerID);
    }

    // Update is called once per frame
    void Update() {
        GetInput();
        ProcessInput();
    }
    void GetInput() {
        // get the raw axis so keyboard control doesn't simulate ramp up and variable control still works for the controller
        _moveInput.x = _rewiredPlayer.GetAxisRaw("MoveHorizontal");
        _jumpInput = _rewiredPlayer.GetButtonDown("Jump");
        _attackInput = _rewiredPlayer.GetButtonDown("Attack");

        Debug.Log(_moveInput.x);
    }
    void ProcessInput() {
        _playerController.TryStateChange(PlayerController.PlayerState.Move, _moveInput);


        if (_jumpInput) {
            _playerController.TryStateChange(PlayerController.PlayerState.Jump);
        }
        if (_attackInput) {
            _playerController.TryStateChange(PlayerController.PlayerState.Attack);
        }
    }
}
