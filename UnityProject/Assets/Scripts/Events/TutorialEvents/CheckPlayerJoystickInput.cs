using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A tutorial function that checks if the player moved the left and right joysticks
/// </summary>
public class CheckPlayerJoystickInput : TutorialEvent
{
    public ControlArm_UsingPhysics leftJoystick;
    public ControlArm_UsingPhysics rightJoyStick;

    public bool isMovingBothJoystick; // Has the player moved both joystick

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (leftJoystick.joyStickLength > 0 && rightJoyStick.joyStickLength > 0)
        {
            isMovingBothJoystick = true;
        }

        if (isMovingBothJoystick)
        {
            eventFinished = true;
        }

        base.Update();
    }
}
