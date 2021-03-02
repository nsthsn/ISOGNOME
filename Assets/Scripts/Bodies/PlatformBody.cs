using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBody : Body
{
    Vector2 _velocity = Vector2.zero;
    Rigidbody2D _rb2d;

    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

    void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void OnStart() {
        _contactFilter.useTriggers = false;
        _contactFilter.useLayerMask = true;
    }

    public override bool IsGrounded() {
        bool rtn = false;

        return rtn;
    }
    public override void Jump() {

    }
    public override void Move(Vector2 direction) {

    }

    public override void DoGravity() {
        _velocity += Physics2D.gravity * Time.deltaTime;

        DoMovement(_velocity, true);
    }

    void DoMovement(Vector2 move, bool moveY) {
        float distance = move.magnitude;

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }
}
