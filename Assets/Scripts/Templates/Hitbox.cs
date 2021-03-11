
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
    enum HurtState
    {
        Hittable,
        Immune
    }

    public delegate void HitEvent();
    public event HitEvent Publish;

    public void Hit() {
        Publish?.Invoke();
        //Debug.Log(Publish.);
    }
}
