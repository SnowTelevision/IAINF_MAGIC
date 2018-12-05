using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showFPS : MonoBehaviour
{
    public Text fpsText;
    public float deltaTime;
    public float fixedDeltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();

        fixedDeltaTime += (Time.fixedDeltaTime - fixedDeltaTime) * 0.1f;
        fps = 1.0f / fixedDeltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
