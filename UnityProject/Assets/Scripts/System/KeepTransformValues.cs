using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTransformValues : MonoBehaviour
{
    public bool keepPosiX;
    public float posiX;
    public bool keepPosiY;
    public float posiY;
    public bool keepPosiZ;
    public float posiZ;
    public bool keepEulerX;
    public float eulerX;
    public bool keepEulerY;
    public float eulerY;
    public bool keepEulerZ;
    public float eulerZ;

    public Vector3 positionToKeep;
    public Vector3 eulerAnglesToKeep;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        positionToKeep = transform.localPosition;
        eulerAnglesToKeep = transform.localEulerAngles;

        if(keepPosiX)
        {
            positionToKeep.x = posiX;
        }
        if (keepPosiY)
        {
            positionToKeep.y = posiY;
        }
        if (keepPosiZ)
        {
            positionToKeep.z = posiZ;
        }
        if (keepEulerX)
        {
            eulerAnglesToKeep.x = eulerX;
        }
        if (keepEulerY)
        {
            eulerAnglesToKeep.y = eulerY;
        }
        if (keepEulerZ)
        {
            eulerAnglesToKeep.z = eulerZ;
        }

        transform.localPosition = positionToKeep;
        transform.localEulerAngles = eulerAnglesToKeep;
    }
}
