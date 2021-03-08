
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

/// <summary>
/// SRP : Manages hitting hurtboxes.
/// </summary>
public class Hitbox : MonoBehaviour
{
    [Flags]
    public enum HitState
    {
        Hitting,
        Inactive
    }

    Collider2D _collider;

    protected StateMachine<HitState> _hitFSM = new StateMachine<HitState>();
    private void Start() {
        _hitFSM.AddState(HitState.Hitting, null, null, null);
        _hitFSM.AddState(HitState.Inactive, null, null, null);

        _hitFSM.CurrentState = HitState.Inactive;
    }
    private void FixedUpdate() {
        _hitFSM.Update();
    }
    void CheckHits() {

    }
}
