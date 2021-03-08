using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template pattern for bodies.
/// </summary>
public abstract class Body : MonoBehaviour
{
    protected bool _grounded = false;
    public bool Grounded {get{ return _grounded; }}


    public abstract void Jump();
    public abstract void Move(Vector2 direction, bool yMove = false);
    public abstract void DoGravity();
}
