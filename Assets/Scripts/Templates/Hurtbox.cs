using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages being hit by Hitboxes.
/// 
/// Is triggered.
/// </summary>
public class Hurtbox : MonoBehaviour
{
    [Flags]
    enum HurtState {
        Hittable,
        Inactive
    }
}
