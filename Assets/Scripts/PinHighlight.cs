using UnityEngine;

public class EmissiveFlicker : MonoBehaviour
{
    [Header("Renderer")]
    public Renderer targetRenderer;

    [Header("Flicker Settings")]
    public Color baseEmissionColor = new Color(1.0f, 0.4f, 0.0f); // orange
    public float minIntensity = 0.8f;
    public float maxIntensity = 2.5f;
    public float flickerSpeed = 8f;

    private Material runtimeMaterial;

    void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            runtimeMaterial = targetRenderer.material; // instance, not shared
            runtimeMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (runtimeMaterial == null) return;

        float pulse = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);

        runtimeMaterial.SetColor("_EmissionColor", baseEmissionColor * intensity);
    }

    void OnDisable()
    {
        if (runtimeMaterial != null)
        {
            runtimeMaterial.SetColor("_EmissionColor", Color.black);
        }
    }
}