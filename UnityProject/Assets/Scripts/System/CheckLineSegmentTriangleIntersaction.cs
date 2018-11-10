using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All credit goes to "nu-assets" in the post:
/// https://answers.unity.com/questions/861719/a-fast-triangle-triangle-intersection-algorithm-fo.html
/// 
/// Checks if the specified ray hits the triagnlge descibed by p1, p2 and p3.
/// Möller–Trumbore ray-triangle intersection algorithm implementation.
/// </summary>
public class CheckLineSegmentTriangleIntersaction : MonoBehaviour
{
    /// <summary>
    /// Checks if the specified segment intersect the triagnlge descibed by p1, p2 and p3.
    /// Möller–Trumbore ray-triangle intersection algorithm implementation.
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="sS"></param>
    /// <param name="sE"></param>
    /// <returns><c>true</c> when the segment intersect the triangle, otherwise <c>false</c></returns>
    public static bool SegmentIntersect(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sS, Vector3 sE)
    {
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;

        // Ray of the segment
        Ray ray = new Ray(sS, (sE - sS));

        Vector3 p, q, t;
        float det, invDet, u, v;


        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Mathf.Epsilon && det < Mathf.Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
        {
            // Distance from the ray origin to the intersecting point is <= than the actual segment's length
            if ((Vector3.Dot(e2, q) * invDet) <= Vector3.Distance(sS, sE))
            {
                //segment does intersect
                return true;
            }
        }

        // No hit at all
        return false;
    }

    /// <summary>
    /// Checks if the specified ray hits the triagnlge descibed by p1, p2 and p3.
    /// Möller–Trumbore ray-triangle intersection algorithm implementation.
    /// </summary>
    /// <param name="p1">Vertex 1 of the triangle.</param>
    /// <param name="p2">Vertex 2 of the triangle.</param>
    /// <param name="p3">Vertex 3 of the triangle.</param>
    /// <param name="ray">The ray to test hit for.</param>
    /// <returns><c>true</c> when the ray hits the triangle, otherwise <c>false</c></returns>
    public static bool RayIntersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
    {
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;

        Vector3 p, q, t;
        float det, invDet, u, v;


        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Mathf.Epsilon && det < Mathf.Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
        {
            //ray does intersect
            return true;
        }

        // No hit at all
        return false;
    }
}
