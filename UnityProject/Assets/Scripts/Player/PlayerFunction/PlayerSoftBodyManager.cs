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
    public GameObject softBodyCenter; // The center of the player soft body
    public MeshFilter playerBodyMeshFilter; // The mesh filter that has the player body soft body mesh
    public Transform playerTransform; // The player's transform
    public GameObject playerSoftBodyJointSetup; // The game object that has the joint components that represents the joint to be used for the player soft body
    public bool generateNewVertexGameObjects; // Do we generate a new set of vertex objects for the player soft body
    public Transform meshToGetVertices; // The transform of the mesh used to get the surface vertices
    public GameObject vertexObjectRef; // The reference of the gameobject to be created for mesh vertices
    public float defaultSurfaceVertexDistance; // How long is the default distance from the surface vertex to the center
    // How far away the vertex has to be from the default distance should the center joint start adjusting mass scale
    public float centerJointsMassScaleAdjustmentThreshold;
    public float centerJointsMassScaleMultiplier; // The multiplier for adjusting center spring mass scale

    public SpringJoint surfaceJointRef; // The reference for the joint that connects the surface edges
    public SpringJoint centerJointRef; // The reference for the joint that connects the center to the surface vertex transforms
    public Mesh playerBodyMesh; // The mesh for the player body soft body mesh
    public List<int> triangleVertexIndexList; // The list of int with the index of vertex for triangles
    public int[] triangleVertexIndexArray; // The array of int with the index of vertex for triangles
    public List<Vector3> meshVerticesPositionList; // The list of the vector3 that represents the position of the vertices for the player body mesh
    public ConvexHullCalculator convexHullCalculator; // The convex hull calculator instance
    public List<Vector3> convexHullVerticesPositionList; // The list of the vector3 that represents the position of the convex hull vertices for the player body mesh
    public List<Vector3> convexHullNormalsList; // The list of the vector3 that represents the normals of the convex hull triangles for the player body mesh
    public List<int> jointConnections; // The list of the index of the pairs of vertex transforms that should be connected with a joint (spring joint or other)
    public List<SpringJoint> surfaceJoints; // The joints that connecting surface edges
    public List<SpringJoint> centerJoints; // The joints that connecting center to surface vertices
    public float centerJointDefaultMassScale; // The default mass scale for center joints in current movement mode
    public float centerJointDefaultSpring; // The default spring for center joints in current movement mode
    public float centerJointDefaultDamper; // The default damper for center joints in current movement mode
    public float centerFixedJointDefaultConnectedMassScale; // The default connected mass scale for center fixed joint in current movement mode
    public float vertexRigidbodyDefaultAngularDrag; // The default angular drag for vertex rigidbodies in current movement mode
    public float vertexJointDefaultMassScale; // The default mass scale for vertex joints in current movement mode
    public Vector3 softBodyMeshCenterPosition; // The position of the center of the soft body mesh
    public float softBodyMeshCenterDistance; // The distance from the position of the center of the soft body mesh to the player body center

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
        // Get the int List of the index of the pairs of vertices that should be connected through a joint
        GetPlayerBodySurfaceJointPairs();
        // Add joints to the player soft body
        AddJointsToSurfaceEdges();
        AddJointsToCenter();

        // Get new set of vertex game objects
        if (generateNewVertexGameObjects)
        {
            CreateVertexObjectsOnMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //UpdateAllTriangles();
        //UpdateBodyMesh();

        // Get the position of the center of the soft body mesh
        softBodyMeshCenterPosition = playerBodyMeshFilter.GetComponent<MeshCollider>().bounds.center;
        // Get the distance from the position of the center of the soft body mesh to the player body center
        softBodyMeshCenterDistance = Vector3.Distance(softBodyCenter.transform.position, softBodyMeshCenterPosition);

        //if (!GetComponent<PlayerInfo>().inWater)
        {
            AdjustCenterJointsParameters();
            AdjustSurfaceVertexRigidbodyParameters();
        }

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

        // Get the joint setup
        surfaceJointRef = playerSoftBodyJointSetup.GetComponents<SpringJoint>()[0];
        centerJointRef = playerSoftBodyJointSetup.GetComponents<SpringJoint>()[1];
    }



    /// <summary>
    /// Inceased the mass scale of the spring joint on the soft body center that connects to the vertex objects
    /// 
    /// if the vertices is moved too far
    /// </summary>
    public void AdjustCenterJointsParameters()
    {
        // Update each joint
        foreach (SpringJoint j in centerJoints)
        {
            /// Adjust based on individual vertex distance
            //j.massScale =
            //    centerJointDefaultMassScale * 
            //    (1 + Mathf.Clamp((Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance) -
            //                      centerJointsMassScaleAdjustmentThreshold),
            //                     0, Mathf.Infinity) * centerJointsMassScaleMultiplier);
            j.massScale =
                centerJointDefaultMassScale *
                (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance),
                               6) * 1300);
            j.spring =
                centerJointDefaultSpring *
                (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance),
                               6) * 1300);

            //foreach (SpringJoint jJ in j.connectedBody.GetComponents<SpringJoint>())
            //{
            //    jJ.massScale =
            //        vertexJointDefaultMassScale *
            //        (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance),
            //                       3.2f) * 2f);
            //}

            //j.damper =
            //    centerJointDefaultDamper *
            //    (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, softBodyMeshCenterPosition) - defaultSurfaceVertexDistance),
            //                   6) * 1300);

            /// Adjust based on soft body mesh center distance
            //j.massScale =
            //    centerJointDefaultMassScale *
            //    (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance),
            //                   6) * 1300);
            //j.spring =
            //    centerJointDefaultSpring *
            //    (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, softBodyMeshCenterPosition) - defaultSurfaceVertexDistance),
            //                   6) * 1300);
            //j.damper =
            //    centerJointDefaultDamper *
            //    (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, softBodyMeshCenterPosition) - defaultSurfaceVertexDistance),
            //                   6) * 1300);

            //// Test
            //if (centerJoints.IndexOf(j) == 0)
            //{
            //    print(j.massScale);
            //}
        }

        softBodyCenter.GetComponent<FixedJoint>().connectedMassScale =
            centerFixedJointDefaultConnectedMassScale *
            (1 + Mathf.Pow(Mathf.Abs(softBodyMeshCenterDistance - defaultSurfaceVertexDistance), 6) * 1300);
    }

    /// <summary>
    /// Adjust rigidbody parameters of the surface vertices
    /// 
    /// if the vertices is moved too far
    /// </summary>
    public void AdjustSurfaceVertexRigidbodyParameters()
    {
        // Update each rigidbody
        foreach (SpringJoint j in centerJoints)
        {
            j.massScale =
                centerJointDefaultMassScale *
                (1 + Mathf.Clamp((Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance) -
                                  centerJointsMassScaleAdjustmentThreshold),
                                 0, Mathf.Infinity) * centerJointsMassScaleMultiplier);
            //j.GetComponent<Rigidbody>().angularDrag =
            //    vertexRigidbodyDefaultAngularDrag /
            //    (1 + Mathf.Pow(Mathf.Abs(Vector3.Distance(softBodyCenter.transform.position, j.connectedBody.position) - defaultSurfaceVertexDistance),
            //                   6) * 9600);

            //// Test
            //if (centerJoints.IndexOf(j) == 0)
            //{
            //    print(j.GetComponent<Rigidbody>().angularDrag);
            //}
        }
    }

    /// <summary>
    /// Add joints to the center from the surface vertex transforms
    /// </summary>
    public void AddJointsToCenter()
    {
        // Add joints to connect every surface vertex transforms
        foreach (Transform t in vertexTransforms)
        {
            // Add joint to the center and connect it with the vertex transform
            SpringJoint newJoint = softBodyCenter.AddComponent<SpringJoint>(centerJointRef);
            newJoint.connectedBody = t.gameObject.GetComponent<Rigidbody>();

            // Store joint to the centerJoints list
            centerJoints.Add(newJoint);
        }
    }

    /// <summary>
    /// Add joints to the edges that are on the surface of the player soft body mesh
    /// </summary>
    public void AddJointsToSurfaceEdges()
    {
        // Go through all edge pairs
        for (int i = 0; i < jointConnections.Count; i += 2)
        {
            // Add joint
            SpringJoint newJoint = vertexTransforms[jointConnections[i]].gameObject.AddComponent<SpringJoint>(surfaceJointRef);
            // Connect the joint to the other vertex
            newJoint.connectedBody = (vertexTransforms[jointConnections[i + 1]].GetComponent<Rigidbody>());

            // Store joint to the surfaceJoints list
            surfaceJoints.Add(newJoint);
        }
    }

    /// <summary>
    /// Check which two vertex transforms should have a joint connecting them, get all the pairs and store in a List<int>
    /// </summary>
    public void GetPlayerBodySurfaceJointPairs()
    {
        // Go through all the triangle edges
        for (int i = 0; i < triangleVertexIndexList.Count; i += 3)
        {
            // Check for the edges of a triangle (both directions)
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i], triangleVertexIndexList[i + 1]))
            {
                jointConnections.Add(triangleVertexIndexList[i]);
                jointConnections.Add(triangleVertexIndexList[i + 1]);
            }
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i + 1], triangleVertexIndexList[i + 2]))
            {
                jointConnections.Add(triangleVertexIndexList[i + 1]);
                jointConnections.Add(triangleVertexIndexList[i + 2]);
            }
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i + 2], triangleVertexIndexList[i]))
            {
                jointConnections.Add(triangleVertexIndexList[i + 2]);
                jointConnections.Add(triangleVertexIndexList[i]);
            }
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i + 2], triangleVertexIndexList[i + 1]))
            {
                jointConnections.Add(triangleVertexIndexList[i + 2]);
                jointConnections.Add(triangleVertexIndexList[i + 1]);
            }
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i + 1], triangleVertexIndexList[i]))
            {
                jointConnections.Add(triangleVertexIndexList[i + 1]);
                jointConnections.Add(triangleVertexIndexList[i]);
            }
            if (!ExistsIntPair(jointConnections, triangleVertexIndexList[i], triangleVertexIndexList[i + 2]))
            {
                jointConnections.Add(triangleVertexIndexList[i]);
                jointConnections.Add(triangleVertexIndexList[i + 2]);
            }
        }
    }

    /// <summary>
    /// Check if a pair of int(a, b) exist in a List of pairs of int
    /// </summary>
    /// <param name="pairList"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool ExistsIntPair(List<int> pairList, int a, int b)
    {
        // Go through all int pairs
        for (int i = 0; i < pairList.Count; i += 2)
        {
            // If the first number of the checked pair matches
            if (pairList[i] == a)
            {
                // If the second number of the checked pair matches
                if (pairList[i + 1] == b)
                {
                    return true;
                }
            }
        }

        return false;
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

    ///// <summary>
    ///// Get all the possible triangle combination of the vertex index (including both the clockwise and counter-clockwise sides)
    ///// </summary>
    //public void UpdateAllTriangles()
    //{
    //    // Clear the triangleVertexIndexList 
    //    triangleVertexIndexList.RemoveRange(0, triangleVertexIndexList.Count);

    //    for (int i = 0; i < vertexTransforms.Count - 2; i++)
    //    {
    //        for (int j = i + 1; j < vertexTransforms.Count - 1; j++)
    //        {
    //            for (int k = j + 1; k < vertexTransforms.Count; k++)
    //            {
    //                //// Test
    //                //if (i == 6 && j == 8 && k == 10)
    //                //{
    //                //    print("");
    //                //}
    //                ////

    //                // If the triangle should be added
    //                if (CheckIfTriangleIsSurface(i, j, k))
    //                {
    //                    // Add one triangle side
    //                    triangleVertexIndexList.Add(i);
    //                    triangleVertexIndexList.Add(j);
    //                    triangleVertexIndexList.Add(k);
    //                }

    //                // If the triangle should be added
    //                if (CheckIfTriangleIsSurface(k, j, i))
    //                {
    //                    // Add the other triangle side
    //                    triangleVertexIndexList.Add(k);
    //                    triangleVertexIndexList.Add(j);
    //                    triangleVertexIndexList.Add(i);
    //                }
    //            }
    //        }
    //    }

    //    // Copy the index list to the index array
    //    triangleVertexIndexArray = triangleVertexIndexList.ToArray();
    //}

    ///// <summary>
    ///// Check if a triangle is the out-most triangle that no vertex connecting the center of the mesh is intersecting it
    ///// </summary>
    ///// <param name="vertexA"></param>
    ///// <param name="vertexB"></param>
    ///// <param name="vertexC"></param>
    ///// <returns></returns>
    //public bool CheckIfTriangleIsSurface(int vertexA, int vertexB, int vertexC)
    //{
    //    // Get the triangle points
    //    //Vector3 p1 = playerTransform.InverseTransformPoint(vertexTransforms[vertexA].position);
    //    //Vector3 p2 = playerTransform.InverseTransformPoint(vertexTransforms[vertexB].position);
    //    //Vector3 p3 = playerTransform.InverseTransformPoint(vertexTransforms[vertexC].position);
    //    Vector3 p1 = vertexTransforms[vertexA].position;
    //    Vector3 p2 = vertexTransforms[vertexB].position;
    //    Vector3 p3 = vertexTransforms[vertexC].position;

    //    // Check for segment from two vertices that are not the checked triangle's
    //    for (int i = 0; i < vertexTransforms.Count - 1; i++)
    //    {
    //        if (i != vertexA && i != vertexB && i != vertexC)
    //        {
    //            for (int j = i + 1; j < vertexTransforms.Count; j++)
    //            {
    //                if (j != vertexA && j != vertexB && j != vertexC)
    //                {
    //                    //// Get the ray to be checked
    //                    //Ray lineSegmentToCheck =
    //                    //    //new Ray(playerTransform.position, (playerTransform.InverseTransformPoint(vertexTransforms[i].position) - playerTransform.position));
    //                    //    new Ray(playerTransform.position, (vertexTransforms[i].position - playerTransform.position));

    //                    // If the ray intersect the triangle, then don't add the triangle
    //                    if (CheckLineSegmentTriangleIntersaction.SegmentIntersect(p1, p2, p3, vertexTransforms[i].position, vertexTransforms[j].position))
    //                    {
    //                        // Test
    //                        if (vertexA == 6 && vertexB == 8 && vertexC == 10)
    //                        {
    //                            Debug.DrawLine(p1, p2, Color.blue);
    //                            Debug.DrawLine(p2, p3, Color.blue);
    //                            Debug.DrawLine(p1, p3, Color.blue);
    //                            Debug.DrawLine(vertexTransforms[i].position, vertexTransforms[j].position, Color.red);
    //                        }
    //                        //

    //                        return false;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return true;
    //}

    ///// <summary>
    ///// Return the vertex coordinates for the mesh
    ///// </summary>
    ///// <returns></returns>
    //public Vector3[] GetBodyMeshVertices()
    //{
    //    // Clear the position list
    //    meshVerticesPositionList.RemoveRange(0, meshVerticesPositionList.Count);

    //    // Add new positions
    //    foreach (Transform t in vertexTransforms)
    //    {
    //        meshVerticesPositionList.Add(playerTransform.InverseTransformPoint(t.position));
    //    }

    //    return meshVerticesPositionList.ToArray();
    //}

    ///// <summary>
    ///// Update the player body soft body mesh
    ///// </summary>
    //public void UpdateBodyMesh()
    //{
    //    // Clear old mesh data
    //    playerBodyMeshFilter.mesh.Clear();

    //    // Put new mesh data
    //    playerBodyMeshFilter.mesh.vertices = GetBodyMeshVertices();
    //    playerBodyMeshFilter.mesh.triangles = triangleVertexIndexArray;

    //    // Recalculate the normal and tangent for the player body mesh
    //    playerBodyMeshFilter.mesh.RecalculateNormals();
    //    playerBodyMeshFilter.mesh.RecalculateTangents();

    //    // Update physics mesh collider
    //    playerBodyMeshFilter.GetComponent<MeshCollider>().sharedMesh = playerBodyMesh;
    //}

    /// <summary>
    /// Create a gameobject on each vertex of the given mesh
    /// </summary>
    public void CreateVertexObjectsOnMesh()
    {
        // Get the list of vertices
        List<Vector3> vertices = new List<Vector3>();
        meshToGetVertices.GetComponent<MeshFilter>().mesh.GetVertices(vertices);

        // Instantiate reference game object on each vertex
        List<Vector3> createdVertices = new List<Vector3>(); // The vertices already created

        foreach (Vector3 v in vertices)
        {
            if (!createdVertices.Contains(v))
            {
                createdVertices.Add(v);
                GameObject newVertexGameObject = Instantiate(vertexObjectRef, meshToGetVertices.TransformPoint(v), Quaternion.identity);
                newVertexGameObject.transform.parent = meshToGetVertices;
            }
        }
    }
}
