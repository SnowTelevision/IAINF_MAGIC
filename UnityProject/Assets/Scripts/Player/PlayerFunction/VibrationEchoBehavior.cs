using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about an "echo projectile"
/// </summary>
public class VibrationEchoBehavior : MonoBehaviour
{
    public LayerMask raycastLayerMask; // The layer(s) that can be collided with the raycast which detects non-moving objects between the player and the reflected echo
    public GameObject echoCreationMark; // The mark that indicates the echo's originated position and its direction, will disappear
    public float creationMarkLastingDuration; // How long does the creation mark last

    public bool echoHit; // Has the echo projectile hit something
    public Vector3 echoCollidingPosition; // Where does this echo collides (bounce back)
    public float echoBounceBackTime; // The time when the echo is bounced back after first collision
    public List<Vector2> blockedReflectedEchoAngleRanges; // Stores what angle ranges are blocked by obstacles before it can hits the player
    public float echoTravelSpeed; // How fast the echo will travel
    public float echoCreatedTime; // When is the echo created

    // Use this for initialization
    void Start()
    {
        echoCreatedTime = Time.time;
        echoCreationMark.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //if (GetComponent<MeshRenderer>().enabled && Time.time - echoCreatedTime > 0.5)
        //{
        //    GetComponent<MeshRenderer>().enabled = false;
        //}

        // Destroy the creation mark after its duration
        if (Time.time - echoCreatedTime > creationMarkLastingDuration)
        {
            DeleteCreationMark();
        }
    }

    private void FixedUpdate()
    {
        // Moves the reflected echo back to the player
        MoveReflectedEcho();
    }

    /// <summary>
    /// When the echo projectile or the reflected echo sphere collides something
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //print(other.name + " enter");
        // If the echo projectile didn't hit on anything yet (means this collision is not from the reflected echo)
        if (!echoHit)
        {
            echoCreationMark.transform.LookAt(transform, Vector3.up); // Make the creation mark points towards the echo's direction

            // If the echo projectile collide with something that is not the player
            if (!other.GetComponentInParent<PlayerInfo>())
            {
                ProjectileEchoHitObject();
            }
        }
        else
        {
            //print(other.name + " enter");
            //print(other.transform.parent.name + " enter parent");
            CheckReflectedEchoCollisions(other);
        }
    }

    /// <summary>
    /// When the reflected echo is in a non-moving object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (echoHit && other.tag != "MovingEnemy" && !other.GetComponentInParent<PlayerInfo>())
        {
            PlayerInfo.sPlayerInfo.ReflectedEchoTravelInObjectHaptic();
        }
    }

    /// <summary>
    /// When the projectile echo created by the player hits an object
    /// </summary>
    public void ProjectileEchoHitObject()
    {
        echoHit = true;

        // Deletes the echo creation mark
        if (echoCreationMark != null)
        {
            DeleteCreationMark();
        }

        //GetComponent<SphereCollider>().radius = 0; // Make the echo sphere's radius start at 0
        GetComponent<Rigidbody>().velocity = Vector3.zero; // Stop the projectile echo and ready to create reflected echo sphere
        echoCollidingPosition = transform.position; // Mark the position where the echo hit first object and bounce back
        echoBounceBackTime = Time.time;
        PlayerInfo.sPlayerInfo.EchoBounceBackHaptic(); // Haptic feedback to let the player knows that the echo hit an object and begin to come back
    }

    ///// <summary>
    ///// Creates a reflected echo by make an expanding sphere trigger collider
    ///// </summary>
    //public void CreatingReflectedEcho()
    //{

    //}

    ///// <summary>
    ///// Expand the reflected echo sphere trigger
    ///// </summary>
    //public void ExpandReflectedEcho()
    //{
    //    if (echoHit)
    //    {
    //        GetComponent<SphereCollider>().radius += Time.deltaTime * echoTravelSpeed / transform.localScale.x;
    //    }
    //}

    /// <summary>
    /// Moves the reflected echo back to the player
    /// </summary>
    public void MoveReflectedEcho()
    {
        // If the echo is bouncing back
        if (echoHit)
        {
            transform.position = echoCollidingPosition +
                                 Vector3.Normalize(PlayerInfo.sPlayerInfo.transform.position - echoCollidingPosition) *
                                 (Time.time - echoBounceBackTime) * echoTravelSpeed;
        }
    }

    /// <summary>
    /// Check what object the reflect echo collided with
    /// </summary>
    /// <param name="collidingObject"></param>
    public void CheckReflectedEchoCollisions(Collider collidingObject)
    {
        // If the reflected echo hits the player
        if (collidingObject.GetComponentInParent<PlayerInfo>())
        {
            ReflectedEchoCollidePlayer(CalculateReflectedEchoCollidingDirection(collidingObject));
        }
        //// If the reflected echo hits moving enemies
        //else if (collidingObject.tag == "MovingEnemy")
        //{
        //    //CalculateBlockedReflectedEchoAngleRange((SphereCollider)collidingObject);

        //}
        // If the reflected echo hits objects
        else
        {
            PlayerInfo.sPlayerInfo.ReflectedEchoHitsEdgeHaptic();
        }
    }

    /// <summary>
    /// Calculate and return the angle between the vector of the center of the reflected echo to the reflected echo's colliding position and positive z
    /// </summary>
    /// <param name="collidingObject"></param>
    /// <returns></returns>
    public float CalculateReflectedEchoCollidingDirection(Collider collidingObject)
    {
        // Get the colliding object's position on the X-Z plain relative to the reflected echo's center
        Vector3 collidingObjectRelativePositionXZ =
            new Vector3(collidingObject.transform.position.x - transform.position.x, 0, collidingObject.transform.position.z - transform.position.z);
        // Get the angle
        float collidingDirection = Vector3.Angle(collidingObjectRelativePositionXZ, Vector3.forward);
        // If the object is on the left side, then make the angle negative
        if (collidingObjectRelativePositionXZ.x < 0)
        {
            collidingDirection *= -1f;
        }

        return collidingDirection;
    }

    ///// <summary>
    ///// Calculate the angle range of the reflected echo when it hits a non-static object and add the range to the total blocked range
    ///// (so it won't collider with walls or other objects that are not moving)
    ///// </summary>
    ///// <param name="blockingObject"></param>
    //public void CalculateBlockedReflectedEchoAngleRange(SphereCollider blockingObject)
    //{
    //    //SphereCollider blockingObjectCollider = blockingObject.GetComponent<SphereCollider>();
    //    float collidingColliderRadius = blockingObject.radius; // How large is the blocking object
    //    // How far is the blocking object from the center of the reflected echo
    //    float blockingObjectDistance = Vector3.Magnitude(blockingObject.transform.position - transform.position);
    //    float collidingDirection = CalculateReflectedEchoCollidingDirection(blockingObject); // Get the relative direction of the blocking object
    //    // Calculate the angle range from the center of the blocking object to its side
    //    float halfAngleRange = Mathf.Atan(collidingColliderRadius / blockingObjectDistance) * Mathf.Rad2Deg;

    //    // Meaning the angle to the clockwise direction is overlaping to the -180 to 0 range
    //    if (collidingDirection + halfAngleRange > 180)
    //    {
    //        blockedReflectedEchoAngleRanges.Add(new Vector2(collidingDirection - halfAngleRange, 180)); // Adding the range less than 180
    //        blockedReflectedEchoAngleRanges.Add(new Vector2(-180, -180 + (collidingDirection + halfAngleRange - 180))); // Adding the range larger than -180
    //    }
    //    // Meaning the angle to the counter-clockwise direction is overlaping to the 0 to 180 range
    //    else if (collidingDirection - halfAngleRange < -180)
    //    {
    //        blockedReflectedEchoAngleRanges.Add(new Vector2(-180, collidingDirection + halfAngleRange)); // Adding the range larger than -180
    //        blockedReflectedEchoAngleRanges.Add(new Vector2(0, (-(collidingDirection - halfAngleRange) - 180))); // Adding the range larger than 0
    //    }
    //    // Meaning the angle range is between -180 to 180
    //    else
    //    {
    //        blockedReflectedEchoAngleRanges.Add(new Vector2(collidingDirection - halfAngleRange, collidingDirection + halfAngleRange));
    //    }
    //}

    /// <summary>
    /// When the reflected echo trigger collided with the player, 
    /// check if the echo in the player's direction is already blocked by something else, or have non-moving objects along the way
    /// if not, creates controller vibration
    /// </summary>
    /// <param name="collidingDirection"></param>
    public void ReflectedEchoCollidePlayer(float collidingDirection)
    {
        //if (!CheckBlockedReflectedEchoAngleRange(collidingDirection) &&
        //    !CheckNonMovingObjectsBetweenReflectedEchoAndPlayer())
        //{
        //    PlayerInfo.sPlayerInfo.ReceivedReflectedEcho();
        //}

        PlayerInfo.sPlayerInfo.ReceivedReflectedEcho();
        Destroy(gameObject); // Destroy this echo when its reflected echo reaches the player
    }

    ///// <summary>
    ///// When the reflected echo hits the player, if the angle is not in the blocked angle range (created by moving objects),
    ///// check if there is a non-moving object in front of the player (true if there is an object between the player and the reflected echo)
    ///// </summary>
    //public bool CheckNonMovingObjectsBetweenReflectedEchoAndPlayer()
    //{
    //    RaycastHit hit;
    //    // Shoot a ray in the player's direction from the center of the reflected echo
    //    Physics.Raycast(transform.position, PlayerInfo.sPlayerInfo.transform.position - transform.position, out hit, Mathf.Infinity, raycastLayerMask);

    //    // If the ray hits the player means there is no non-moving obstacles in-between
    //    if (hit.collider.GetComponentInParent<PlayerInfo>())
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    ///// <summary>
    ///// Check if the angle where the player collides the reflected echo is in the range that is blocked by other moving object
    ///// (returns true if the angle is blocked, false if the angle is valid)
    ///// </summary>
    ///// <param name="playerDirection"></param>
    ///// <returns></returns>
    //public bool CheckBlockedReflectedEchoAngleRange(float playerDirection)
    //{
    //    foreach (Vector2 range in blockedReflectedEchoAngleRanges)
    //    {
    //        if (playerDirection > range.x && playerDirection < range.y)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    /// <summary>
    /// Destroy the echo creation mark
    /// </summary>
    public void DeleteCreationMark()
    {
        Destroy(echoCreationMark);
        echoCreationMark = null;
    }
}
