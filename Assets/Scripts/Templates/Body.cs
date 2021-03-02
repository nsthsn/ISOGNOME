using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template pattern for bodies.
/// </summary>
public abstract class Body : MonoBehaviour
{
    public abstract bool IsGrounded();
    public abstract void Jump();
    public abstract void Move(Vector2 direction);
    public abstract void DoGravity();
}
