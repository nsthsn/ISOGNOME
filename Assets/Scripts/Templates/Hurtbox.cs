using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

/// <summary>
/// Manages being hit by Hitboxes.
/// 
/// Is triggered.
/// </summary>
public class Hurtbox : MonoBehaviour
{

    [Flags]
    public enum HitState
    {
        Hitting,
        Inactive
    }

    protected Collider2D[] _hurtBuffer = new Collider2D[16];
    protected List<Collider2D> _hurtList = new List<Collider2D>();

    protected Collider2D _hitCollider;
    protected ContactFilter2D _contactFilter;

    protected StateMachine<HitState> _hitFSM = new StateMachine<HitState>();
    private void Start() {
        _hitFSM.AddState(HitState.Hitting, HitStart, HitUpdate, HitStop);
        _hitFSM.AddState(HitState.Inactive, null, null, null);

        _hitFSM.CurrentState = HitState.Inactive;

        _hitCollider = GetComponent<Collider2D>();

        int layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);

        _contactFilter.useTriggers = true;
        _contactFilter.SetLayerMask(layerMask);
        _contactFilter.useLayerMask = true;
    }

    public void Activate() {
        if (_hitFSM.CurrentState != HitState.Hitting) {
            _hitFSM.CurrentState = HitState.Hitting;
        }
        
    }
    public void Deactivate() {
        if (_hitFSM.CurrentState != HitState.Inactive) {
            _hitFSM.CurrentState = HitState.Inactive;
        }
    }
    private void FixedUpdate() {
        _hitFSM.Update();
    }
    void HitStart() {
    }
    void HitUpdate() {
        int count = _hitCollider.OverlapCollider(_contactFilter, _hurtBuffer);

        for (int i = 0; i < count; i++) {
            if (!_hurtList.Contains(_hurtBuffer[i])) {
                _hurtList.Add(_hurtBuffer[i]);
                _hurtBuffer[i].gameObject.GetComponent<Hitbox>().Hit();
                Debug.Log("adding");
            }
        }
    }
    void HitStop() {
        _hurtList.Clear();
    }
}
