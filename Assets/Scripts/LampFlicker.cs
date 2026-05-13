using UnityEngine;
using System.Collections;

/// <summary>
/// Drop on a Lamp prefab instance (or any GameObject with a Light in its children).
/// Lamp burns steadily, then occasionally flickers for a moment, then settles again.
/// Each lamp randomizes its own timing so multiple flickering lamps don't sync up.
/// </summary>
public class LampFlicker : MonoBehaviour
{
    [Header("Timing (seconds)")]
    [Tooltip("How long the lamp stays steady between flicker bursts. Random in this range.")]
    public Vector2 secondsBetweenBursts = new Vector2(4f, 14f);

    [Tooltip("How many on/off pulses per burst.")]
    public Vector2Int flickersPerBurst = new Vector2Int(2, 5);

    [Tooltip("How long the lamp stays dim during one pulse.")]
    public Vector2 offDuration = new Vector2(0.04f, 0.12f);

    [Tooltip("How long the lamp stays bright between pulses in a burst.")]
    public Vector2 onDuration = new Vector2(0.05f, 0.15f);

    [Header("Intensity")]
    [Tooltip("Multiplier applied to the lamp's base intensity when flickering off.")]
    [Range(0f, 1f)] public float dimMultiplier = 0.05f;

    Light _light;
    float _baseIntensity;

    void Start()
    {
        _light = GetComponentInChildren<Light>();
        if (_light == null)
        {
            Debug.LogWarning($"[LampFlicker] No Light found under {name}. Disabling.");
            enabled = false;
            return;
        }
        _baseIntensity = _light.intensity;
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        // Random initial offset so lamps don't all flicker in sync.
        yield return new WaitForSeconds(Random.Range(0f, secondsBetweenBursts.y));

        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(secondsBetweenBursts.x, secondsBetweenBursts.y));

            int pulses = Random.Range(flickersPerBurst.x, flickersPerBurst.y + 1);
            for (int i = 0; i < pulses; i++)
            {
                _light.intensity = _baseIntensity * dimMultiplier;
                yield return new WaitForSeconds(Random.Range(offDuration.x, offDuration.y));
                _light.intensity = _baseIntensity;
                yield return new WaitForSeconds(Random.Range(onDuration.x, onDuration.y));
            }

            _light.intensity = _baseIntensity; // ensure we end on bright
        }
    }
}
