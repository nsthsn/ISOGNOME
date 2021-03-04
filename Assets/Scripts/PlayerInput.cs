using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SRP : Accepts user input and commands PlayerController
/// </summary>
public class PlayerInput : MonoBehaviour
{
    PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            _playerController.TryStateChange(PlayerController.PlayerState.Jump);
        }

        Vector2 moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) {
            moveInput = Vector2.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveInput = Vector2.right;
        }

        // all movements are valid movements! so well try this every frame
        _playerController.TryStateChange(PlayerController.PlayerState.Move, moveInput);
    }
}
