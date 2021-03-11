using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template pattern for bodies.
/// </summary>
public abstract class Body : MonoBehaviour
{
    // move levers
    protected float _changeMoveTotalTime = .3f;
    protected float _maxSpeed = 14.14f;


    protected bool _grounded = false;
    public bool Grounded {get{ return _grounded; }}

    public abstract void Jump();
    public abstract void Move(Vector2 direction);
    public abstract void DoGravity();
}
