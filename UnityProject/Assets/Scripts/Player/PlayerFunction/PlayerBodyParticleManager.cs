using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.Scripting.Effects;
using System;

/// <summary>
/// Control the particle effects on the player character's body and arms to reflect game event
/// </summary>
public class PlayerBodyParticleManager : MonoBehaviour
{
    public ParticleProfile bodyDefault; // The particle profile for when the body is default
    public ParticleProfile bodyExcited; // The particle profile for when the body is excited
    public ParticleProfile leftArmDefault; // The particle profile for when the left arm is default
    public ParticleProfile leftArmExcited; // The particle profile for when the left arm is excited
    public ParticleProfile leftArmTired; // The particle profile for when the left arm is tired
    public ParticleProfile rightArmDefault; // The particle profile for when the right arm is default
    public ParticleProfile rightArmExcited; // The particle profile for when the right arm is excited
    public ParticleProfile rightArmTired; // The particle profile for when the right arm is tired
    public ParticleProfile playerDead; // The particle profile for when the player is dead

    public ParticleProfile bodyCurrentProfile; // The current particle profile of the body
    public ParticleProfile leftArmCurrentProfile; // The current particle profile of the left arm
    public ParticleProfile rightArmCurrentProfile; // The current particle profile of the right arm
    public UpdatePlayerUI playerUIcontroller; // The controller for the player info UI

    // Use this for initialization
    void Start()
    {
        playerUIcontroller = FindObjectOfType<UpdatePlayerUI>();

        // Change the particle profiles to default for body and arms
        ChangeParticleSystemProfile(bodyDefault);
        ChangeParticleSystemProfile(leftArmDefault);
        ChangeParticleSystemProfile(rightArmDefault);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateArmParticleSystem(GameManager.sPlayer.GetComponent<PlayerInfo>().leftArmController);
        UpdateArmParticleSystem(GameManager.sPlayer.GetComponent<PlayerInfo>().rightArmController);
    }

    /// <summary>
    /// Change the profile for particle system(s)
    /// </summary>
    /// <param name="pp"></param>
    public void ChangeParticleSystemProfile(ParticleProfile pp)
    {
        for (int i = 0; i < pp.particleSystems.Length; i++)
        {
            var main = pp.particleSystems[i].main;
            // Change the color over lifetime
            var col = pp.particleSystems[i].colorOverLifetime;
            col.color = pp.colorOverLifttime;
            // Change the size
            main.startSize = pp.particleSize;
            // Change the speed
            main.startSpeed = pp.particleSpeed;
            // Turn on/off particle plexus for lines
            pp.particlePlexus[i].enabled = pp.createLines;
            // Change the material emission
            ParticleSystemRenderer psr = pp.particleSystems[i].GetComponent<ParticleSystemRenderer>();
            Color newColor = Color.white;
            newColor.a = pp.particleMaterialEmission;
            if (psr.material.HasProperty("_EmissionColor"))
            {
                psr.material.SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(psr.material.color, pp.particleMaterialEmission));
            }
            //// If the material does not have emission then just change the main color alpha
            //else if (psr.material.HasProperty("_TintColor"))
            //{
            //    psr.material.SetColor("_TintColor", newColor);
            //}
            //else
            //{
            //    psr.material.color = newColor;
            //}
        }
    }

    /// <summary>
    /// Update the arm's particle system based on if the arm is touching or carrying/holding an item, is the arm low stamina
    /// </summary>
    /// <param name="arm"></param>
    public void UpdateArmParticleSystem(ControlArm_UsingPhysics arm)
    {
        DetectCollision armTipCollision = arm.armTip.GetComponent<DetectCollision>();

        // If the armTip is touching or carrying/holding an item
        if (armTipCollision.isColliding || armTipCollision.isEnteringTrigger)
        {
            if (arm.isLeftArm && leftArmCurrentProfile != leftArmExcited)
            {
                leftArmCurrentProfile = leftArmExcited;
            }
            if (!arm.isLeftArm && rightArmCurrentProfile != rightArmExcited)
            {
                rightArmCurrentProfile = rightArmExcited;
            }
        }
        else
        {
            if (arm.isLeftArm && leftArmCurrentProfile != leftArmDefault)
            {
                leftArmCurrentProfile = leftArmDefault;
            }
            if (!arm.isLeftArm && rightArmCurrentProfile != rightArmDefault)
            {
                rightArmCurrentProfile = rightArmDefault;
            }
        }

        // If the arm is tired
        if (arm.armCurrentStamina / arm.armMaximumStamina <= playerUIcontroller.lowStaminaPercent)
        {
            if (arm.isLeftArm && leftArmCurrentProfile != leftArmTired)
            {
                leftArmCurrentProfile = leftArmTired;
            }
            if (!arm.isLeftArm && rightArmCurrentProfile != rightArmTired)
            {
                rightArmCurrentProfile = rightArmTired;
            }
        }

        // Update the arm particle systems
        if (arm.isLeftArm)
        {
            ChangeParticleSystemProfile(leftArmCurrentProfile);
        }
        else
        {
            ChangeParticleSystemProfile(rightArmCurrentProfile);
        }
    }
}

/// <summary>
/// Store the information for a particle state
/// </summary>
[Serializable]
public class ParticleProfile
{
    public ParticleSystem[] particleSystems; // The particle system(s) that has this profile
    public Gradient colorOverLifttime; // The color over lifetime of this profile
    public float particleSize; // The particle size of this profile
    public float particleSpeed; // The particle speed of this profile
    public float particleMaterialEmission; // The emission of the particle material of this profile
    public ParticlePlexus[] particlePlexus; // The particle plexus(s)
    public bool createLines; // Should this particle system turn on particle plexus of this profile
}