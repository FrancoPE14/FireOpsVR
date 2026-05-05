using UnityEngine;

[RequireComponent(typeof(Light))]
public class HintLightFlicker : MonoBehaviour
{
    public float minIntensity = 1.2f;
    public float maxIntensity = 2.2f;
    public float flickerSpeed = 12f;

    private Light hintLight;
    private bool flickerOn = false;
    private float baseRange;

    void Awake()
    {
        hintLight = GetComponent<Light>();
        baseRange = hintLight.range;
        hintLight.enabled = false;
    }

    void Update()
    {
        if (!flickerOn || hintLight == null) return;

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        hintLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        hintLight.range = baseRange + noise * 0.05f;
    }

    public void StartFlicker()
    {
        if (hintLight == null) return;
        flickerOn = true;
        hintLight.enabled = true;
    }

    public void StopFlicker()
    {
        if (hintLight == null) return;
        flickerOn = false;
        hintLight.enabled = false;
    }
}