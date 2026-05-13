using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowingRotatingOrb : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f);

    [Header("Floating")]
    public bool floatUpAndDown = true;
    public float floatHeight = 0.25f;
    public float floatSpeed = 2f;

    [Header("Glow")]
    public Color glowColor = Color.cyan;
    public float minGlow = 1.5f;
    public float maxGlow = 4f;
    public float glowPulseSpeed = 2f;

    private Vector3 startPosition;
    private Renderer orbRenderer;
    private Material orbMaterial;

    void Start()
    {
        startPosition = transform.position;
        orbRenderer = GetComponent<Renderer>();

        // Make a unique material instance so this orb can glow without changing other objects.
        orbMaterial = orbRenderer.material;
        orbMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

        if (floatUpAndDown)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = startPosition + new Vector3(0f, yOffset, 0f);
        }

        float pulse = Mathf.Lerp(minGlow, maxGlow, (Mathf.Sin(Time.time * glowPulseSpeed) + 1f) / 2f);
        orbMaterial.SetColor("_EmissionColor", glowColor * pulse);
    }
}
