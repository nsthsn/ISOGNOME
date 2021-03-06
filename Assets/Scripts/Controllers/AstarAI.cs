using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarAI : MonoBehaviour
{
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Seeker seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    public void OnPathComplete(Path p) {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
