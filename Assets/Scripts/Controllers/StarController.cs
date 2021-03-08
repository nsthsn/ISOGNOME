
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StarController : MonoBehaviour
{
    [Flags]
    public enum StarState {
        Idle,
        Seeking,
        Attacking,
    }

    Body _body;
    Transform _target;
    Seeker _seeker;
    Rigidbody2D _rb;

    float speed = 2;
    float nextWaypointDistance = 3;

    Path _path;
    int _currentWaypoint = 0;
    bool _reachedEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Body>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        _seeker = GetComponent<Seeker>();
        _seeker.StartPath(transform.position, _target.position, OnPathComplete);

        InvokeRepeating("UpdatePath", 0, .5f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_path == null) return;

        if(_currentWaypoint >= _path.vectorPath.Count) {
            _reachedEnd = true;
            _currentWaypoint = 0;
            return;
        } else {
            _reachedEnd = false;
        }

        Vector2 direction = ((_path.vectorPath[_currentWaypoint] - transform.position)).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        Debug.Log(direction);
        // we call move twice since this object moves on y
        _body.Move(direction, true);
        _body.Move(direction);
        


        float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance) {
            _currentWaypoint++;
        }
    }
    void UpdatePath() {
        if(_seeker.IsDone()) {
            _seeker.StartPath(transform.position, _target.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p) {
        if(!p.error) {
            _path = p;
            _currentWaypoint = 0;
        }
    }
}
