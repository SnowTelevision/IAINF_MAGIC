using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detect player body or arm collision with other objects in echo mode
/// </summary>
public class DetectCollisionInEchoMode : MonoBehaviour
{
    //public bool 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(transform.parent.name + ", " + collision.collider.transform.parent.name);

        if (PlayerInfo.isInEchoMode) // If the player is in echo mode
        {
            // If collide on object
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("CanCollideEcho") ||
                collision.collider.gameObject.layer == LayerMask.NameToLayer("CanCollideEchoRaycast"))
            {
                // If the object is not the player himself
                if (!collision.collider.GetComponentInParent<PlayerInfo>() &&
                    !collision.collider.GetComponentInParent<ArmUseItem>())
                {
                    if (GetComponentInParent<ArmUseItem>()) // If this is the player's armTip
                    {
                        PlayerInfo.sPlayerInfo.ArmTipCollideObjectHaptic();
                    }
                    else // If this is the body
                    {
                        PlayerInfo.sPlayerInfo.BodyCollideObjectHaptic();
                    }
                }
            }
        }
    }
}
