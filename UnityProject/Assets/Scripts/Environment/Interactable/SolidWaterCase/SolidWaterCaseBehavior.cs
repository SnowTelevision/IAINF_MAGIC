using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// Control the behavior of a solid water case
/// </summary>
public class SolidWaterCaseBehavior : MonoBehaviour
{
    public GameObject[] solidWaterGenerators; // The list of 8 solid water generators
    public Vector3[] UnitCubeCoords; // 8 coordinate points for an unit cube
    public Vector3 solidWaterCaseSize; // The size of the solid water case
    public GameObject solidWaterCase; // The solid water case gameObject
    public float targetCaseAlpha; // The alpha value of the material on the solid water case
    public float targetCaseEmissionIntensity; // The emission intensity of the material on the solid water case
    public float caseFormingDuration; // How long does the solid water case took to form

    public int arrivedGeneratorCount; // How many generators have arrived at target position to form solid water case

    // Use this for initialization
    void Start()
    {
        // Normalize the alpha
        targetCaseAlpha /= 255f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BehaviorTree>().SetVariableValue("ArrivedGeneratorCount", arrivedGeneratorCount);
    }

    /// <summary>
    /// Let the generator start flocking
    /// </summary>
    public void GeneratorStartFlock()
    {
        // Reset the arrived generator count
        arrivedGeneratorCount = 0;

        for (int i = 0; i < solidWaterGenerators.Length; i++)
        {
            // Start the generators flocking behavior
            solidWaterGenerators[i].GetComponent<SolidWaterGeneratorBehavior>().StartFlocking();
        }
    }

    /// <summary>
    /// Let each generator go to corresponding coords
    /// </summary>
    public void GeneratorsGoToCoords()
    {
        // Set up the target position for each generator
        for (int i = 0; i < solidWaterGenerators.Length; i++)
        {
            // Stop the generators flocking behavior
            solidWaterGenerators[i].GetComponent<SolidWaterGeneratorBehavior>().StopFlocking();

            // Assign relative target position
            solidWaterGenerators[i].GetComponent<SolidWaterGeneratorBehavior>().relativeTargetPosition =
                 new Vector3(UnitCubeCoords[i].x * solidWaterCaseSize.x * 0.5f,
                             UnitCubeCoords[i].y * solidWaterCaseSize.y * 0.5f,
                             UnitCubeCoords[i].z * solidWaterCaseSize.z * 0.5f);

            // Assign target rotation
            solidWaterGenerators[i].GetComponent<SolidWaterGeneratorBehavior>().targetRotation = Quaternion.LookRotation(UnitCubeCoords[i]);

            // Let the generators start moving to target position
            solidWaterGenerators[i].GetComponent<SolidWaterGeneratorBehavior>().StartMoveToTargetPosition();
        }
    }

    /// <summary>
    /// Destroy the solid water case
    /// </summary>
    public void DestroySolidWaterCase()
    {
        // Start changing the case material
        StartCoroutine(LerpMaterial(targetCaseAlpha, 0, targetCaseEmissionIntensity, 0));
    }

    /// <summary>
    /// Generate the solid water case
    /// </summary>
    public void CreateSolidWaterCase()
    {
        // Set the size of the solid water case
        solidWaterCase.transform.localScale = solidWaterCaseSize;

        // Start changing the case material
        StartCoroutine(LerpMaterial(0, targetCaseAlpha, 0, targetCaseEmissionIntensity));
    }

    /// <summary>
    /// Change the transparency and emission on the solid water case's material
    /// </summary>
    /// <returns></returns>
    public IEnumerator LerpMaterial(float startAlpha, float targetAlpha, float startEmission, float targetEmission)
    {
        // Set up lerping colors
        Color startColor = solidWaterCase.GetComponent<MeshRenderer>().material.color;
        startColor.a = startAlpha;
        Color targetColor = solidWaterCase.GetComponent<MeshRenderer>().material.color;
        targetColor.a = targetAlpha;
        Color emissionColor = solidWaterCase.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");

        // Lerp the material colors
        for (float t = 0; t < 1; t += Time.deltaTime / caseFormingDuration)
        {
            solidWaterCase.GetComponent<MeshRenderer>().material.color = Color.Lerp(startColor, targetColor, t);
            //solidWaterCase.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", GetHDRcolor.GetColorInHDR(emissionColor, Mathf.Lerp(startEmission, targetEmission, t)));
            yield return null;
        }
    }

    /*
    /// <summary>
    /// Add 1 to the arrivedGeneratorCount when a generator reached target position
    /// </summary>
    public void GeneratorReachedTargetPosition(int generatorIndex)
    {
        solidWaterGenerators[generatorIndex].GetComponent<BehaviorTree>().enabled = false;
        arrivedGeneratorCount++;
    }
    */
}
