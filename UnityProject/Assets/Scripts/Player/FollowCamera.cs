﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public float chaseSpeed;
    public GameObject target;
    public Vector3 offset;

    Vector3 targetPos;

    //Test
    public bool test;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (test)
        {
            transform.eulerAngles += Vector3.up * 10.0f * Time.fixedDeltaTime;
            return;
        }

        if (target)
        {
            Vector3 posNoY = transform.position;
            posNoY.y = target.transform.position.y;

            Vector3 targetDirection = (target.transform.position - posNoY);

            interpVelocity = targetDirection.magnitude * chaseSpeed;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }
}

// Original post with image here  >  http://unity3diy.blogspot.com/2015/02/unity-2d-camera-follow-script.html