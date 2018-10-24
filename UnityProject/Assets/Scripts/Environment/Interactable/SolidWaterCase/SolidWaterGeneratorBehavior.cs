using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidWaterGeneratorBehavior : MonoBehaviour
{
    public Transform solidWaterCase; // The case this generator belongs to
    public int maxVelocity; // The maximum velocity of the generator
    public int minVelocity; // The minimum velocity of the generator
    public int maxTurnTime; // The maximum duration of one turn during swarm
    public int minTurnTime; // The minimum duration of one turn during swarm
    public int maxFlockAngle; // The maximum offset swarm angle of the generator
    public float maxTurnSpeedWhenFormingCase; // How fast the generators can turn when forming the case

    public int generatorIndex; // This generator's index in the case controller's array
    public Coroutine behaviorCoroutine; // The currently running behavior coroutine
    public bool isFlocking; // Should the generator flocking
    public bool isMovingToTargetPosition; // Should the generator move toward target position then rotate to target rotation
    public Vector3 relativeTargetPosition; // The target position relative to the case when the solid water case is forming
    public Quaternion targetRotation; // The target rotation for the generator when the solid water case is forming
    public Vector3 targetPosition; // The target position for the generator when the solid water case is forming

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    /// <summary>
    /// Let the generator move to the target position to form solid water case
    /// </summary>
    public void StartMoveToTargetPosition()
    {
        // Get the target position and rotation in Quaternion
        targetPosition = solidWaterCase.position + relativeTargetPosition;

        // Start moving the generator
        isMovingToTargetPosition = true;
    }

    /// <summary>
    /// The generator moving to target position and rotate to target angle
    /// </summary>
    public void MoveToTarget()
    {
        if (isMovingToTargetPosition)
        {
            // Move to target position
            if (Vector3.Magnitude(transform.position - targetPosition) > maxVelocity * Time.deltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxVelocity * Time.deltaTime);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), maxTurnSpeedWhenFormingCase * Time.deltaTime);

                return;
            }

            // Rotate to target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) > maxFlockAngle * Time.deltaTime)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxTurnSpeedWhenFormingCase * Time.deltaTime);

                return;
            }

            // Stop moving
            isMovingToTargetPosition = false;

            // Make the generator move with the case
            transform.parent = solidWaterCase;

            // Tell the case controller that this generator is in position
            solidWaterCase.GetComponent<SolidWaterCaseBehavior>().arrivedGeneratorCount++;
        }
    }

    /*
    /// <summary>
    /// The generator moving to target position and rotate to target angle coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToTarget()
    {
        // The euler rotation to lerp
        Vector3 rotation;

        // Get the target position
        Vector3 targetPosition = solidWaterCase.position + relativeTargetPosition;

        /// Rotate the generator towards the target position
        // Get the rotation angles
        float oldX = transform.eulerAngles.x;
        float oldY = transform.eulerAngles.y;
        float oldZ = transform.eulerAngles.z;
        transform.LookAt(targetPosition);
        float newX = transform.eulerAngles.x;
        float newY = transform.eulerAngles.y;
        float newZ = transform.eulerAngles.z;

        // Get the turn duration
        float turnTime = Vector3.Angle(transform.forward, (targetPosition - transform.position)) / maxTurnSpeedWhenFormingCase;

        // Rotate towards the target position
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / turnTime)
        {
            rotation.x = Mathf.LerpAngle(oldX, newX, t);
            rotation.y = Mathf.LerpAngle(oldY, newY, t);
            rotation.z = Mathf.LerpAngle(oldZ, newZ, t);

            transform.eulerAngles = rotation;

            yield return null;
        }

        transform.LookAt(solidWaterCase.position + relativeTargetPosition);

        /// Move the generator towards the target position
        // Get the move duration and the direction vector to the target position
        float moveTime = Vector3.Distance(targetPosition, transform.position) / maxVelocity;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Move towards the target position
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
        {
            transform.position += moveDirection * Time.deltaTime * maxVelocity;

            yield return null;
        }

        transform.position = targetPosition;

        /// Rotate the generator towards the target rotation
        // Get the rotation angles
        oldX = transform.eulerAngles.x;
        oldY = transform.eulerAngles.y;
        oldZ = transform.eulerAngles.z;

        // Get the turn duration
        turnTime = Vector3.Angle(transform.forward, solidWaterCase.GetComponent<SolidWaterCaseBehavior>().UnitCubeCoords[generatorIndex]) / maxTurnSpeedWhenFormingCase;

        // Rotate towards the target rotation
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / turnTime)
        {
            rotation.x = Mathf.LerpAngle(oldX, targetRotation.x, t);
            rotation.y = Mathf.LerpAngle(oldY, targetRotation.y, t);
            rotation.z = Mathf.LerpAngle(oldZ, targetRotation.z, t);

            transform.eulerAngles = rotation;

            yield return null;
        }

        transform.eulerAngles = targetRotation;

        // Tell the case controller that this generator is in position
        solidWaterCase.GetComponent<SolidWaterCaseBehavior>().arrivedGeneratorCount++;
        behaviorCoroutine = null;
    }
    */

    /// <summary>
    /// Let the generator start flocking
    /// </summary>
    public void StartFlocking()
    {
        // Stop any running behaviors
        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }

        // Start the flocking behavior
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        isFlocking = true;
        behaviorCoroutine = StartCoroutine(Flocking());
    }

    /// <summary>
    /// Let the generator stop flocking
    /// </summary>
    public void StopFlocking()
    {
        // Stop the flocking behavior
        isFlocking = false;
        StopCoroutine(behaviorCoroutine);
        behaviorCoroutine = null;

        // Stop the generator from moving
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    /// <summary>
    /// The flocking behavior coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator Flocking()
    {
        // The euler rotation to lerp
        Vector3 rotation;

        // Keep flocking if it should flock
        while (isFlocking)
        {
            // Get the generator speed
            float startSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            float targetSpeed = BetterRandom.betterRandom(minVelocity * 100, maxVelocity * 100) / 100f;

            // Get the swarm turn time
            float turnTime = BetterRandom.betterRandom(minTurnTime * 100, maxTurnTime * 100) / 100f;

            // Get the starting and target rotation angles
            float oldX = transform.eulerAngles.x;
            float oldY = transform.eulerAngles.y;
            float oldZ = transform.eulerAngles.z;
            transform.LookAt(solidWaterCase);
            float newX = transform.eulerAngles.x + BetterRandom.betterRandom(-maxFlockAngle, maxFlockAngle);
            float newY = transform.eulerAngles.y + BetterRandom.betterRandom(-maxFlockAngle, maxFlockAngle);
            float newZ = transform.eulerAngles.z + BetterRandom.betterRandom(-maxFlockAngle, maxFlockAngle);

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / turnTime)
            {
                rotation.x = Mathf.LerpAngle(oldX, newX, t);
                rotation.y = Mathf.LerpAngle(oldY, newY, t);
                rotation.z = Mathf.LerpAngle(oldZ, newZ, t);

                transform.eulerAngles = rotation;

                GetComponent<Rigidbody>().velocity = transform.forward.normalized * Mathf.Lerp(startSpeed, targetSpeed, t);
                yield return null;
            }

            yield return null;
        }
    }
}
