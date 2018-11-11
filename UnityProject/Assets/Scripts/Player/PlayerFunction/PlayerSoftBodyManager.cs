using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GK;

/// <summary>
/// Setup and update the player body's soft body mesh
/// </summary>
public class PlayerSoftBodyManager : MonoBehaviour
{
    public List<Transform> vertexTransforms; // The list of transforms that represents every possible vertex
    public MeshFilter playerBodyMeshFilter; // The mesh filter that has the player body soft body mesh
    public Transform playerTransform; // The player's transform

    public Mesh playerBodyMesh; // The mesh for the player body soft body mesh
    public List<int> triangleVertexIndexList; // The list of int with the index of vertex for triangles
    public int[] triangleVertexIndexArray; // The array of int with the index of vertex for triangles
    public List<Vector3> meshVerticesPositionList; // The list of the vector3 that represents the position of the vertices for the player body mesh
    public ConvexHullCalculator convexHullCalculator; // The convex hull calculator instance
    public List<Vector3> convexHullVerticesPositionList; // The list of the vector3 that represents the position of the convex hull vertices for the player body mesh
    public List<Vector3> convexHullNormalsList; // The list of the vector3 that represents the normals of the convex hull triangles for the player body mesh

    // Use this for initialization
    void Start()
    {
        // Initial setup
        InitialSetup();
        // Get all the triangle face vertex index
        //GetAllTriangles();

        // Make a convex hull mesh for the player
        GetConvexHullForPlayerBodyMesh();
        // Update the transform list according to the order in convexHullVerticesPositionList calculated by convexHullCalculator
        UpdateVertexTransformsList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //UpdateAllTriangles();
        //UpdateBodyMesh();
        UpdateConvexHullMesh();
    }

    /// <summary>
    /// Setup this class, such as initiate List<>
    /// </summary>
    public void InitialSetup()
    {
        // Initialize the meshVerticesPositionList
        meshVerticesPositionList = new List<Vector3>();
        // Initialize the triangleVertexIndexList
        triangleVertexIndexList = new List<int>();

        // Create and assign new mesh instance
        playerBodyMesh = new Mesh();
        playerBodyMeshFilter.mesh = playerBodyMesh;

        // Create new ConvexHullCalculator instance
        convexHullCalculator = new ConvexHullCalculator();
    }

    /// <summary>
    /// Use the convex hull calculator to get a convex hull for the player soft body mesh
    /// </summary>
    public void GetConvexHullForPlayerBodyMesh()
    {
        // Add new positions
        foreach (Transform t in vertexTransforms)
        {
            meshVerticesPositionList.Add(playerTransform.InverseTransformPoint(t.position));
        }

        // Get convex hull
        convexHullCalculator.GenerateHull(meshVerticesPositionList, false, ref convexHullVerticesPositionList, ref triangleVertexIndexList, ref convexHullNormalsList);

        // Create the mesh
        playerBodyMeshFilter.mesh.Clear();
        playerBodyMeshFilter.mesh.vertices = convexHullVerticesPositionList.ToArray();
        playerBodyMeshFilter.mesh.triangles = triangleVertexIndexList.ToArray();
        playerBodyMeshFilter.mesh.normals = convexHullVerticesPositionList.ToArray();
    }

    /// <summary>
    /// Update the index of the vertexTransforms list according to the order in the convexHullVerticesPositionList
    /// </summary>
    public void UpdateVertexTransformsList()
    {
        // Store the initial transform list
        Transform[] initialTransformList = vertexTransforms.ToArray();
        // Clear the vertexTransforms list
        vertexTransforms.RemoveRange(0, vertexTransforms.Count);

        // Add the transform back in the correct order
        foreach (Vector3 v in convexHullVerticesPositionList)
        {
            foreach (Transform t in initialTransformList)
            {
                // If the position match
                if (playerTransform.InverseTransformPoint(t.position) == v)
                {
                    // Insert the transform back in
                    vertexTransforms.Add(t);
                }
            }
        }
    }

    /// <summary>
    /// Update the soft body mesh created by the convex hull calculator
    /// </summary>
    public void UpdateConvexHullMesh()
    {
        //GetConvexHullForPlayerBodyMesh();

        // Clear the position list
        meshVerticesPositionList.RemoveRange(0, meshVerticesPositionList.Count);

        // Add new positions
        foreach (Transform t in vertexTransforms)
        {
            meshVerticesPositionList.Add(playerTransform.InverseTransformPoint(t.position));
        }

        //playerBodyMeshFilter.mesh.Clear();
        playerBodyMeshFilter.mesh.vertices = meshVerticesPositionList.ToArray();
        //playerBodyMeshFilter.mesh.triangles = triangleVertexIndexList.ToArray();
        //playerBodyMeshFilter.mesh.normals = convexHullVerticesPositionList.ToArray();

        // Recalculate the normal and tangent for the player body mesh
        playerBodyMeshFilter.mesh.RecalculateNormals();
        playerBodyMeshFilter.mesh.RecalculateTangents();

        // Update physics mesh collider
        playerBodyMeshFilter.GetComponent<MeshCollider>().sharedMesh = playerBodyMesh;
    }

    ///// <summary>
    ///// Get all the possible triangle combination of the vertex index (including both the clockwise and counter-clockwise sides)
    ///// </summary>
    //public void GetAllTriangles()
    //{
    //    for (int i = 0; i < vertexTransforms.Count - 2; i++)
    //    {
    //        for (int j = i + 1; j < vertexTransforms.Count - 1; j++)
    //        {
    //            for (int k = j + 1; k < vertexTransforms.Count; k++)
    //            {
    //                // Add one triangle side
    //                triangleVertexIndexList.Add(i);
    //                triangleVertexIndexList.Add(j);
    //                triangleVertexIndexList.Add(k);

    //                // Add the other triangle side
    //                triangleVertexIndexList.Add(k);
    //                triangleVertexIndexList.Add(j);
    //                triangleVertexIndexList.Add(i);
    //            }
    //        }
    //    }

    //    // Copy the index list to the index array
    //    triangleVertexIndexArray = triangleVertexIndexList.ToArray();
    //}

    /// <summary>
    /// Get all the possible triangle combination of the vertex index (including both the clockwise and counter-clockwise sides)
    /// </summary>
    public void UpdateAllTriangles()
    {
        // Clear the triangleVertexIndexList 
        triangleVertexIndexList.RemoveRange(0, triangleVertexIndexList.Count);

        for (int i = 0; i < vertexTransforms.Count - 2; i++)
        {
            for (int j = i + 1; j < vertexTransforms.Count - 1; j++)
            {
                for (int k = j + 1; k < vertexTransforms.Count; k++)
                {
                    //// Test
                    //if (i == 6 && j == 8 && k == 10)
                    //{
                    //    print("");
                    //}
                    ////

                    // If the triangle should be added
                    if (CheckIfTriangleIsSurface(i, j, k))
                    {
                        // Add one triangle side
                        triangleVertexIndexList.Add(i);
                        triangleVertexIndexList.Add(j);
                        triangleVertexIndexList.Add(k);
                    }

                    // If the triangle should be added
                    if (CheckIfTriangleIsSurface(k, j, i))
                    {
                        // Add the other triangle side
                        triangleVertexIndexList.Add(k);
                        triangleVertexIndexList.Add(j);
                        triangleVertexIndexList.Add(i);
                    }
                }
            }
        }

        // Copy the index list to the index array
        triangleVertexIndexArray = triangleVertexIndexList.ToArray();
    }

    /// <summary>
    /// Check if a triangle is the out-most triangle that no vertex connecting the center of the mesh is intersecting it
    /// </summary>
    /// <param name="vertexA"></param>
    /// <param name="vertexB"></param>
    /// <param name="vertexC"></param>
    /// <returns></returns>
    public bool CheckIfTriangleIsSurface(int vertexA, int vertexB, int vertexC)
    {
        // Get the triangle points
        //Vector3 p1 = playerTransform.InverseTransformPoint(vertexTransforms[vertexA].position);
        //Vector3 p2 = playerTransform.InverseTransformPoint(vertexTransforms[vertexB].position);
        //Vector3 p3 = playerTransform.InverseTransformPoint(vertexTransforms[vertexC].position);
        Vector3 p1 = vertexTransforms[vertexA].position;
        Vector3 p2 = vertexTransforms[vertexB].position;
        Vector3 p3 = vertexTransforms[vertexC].position;

        // Check for segment from two vertices that are not the checked triangle's
        for (int i = 0; i < vertexTransforms.Count - 1; i++)
        {
            if (i != vertexA && i != vertexB && i != vertexC)
            {
                for (int j = i + 1; j < vertexTransforms.Count; j++)
                {
                    if (j != vertexA && j != vertexB && j != vertexC)
                    {
                        //// Get the ray to be checked
                        //Ray lineSegmentToCheck =
                        //    //new Ray(playerTransform.position, (playerTransform.InverseTransformPoint(vertexTransforms[i].position) - playerTransform.position));
                        //    new Ray(playerTransform.position, (vertexTransforms[i].position - playerTransform.position));

                        // If the ray intersect the triangle, then don't add the triangle
                        if (CheckLineSegmentTriangleIntersaction.SegmentIntersect(p1, p2, p3, vertexTransforms[i].position, vertexTransforms[j].position))
                        {
                            // Test
                            if (vertexA == 6 && vertexB == 8 && vertexC == 10)
                            {
                                Debug.DrawLine(p1, p2, Color.blue);
                                Debug.DrawLine(p2, p3, Color.blue);
                                Debug.DrawLine(p1, p3, Color.blue);
                                Debug.DrawLine(vertexTransforms[i].position, vertexTransforms[j].position, Color.red);
                            }
                            //

                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Return the vertex coordinates for the mesh
    /// </summary>
    /// <returns></returns>
    public Vector3[] GetBodyMeshVertices()
    {
        // Clear the position list
        meshVerticesPositionList.RemoveRange(0, meshVerticesPositionList.Count);

        // Add new positions
        foreach (Transform t in vertexTransforms)
        {
            meshVerticesPositionList.Add(playerTransform.InverseTransformPoint(t.position));
        }

        return meshVerticesPositionList.ToArray();
    }

    /// <summary>
    /// Update the player body soft body mesh
    /// </summary>
    public void UpdateBodyMesh()
    {
        // Clear old mesh data
        playerBodyMeshFilter.mesh.Clear();

        // Put new mesh data
        playerBodyMeshFilter.mesh.vertices = GetBodyMeshVertices();
        playerBodyMeshFilter.mesh.triangles = triangleVertexIndexArray;

        // Recalculate the normal and tangent for the player body mesh
        playerBodyMeshFilter.mesh.RecalculateNormals();
        playerBodyMeshFilter.mesh.RecalculateTangents();

        // Update physics mesh collider
        playerBodyMeshFilter.GetComponent<MeshCollider>().sharedMesh = playerBodyMesh;
    }
}
